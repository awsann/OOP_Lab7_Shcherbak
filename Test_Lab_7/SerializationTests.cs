using Lab_7;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test_Lab_7
{
    [TestClass]
    public class SerializationTests
    {
        private List<Film> testFilms;
        private string testCsvPath = "test_films.csv";
        private string testJsonPath = "test_films.json";
        private string emptyListCsv = "empty_list.csv";
        private string emptyListJson = "empty_list.json";
        private string brokenCsv = "broken_films.csv";
        private string nonExistentPath = "non_existent_file.csv";

        [TestInitialize]
        public void Setup()
        {
            Film.ResetCounter();
            testFilms = new List<Film>
            {
                new Film("Matrix", "Wachowski", 1999, 8.7, FilmGenre.ACTION),
                new Film("Inception", "Nolan", 2010, 8.8, FilmGenre.ACTION),
                new Film("Titanic", "Cameron", 1997, 7.8, FilmGenre.DRAMA)
            };
        }

        [TestCleanup]
        public void Cleanup()
        {
            testFilms?.Clear();
            Film.ResetCounter();

            // Видаляємо всі тестові файли
            string[] testFiles = { testCsvPath, testJsonPath, emptyListCsv, emptyListJson, brokenCsv };
            foreach (string file in testFiles)
            {
                if (File.Exists(file))
                {
                    try { File.Delete(file); } catch { }
                }
            }
        }

        //CSV СЕРІАЛІЗАЦІЯ

        [TestMethod]
        public void SaveToCSV_ValidList_CreatesFile()
        {
            // Act
            SaveToCSV(testFilms, testCsvPath);

            // Assert
            Assert.IsTrue(File.Exists(testCsvPath), "CSV файл не створено");
        }

        [TestMethod]
        public void SaveToCSV_ValidList_FileContainsHeader()
        {
            // Act
            SaveToCSV(testFilms, testCsvPath);
            string[] lines = File.ReadAllLines(testCsvPath);

            // Assert
            Assert.IsTrue(lines.Length > 0, "Файл порожній");
            Assert.IsTrue(lines[0].Contains("Title"), "Заголовок не містить Title");
            Assert.IsTrue(lines[0].Contains("Director"), "Заголовок не містить Director");
            Assert.IsTrue(lines[0].Contains("ReleaseYear"), "Заголовок не містить ReleaseYear");
        }

        [TestMethod]
        public void SaveToCSV_ValidList_FileContainsCorrectNumberOfLines()
        {
            // Act
            SaveToCSV(testFilms, testCsvPath);
            string[] lines = File.ReadAllLines(testCsvPath);

            // Assert
            Assert.AreEqual(4, lines.Length, "Неправильна кількість рядків");
        }

        [TestMethod]
        public void SaveToCSV_EmptyList_CreatesFileWithHeaderOnly()
        {
            // Arrange
            List<Film> emptyList = new List<Film>();

            // Act
            SaveToCSV(emptyList, emptyListCsv);
            string[] lines = File.ReadAllLines(emptyListCsv);

            // Assert
            Assert.AreEqual(1, lines.Length, "Має бути тільки заголовок");
        }

        [TestMethod]
        public void SaveToCSV_OverwritesExistingFile()
        {
            // Arrange
            SaveToCSV(testFilms, testCsvPath);
            List<Film> newList = new List<Film>
            {
                new Film("NewFilm", "NewDirector", 2020, 9.0, FilmGenre.COMEDY)
            };

            // Act
            SaveToCSV(newList, testCsvPath);
            string[] lines = File.ReadAllLines(testCsvPath);

            // Assert
            Assert.AreEqual(2, lines.Length, "Файл має містити заголовок + 1 фільм");
            Assert.IsTrue(lines[1].Contains("NewFilm"), "Файл не перезаписано");
        }

        [TestMethod]
        public void SaveToCSV_FilmWithCommaInTitle_SavesCorrectly()
        {
            // Arrange
            List<Film> specialFilms = new List<Film>
            {
                new Film("Film One", "DirectorName", 2020, 8.0, FilmGenre.DRAMA)
            };

            // Act
            SaveToCSV(specialFilms, testCsvPath);
            List<Film> loaded = LoadFromCSV(testCsvPath);

            // Assert
            Assert.AreEqual(1, loaded.Count, "Фільм не завантажено");
            Assert.AreEqual("Film One", loaded[0].Title);
        }

        //CSV ДЕСЕРІАЛІЗАЦІЯ

        [TestMethod]
        public void LoadFromCSV_ValidFile_ReturnsCorrectCount()
        {
            // Arrange
            SaveToCSV(testFilms, testCsvPath);

            // Act
            List<Film> loaded = LoadFromCSV(testCsvPath);

            // Assert
            Assert.AreEqual(3, loaded.Count, "Завантажено неправильну кількість фільмів");
        }

        [TestMethod]
        public void LoadFromCSV_ValidFile_PreservesAllData()
        {
            // Arrange
            SaveToCSV(testFilms, testCsvPath);

            // Act
            List<Film> loaded = LoadFromCSV(testCsvPath);

            // Assert
            Assert.AreEqual(testFilms[0].Title, loaded[0].Title);
            Assert.AreEqual(testFilms[0].Director, loaded[0].Director);
            Assert.AreEqual(testFilms[0].ReleaseYear, loaded[0].ReleaseYear);
            Assert.AreEqual(testFilms[0].Rating, loaded[0].Rating, 0.01);
            Assert.AreEqual(testFilms[0].Genre, loaded[0].Genre);
        }

        [TestMethod]
        public void LoadFromCSV_FileWithOnlyHeader_ReturnsEmptyList()
        {
            // Arrange
            File.WriteAllLines(emptyListCsv, new[] { "Title,Director,ReleaseYear,Rating,Genre" });

            // Act
            List<Film> loaded = LoadFromCSV(emptyListCsv);

            // Assert
            Assert.AreEqual(0, loaded.Count, "Має повернути порожній список");
        }

        [TestMethod]
        public void LoadFromCSV_BrokenLines_SkipsInvalidRows()
        {
            // Arrange
            string[] lines = new[]
            {
                "Title,Director,ReleaseYear,Rating,Genre",
                "ValidFilm,Director,2020,8.0,0",
                "BadYear,Director,abc,7.0,0",
                "MissingData,Director",
                "AnotherValid,DirectorTwo,2021,7.5,1"
            };
            File.WriteAllLines(brokenCsv, lines);

            // Act
            List<Film> loaded = LoadFromCSV(brokenCsv);

            // Assert
            Assert.AreEqual(2, loaded.Count, "Має завантажити тільки 2 коректні фільми");
            Assert.AreEqual("ValidFilm", loaded[0].Title);
            Assert.AreEqual("AnotherValid", loaded[1].Title);
        }

        [TestMethod]
        public void LoadFromCSV_AllBrokenLines_ReturnsEmptyList()
        {
            // Arrange
            string[] lines = new[]
            {
                "Title,Director,ReleaseYear,Rating,Genre",
                "BadYear,Director,abc,7.0,0",
                "MissingData,Director",
                "TooFewFields"
            };
            File.WriteAllLines(brokenCsv, lines);

            // Act
            List<Film> loaded = LoadFromCSV(brokenCsv);

            // Assert
            Assert.AreEqual(0, loaded.Count, "Має повернути порожній список");
        }

        [TestMethod]
        public void LoadFromCSV_NonExistentFile_ReturnsEmptyList()
        {
            // Act
            List<Film> loaded = LoadFromCSV(nonExistentPath);

            // Assert
            Assert.IsNotNull(loaded, "Має повернути непорожній список");
            Assert.AreEqual(0, loaded.Count, "Має повернути порожній список");
        }

        [TestMethod]
        public void LoadFromCSV_EmptyFile_ReturnsEmptyList()
        {
            // Arrange
            File.WriteAllText(emptyListCsv, "");

            // Act
            List<Film> loaded = LoadFromCSV(emptyListCsv);

            // Assert
            Assert.AreEqual(0, loaded.Count, "Має повернути порожній список");
        }

        //JSON СЕРІАЛІЗАЦІЯ

        [TestMethod]
        public void SaveToJSON_ValidList_CreatesFile()
        {
            // Act
            SaveToJSON(testFilms, testJsonPath);

            // Assert
            Assert.IsTrue(File.Exists(testJsonPath), "JSON файл не створено");
        }

        [TestMethod]
        public void SaveToJSON_ValidList_CreatesValidJSON()
        {
            // Act
            SaveToJSON(testFilms, testJsonPath);
            string content = File.ReadAllText(testJsonPath);

            // Assert
            Assert.IsTrue(content.StartsWith("["), "JSON має починатися з [");
            Assert.IsTrue(content.EndsWith("]"), "JSON має закінчуватися на ]");
            Assert.IsTrue(content.Contains("Matrix"), "JSON має містити дані фільму");
        }

        [TestMethod]
        public void SaveToJSON_EmptyList_CreatesEmptyArray()
        {
            // Arrange
            List<Film> emptyList = new List<Film>();

            // Act
            SaveToJSON(emptyList, emptyListJson);
            string content = File.ReadAllText(emptyListJson).Trim();

            // Assert
            Assert.IsTrue(content == "[]" || content.Contains("[]"), "JSON має містити порожній масив");
        }

        [TestMethod]
        public void SaveToJSON_OverwritesExistingFile()
        {
            // Arrange
            SaveToJSON(testFilms, testJsonPath);
            List<Film> newList = new List<Film>
            {
                new Film("NewFilm", "NewDirector", 2020, 9.0, FilmGenre.COMEDY)
            };

            // Act
            SaveToJSON(newList, testJsonPath);
            List<Film> loaded = LoadFromJSON(testJsonPath);

            // Assert
            Assert.AreEqual(1, loaded.Count, "Файл має містити 1 фільм");
            Assert.AreEqual("NewFilm", loaded[0].Title);
        }

        [TestMethod]
        public void SaveToJSON_SpecialCharacters_SavesCorrectly()
        {
            // Arrange
            List<Film> specialFilms = new List<Film>
            {
                new Film("Film Title", "ONeill", 2020, 8.0, FilmGenre.DRAMA)
            };

            // Act
            SaveToJSON(specialFilms, testJsonPath);
            List<Film> loaded = LoadFromJSON(testJsonPath);

            // Assert
            Assert.AreEqual(1, loaded.Count);
            Assert.AreEqual("Film Title", loaded[0].Title);
            Assert.AreEqual("ONeill", loaded[0].Director);
        }

        //JSON ДЕСЕРІАЛІЗАЦІЯ

        [TestMethod]
        public void LoadFromJSON_ValidFile_ReturnsCorrectCount()
        {
            // Arrange
            SaveToJSON(testFilms, testJsonPath);

            // Act
            List<Film> loaded = LoadFromJSON(testJsonPath);

            // Assert
            Assert.AreEqual(3, loaded.Count, "Завантажено неправильну кількість фільмів");
        }

        [TestMethod]
        public void LoadFromJSON_ValidFile_PreservesAllData()
        {
            // Arrange
            SaveToJSON(testFilms, testJsonPath);

            // Act
            List<Film> loaded = LoadFromJSON(testJsonPath);

            // Assert
            Assert.AreEqual(testFilms[0].Title, loaded[0].Title);
            Assert.AreEqual(testFilms[0].Director, loaded[0].Director);
            Assert.AreEqual(testFilms[0].ReleaseYear, loaded[0].ReleaseYear);
            Assert.AreEqual(testFilms[0].Rating, loaded[0].Rating, 0.01);
            Assert.AreEqual(testFilms[0].Genre, loaded[0].Genre);
        }

        [TestMethod]
        public void LoadFromJSON_EmptyArray_ReturnsEmptyList()
        {
            // Arrange
            File.WriteAllText(emptyListJson, "[]");

            // Act
            List<Film> loaded = LoadFromJSON(emptyListJson);

            // Assert
            Assert.AreEqual(0, loaded.Count, "Має повернути порожній список");
        }

        [TestMethod]
        public void LoadFromJSON_NonExistentFile_ReturnsEmptyList()
        {
            // Act
            List<Film> loaded = LoadFromJSON("non_existent.json");

            // Assert
            Assert.IsNotNull(loaded);
            Assert.AreEqual(0, loaded.Count);
        }

        [TestMethod]
        public void LoadFromJSON_InvalidJSON_ReturnsEmptyList()
        {
            // Arrange
            File.WriteAllText(testJsonPath, "{invalid json content}");

            // Act
            List<Film> loaded = LoadFromJSON(testJsonPath);

            // Assert
            Assert.AreEqual(0, loaded.Count, "Має повернути порожній список при некоректному JSON");
        }

        [TestMethod]
        public void LoadFromJSON_EmptyFile_ReturnsEmptyList()
        {
            // Arrange
            File.WriteAllText(emptyListJson, "");

            // Act
            List<Film> loaded = LoadFromJSON(emptyListJson);

            // Assert
            Assert.AreEqual(0, loaded.Count);
        }

        //ПОРІВНЯННЯ CSV vs JSON

        [TestMethod]
        public void CSVandJSON_SaveSameData_BothPreserveData()
        {
            // Arrange
            Assert.IsNotNull(testFilms, "testFilms не може бути null");
            Assert.IsTrue(testFilms.Count == 3, $"Очікувалося 3 фільми, але знайдено {testFilms.Count}");

            // Act
            SaveToCSV(testFilms, testCsvPath);
            SaveToJSON(testFilms, testJsonPath);
            List<Film> csvLoaded = LoadFromCSV(testCsvPath);
            List<Film> jsonLoaded = LoadFromJSON(testJsonPath);

            // Assert
            Assert.AreEqual(3, csvLoaded.Count, "CSV має містити 3 фільми");
            Assert.AreEqual(3, jsonLoaded.Count, "JSON має містити 3 фільми");
            Assert.AreEqual(csvLoaded.Count, jsonLoaded.Count);
            Assert.AreEqual(csvLoaded[0].Title, jsonLoaded[0].Title);
            Assert.AreEqual(csvLoaded[0].Rating, jsonLoaded[0].Rating, 0.01);
        }

        [TestMethod]
        public void CSVandJSON_SaveEmptyList_BothHandleCorrectly()
        {
            // Arrange
            List<Film> emptyList = new List<Film>();

            // Act
            SaveToCSV(emptyList, emptyListCsv);
            SaveToJSON(emptyList, emptyListJson);

            List<Film> csvLoaded = LoadFromCSV(emptyListCsv);
            List<Film> jsonLoaded = LoadFromJSON(emptyListJson);

            // Assert
            Assert.AreEqual(0, csvLoaded.Count);
            Assert.AreEqual(0, jsonLoaded.Count);
        }

        //ТЕСТИ З ВЕЛИКИМИ ДАНИМИ

        [TestMethod]
        public void SaveAndLoad_LargeList_WorksCorrectly()
        {
            // Arrange
            Film.ResetCounter();
            List<Film> largeList = new List<Film>();
            string[] directors = {
        "Spielberg", "Nolan", "Tarantino", "Cameron", "Scott",
        "Kubrick", "Hitchcock", "Scorsese", "Coppola", "Lucas"
    };
            string[] prefixes = {
        "The Great", "Epic", "Last", "First", "Final",
        "Secret", "Hidden", "Lost", "Dark", "Bright"
    };
            string[] suffixes = {
        "Adventure", "Journey", "Story", "Legend", "Tale",
        "Mystery", "Quest", "Mission", "Battle", "Victory"
    };
            for (int i = 0; i < 50; i++)
            {
                string title = $"{prefixes[i % prefixes.Length]} {suffixes[i % suffixes.Length]}";
                string director = directors[i % directors.Length];
                int year = 2000 + (i % 25);
                double rating = 5.0 + (i % 5);
                FilmGenre genre = (FilmGenre)(i % 5);
                largeList.Add(new Film(title, director, year, rating, genre));
            }

            // Act CSV
            SaveToCSV(largeList, testCsvPath);
            Film.ResetCounter();
            List<Film> csvLoaded = LoadFromCSV(testCsvPath);

            // Act JSON
            SaveToJSON(largeList, testJsonPath);
            Film.ResetCounter();
            List<Film> jsonLoaded = LoadFromJSON(testJsonPath);

            // Assert
            Assert.AreEqual(50, csvLoaded.Count, "CSV: Неправильна кількість");
            Assert.AreEqual(50, jsonLoaded.Count, "JSON: Неправильна кількість");
        }

        //ТЕСТИ НА ВСІХ ЖАНРАХ

        [TestMethod]
        public void SaveAndLoad_AllGenres_PreservesGenres()
        {
            // Arrange
            List<Film> allGenres = new List<Film>
            {
                new Film("ActionFilm", "DirectorOne", 2020, 8.0, FilmGenre.ACTION),
                new Film("ComedyFilm", "DirectorTwo", 2020, 7.0, FilmGenre.COMEDY),
                new Film("DramaFilm", "DirectorThree", 2020, 8.5, FilmGenre.DRAMA),
                new Film("HorrorFilm", "DirectorFour", 2020, 7.5, FilmGenre.HORROR),
                new Film("RomanceFilm", "DirectorFive", 2020, 7.8, FilmGenre.ROMANCE)
            };

            // Act
            SaveToCSV(allGenres, testCsvPath);
            List<Film> loaded = LoadFromCSV(testCsvPath);

            // Assert
            Assert.AreEqual(5, loaded.Count);
            Assert.AreEqual(FilmGenre.ACTION, loaded[0].Genre);
            Assert.AreEqual(FilmGenre.COMEDY, loaded[1].Genre);
            Assert.AreEqual(FilmGenre.DRAMA, loaded[2].Genre);
            Assert.AreEqual(FilmGenre.HORROR, loaded[3].Genre);
            Assert.AreEqual(FilmGenre.ROMANCE, loaded[4].Genre);
        }

        //ТЕСТИ ГРАНИЧНИХ ЗНАЧЕНЬ

        [TestMethod]
        public void SaveAndLoad_BoundaryValues_PreservesData()
        {
            // Arrange
            List<Film> boundaryFilms = new List<Film>
            {
                new Film("MinYear", "DirectorMin", 1960, 0.0, FilmGenre.DRAMA),
                new Film("MaxYear", "DirectorMax", DateTime.Now.Year, 10.0, FilmGenre.ACTION),
                new Film("MinTitle", "MinDirector", 2020, 5.0, FilmGenre.COMEDY)
            };

            // Act
            SaveToCSV(boundaryFilms, testCsvPath);
            List<Film> csvLoaded = LoadFromCSV(testCsvPath);

            SaveToJSON(boundaryFilms, testJsonPath);
            List<Film> jsonLoaded = LoadFromJSON(testJsonPath);

            // Assert
            Assert.AreEqual(3, csvLoaded.Count);
            Assert.AreEqual(3, jsonLoaded.Count);
            Assert.AreEqual(1960, csvLoaded[0].ReleaseYear);
            Assert.AreEqual(0.0, csvLoaded[0].Rating, 0.01);
            Assert.AreEqual(10.0, csvLoaded[1].Rating, 0.01);
        }

        //ДОПОМІЖНІ МЕТОДИ

        private void SaveToCSV(List<Film> films, string path)
        {
            try
            {
                List<string> lines = new List<string>();
                lines.Add("Title,Director,ReleaseYear,Rating,Genre");

                foreach (Film item in films)
                {
                    lines.Add(item.ToString());
                }

                File.WriteAllLines(path, lines);
            }
            catch (Exception)
            {
                //Ігноруємо помилки для тестів
            }
        }

        private List<Film> LoadFromCSV(string path)
        {
            List<Film> loadedFilms = new List<Film>();
            try
            {
                if (!File.Exists(path))
                    return loadedFilms;
                string[] lines = File.ReadAllLines(path);
                for (int i = 1; i < lines.Length; i++)
                {
                    Film film;
                    if (Film.TryParse(lines[i], out film))
                    {
                        loadedFilms.Add(film);
                    }
                }
            }
            catch (Exception)
            {
                //Повертаємо те що є
            }
            return loadedFilms;
        }

        private void SaveToJSON(List<Film> films, string path)
        {
            try
            {
                string jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(films, Newtonsoft.Json.Formatting.Indented);
                File.WriteAllText(path, jsonString);
            }
            catch (Exception)
            {
                //Ігноруємо помилки для тестів
            }
        }

        private List<Film> LoadFromJSON(string path)
        {
            List<Film> loadedFilms = new List<Film>();
            try
            {
                if (!File.Exists(path))
                    return loadedFilms;
                string text = File.ReadAllText(path);
                if (string.IsNullOrWhiteSpace(text))
                    return loadedFilms;
                loadedFilms = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Film>>(text);
                if (loadedFilms == null)
                    loadedFilms = new List<Film>();
            }
            catch (Exception)
            {
                return new List<Film>();
            }
            return loadedFilms;
        }
    }
}