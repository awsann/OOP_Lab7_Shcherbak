using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_7
{
    public class Film
    {
        // Приватні поля БЕЗ JSON атрибутів
        private string title;
        private string director;
        private int releaseYear;
        private FilmGenre genre;

        // Статичні поля (ігноруються при серіалізації)
        [JsonIgnore]
        private static int filmCounter = 0;

        [JsonIgnore]
        private static double averageRating = 0.0;

        [JsonIgnore]
        private static double totalRating = 0.0;

        // Статичні властивості
        public static int FilmCounter
        {
            get { return filmCounter; }
        }

        public static double AverageRating
        {
            get { return averageRating; }
        }

        // Обчислювана властивість (ігнорується при серіалізації)
        [JsonIgnore]
        public int YearsFromPremiere
        {
            get { return DateTime.Now.Year - releaseYear; }
        }

        // Конструктор за замовчуванням
        public Film()
        {
            title = "Без назви";
            director = "Невідомий";
            releaseYear = DateTime.Now.Year;
            Rating = 0.0;
            genre = FilmGenre.DRAMA;
            filmCounter++;
            totalRating += Rating;
            if (filmCounter > 0)
                averageRating = totalRating / filmCounter;
        }

        // Публічні властивості з валідацією та JSON атрибутами
        [JsonProperty("title")]
        public string Title
        {
            get { return title; }
            set
            {
                if (string.IsNullOrWhiteSpace(value) || value.Length < 3 || value.Length > 20)
                {
                    throw new ArgumentException("Помилка! Назва від 3 до 20 символів. Введіть ще раз: ");
                }
                title = value.Trim();
            }
        }

        [JsonProperty("director")]
        public string Director
        {
            get { return director; }
            set
            {
                if (string.IsNullOrWhiteSpace(value) || value.Length < 3 || value.Length > 20 || !IsOnlyLetters(value))
                {
                    throw new ArgumentException("Помилка! Режисер від 3 до 20 букв (тільки літери). Введіть ще раз: ");
                }
                director = value.Trim();
            }
        }

        [JsonProperty("year")]
        public int ReleaseYear
        {
            get { return releaseYear; }
            set
            {
                if (value < 1960 || value > DateTime.Now.Year)
                {
                    throw new ArgumentException($"Помилка! Рік від 1960 до {DateTime.Now.Year}. Введіть ще раз: ");
                }
                releaseYear = value;
            }
        }

        [JsonProperty("rating")]
        public double Rating { get; set; }

        [JsonProperty("genre")]
        public FilmGenre Genre
        {
            get { return genre; }
            set
            {
                if (!Enum.IsDefined(typeof(FilmGenre), value))
                {
                    throw new ArgumentException("Помилка! Невірне значення жанру.");
                }
                genre = value;
            }
        }

        // Приватний метод для перевірки
        private bool IsOnlyLetters(string text)
        {
            foreach (char c in text)
            {
                if (!char.IsLetter(c) && c != ' ')
                    return false;
            }
            return true;
        }

        // Метод для встановлення рейтингу
        public void SetRating(double value)
        {
            if (value < 0.0 || value > 10.0)
            {
                throw new ArgumentException("Помилка! Рейтинг від 0.0 до 10.0. Введіть ще раз: ");
            }
            totalRating = totalRating - Rating + value;
            Rating = value;
            if (filmCounter > 0)
                averageRating = totalRating / filmCounter;
        }

        // КОНСТРУКТОРИ
        public Film(string title, string director, int releaseYear)
            : this(title, director, releaseYear, 5.0, FilmGenre.DRAMA)
        {
        }

        [JsonConstructor]
        public Film(string title, string director, int year, double rating, FilmGenre genre)
        {
            this.Title = title;
            this.Director = director;
            this.ReleaseYear = year;
            this.Genre = genre;
            filmCounter++;
            this.SetRating(rating);
        }

        public Film(Film other)
        {
            if (other == null)
                throw new ArgumentNullException("Неможливо скопіювати null об'єкт!");
            this.title = other.title;
            this.director = other.director;
            this.releaseYear = other.releaseYear;
            this.genre = other.genre;
            filmCounter++;
            this.SetRating(other.Rating);
        }

        // СТАТИЧНІ МЕТОДИ
        public static string GetRatingRecommendation(double rating)
        {
            if (rating >= 9.0) return "Шедевр! Обов'язково до перегляду!";
            else if (rating >= 8.0) return "Відмінний фільм, рекомендується!";
            else if (rating >= 7.0) return "Хороший фільм, варто подивитись.";
            else if (rating >= 5.0) return "Середній фільм, на любителя.";
            else return "Низький рейтинг, дивитись не рекомендується.";
        }

        public static bool IsCultClassic(int year, double rating)
        {
            int age = DateTime.Now.Year - year;
            return (age >= 20 && rating >= 7.5);
        }

        public static void ResetCounter()
        {
            filmCounter = 0;
            totalRating = 0.0;
            averageRating = 0.0;
        }

        public override string ToString()
        {
            return $"{title},{director},{releaseYear},{Rating.ToString(System.Globalization.CultureInfo.InvariantCulture)},{(int)genre}";
        }

        public static Film Parse(string s)
        {
            if (string.IsNullOrEmpty(s))
                throw new ArgumentNullException("Рядок не може бути null або пустим!");
            string[] parts = s.Split(',');
            if (parts.Length != 5)
                throw new FormatException("Рядок не у правильному форматі! Очікується: Назва,Режисер,Рік,Рейтинг,Жанр");
            try
            {
                string title = parts[0].Trim();
                string director = parts[1].Trim();
                int year = int.Parse(parts[2].Trim());
                double rating = double.Parse(parts[3].Trim(), System.Globalization.CultureInfo.InvariantCulture);
                int genreInt = int.Parse(parts[4].Trim());
                if (genreInt < 0 || genreInt > 4)
                    throw new ArgumentException("Жанр має бути від 0 до 4!");
                FilmGenre genre = (FilmGenre)genreInt;
                return new Film(title, director, year, rating, genre);
            }
            catch (FormatException)
            {
                throw new FormatException("Помилка формату! Перевірте правильність введених даних.");
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException($"Помилка валідації: {ex.Message}");
            }
        }

        public static bool TryParse(string s, out Film film)
        {
            film = null;
            try
            {
                film = Parse(s);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
            catch (ArgumentNullException)
            {
                return false;
            }
            catch (ArgumentException)
            {
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        // ПЕРЕВАНТАЖЕНІ МЕТОДИ GetInfo()
        public string GetInfo()
        {
            return $"Назва: {title}, Режисер: {director}, Рік: {releaseYear}, Рейтинг: {Rating.ToString("F1", System.Globalization.CultureInfo.InvariantCulture)}, Жанр: {GetGenreName()}";
        }

        public string GetInfo(bool detailed)
        {
            if (detailed)
            {
                return $"Назва: {title}\n" +
                       $"Режисер: {director}\n" +
                       $"Рік випуску: {releaseYear}\n" +
                       $"Рейтинг: {Rating:F1}/10.0\n" +
                       $"Жанр: {GetGenreName()}\n" +
                       $"Років з прем'єри: {YearsFromPremiere}\n" +
                       $"Категорія за віком: {GetAgeCategory()}\n" +
                       $"Високорейтинговий: {(IsHighRated() ? "Так" : "Ні")}";
            }
            else
            {
                return $"{title} ({releaseYear}) - {Rating.ToString("F1", System.Globalization.CultureInfo.InvariantCulture)}";
            }
        }

        public string GetInfo(int number)
        {
            return $"{number}. {GetInfo()}";
        }

        public string GetInfo(bool showTitle, bool showDirector, bool showYear, bool showRating)
        {
            List<string> parts = new List<string>();
            if (showTitle) parts.Add($"Назва: {title}");
            if (showDirector) parts.Add($"Режисер: {director}");
            if (showYear) parts.Add($"Рік: {releaseYear}");
            if (showRating) parts.Add($"Рейтинг: {Rating.ToString("F1", System.Globalization.CultureInfo.InvariantCulture)}");
            return string.Join(", ", parts);
        }

        public bool IsClassic()
        {
            return YearsFromPremiere >= 20;
        }

        public bool IsModern()
        {
            return YearsFromPremiere <= 5;
        }

        public string GetAgeCategory()
        {
            if (YearsFromPremiere >= 20) return "Класичний";
            else if (YearsFromPremiere >= 5) return "Недавній";
            else return "Сучасний";
        }

        public string GetGenreName()
        {
            switch (genre)
            {
                case FilmGenre.ACTION: return "Бойовик";
                case FilmGenre.COMEDY: return "Комедія";
                case FilmGenre.DRAMA: return "Драма";
                case FilmGenre.HORROR: return "Жахи";
                case FilmGenre.ROMANCE: return "Романтика";
                default: return "Невідомий";
            }
        }

        public bool IsHighRated()
        {
            return Rating >= 8.0;
        }

        public string GetAdditionalInfo()
        {
            return $"Років з прем'єри: {YearsFromPremiere}\n" +
                   $"Категорія за віком: {GetAgeCategory()}\n" +
                   $"Високорейтинговий: {(IsHighRated() ? "Так" : "Ні")}\n" +
                   $"Класичний: {(IsClassic() ? "Так" : "Ні")}\n" +
                   $"Сучасний: {(IsModern() ? "Так" : "Ні")}";
        }
    }
}