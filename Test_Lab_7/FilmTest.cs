using Lab_7;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test_Lab_7
{
    [TestClass]
    public sealed class FilmTest
    {
        private Film film;

        [TestInitialize]
        public void Setup()
        {
            film = new Film();
        }

        [TestCleanup]
        public void Cleanup()
        {
            film = null;
            Film.ResetCounter();
        }

        //ТЕСТИ ВЛАСТИВОСТЕЙ (Properties)
        [TestMethod]
        public void Title_Set_ValidValue_Success()
        {
            // Arrange
            string validTitle = "Титанік";
            // Act
            film.Title = validTitle;
            // Assert
            Assert.AreEqual(validTitle, film.Title);
        }

        [TestMethod]
        public void Title_Set_TooShort_ThrowsException()
        {
            // Act
            bool exceptionThrown = false;
            try
            {
                film.Title = "Ab";
            }
            catch (ArgumentException)
            {
                exceptionThrown = true;
            }
            // Assert
            Assert.IsTrue(exceptionThrown);
        }

        [TestMethod]
        public void Title_Set_TooLong_ThrowsException()
        {
            // Act
            bool exceptionThrown = false;
            try
            {
                film.Title = "Дуже довга назва фільму більше ніж двадцять символів";
            }
            catch (ArgumentException)
            {
                exceptionThrown = true;
            }
            // Assert
            Assert.IsTrue(exceptionThrown);
        }

        [TestMethod]
        public void Title_Set_Null_ThrowsException()
        {
            // Act
            bool exceptionThrown = false;
            try
            {
                film.Title = null;
            }
            catch (ArgumentException)
            {
                exceptionThrown = true;
            }
            // Assert
            Assert.IsTrue(exceptionThrown);
        }

        [TestMethod]
        public void Title_Set_WhiteSpace_ThrowsException()
        {
            // Act
            bool exceptionThrown = false;
            try
            {
                film.Title = "   ";
            }
            catch (ArgumentException)
            {
                exceptionThrown = true;
            }
            // Assert
            Assert.IsTrue(exceptionThrown);
        }

        [TestMethod]
        public void Director_Set_ValidValue_Success()
        {
            // Arrange
            string validDirector = "Spielberg";
            // Act
            film.Director = validDirector;
            // Assert
            Assert.AreEqual(validDirector, film.Director);
        }

        [TestMethod]
        public void Director_Set_TooShort_ThrowsException()
        {
            // Act
            bool exceptionThrown = false;
            try
            {
                film.Director = "Ab";
            }
            catch (ArgumentException)
            {
                exceptionThrown = true;
            }
            // Assert
            Assert.IsTrue(exceptionThrown);
        }

        [TestMethod]
        public void Director_Set_ContainsDigits_ThrowsException()
        {
            // Act
            bool exceptionThrown = false;
            try
            {
                film.Director = "Nolan123";
            }
            catch (ArgumentException)
            {
                exceptionThrown = true;
            }
            // Assert
            Assert.IsTrue(exceptionThrown);
        }

        [TestMethod]
        public void Director_Set_Null_ThrowsException()
        {
            // Act
            bool exceptionThrown = false;
            try
            {
                film.Director = null;
            }
            catch (ArgumentException)
            {
                exceptionThrown = true;
            }
            // Assert
            Assert.IsTrue(exceptionThrown);
        }

        [TestMethod]
        public void ReleaseYear_Set_ValidValue_Success()
        {
            // Arrange
            int validYear = 2020;
            // Act
            film.ReleaseYear = validYear;
            // Assert
            Assert.AreEqual(validYear, film.ReleaseYear);
        }

        [TestMethod]
        public void ReleaseYear_Set_TooEarly_ThrowsException()
        {
            // Act
            bool exceptionThrown = false;
            try
            {
                film.ReleaseYear = 1959;
            }
            catch (ArgumentException)
            {
                exceptionThrown = true;
            }
            // Assert
            Assert.IsTrue(exceptionThrown);
        }

        [TestMethod]
        public void ReleaseYear_Set_TooLate_ThrowsException()
        {
            // Act
            bool exceptionThrown = false;
            try
            {
                film.ReleaseYear = 2026;
            }
            catch (ArgumentException)
            {
                exceptionThrown = true;
            }
            // Assert
            Assert.IsTrue(exceptionThrown);
        }

        [TestMethod]
        [DataRow(1960)]
        [DataRow(1990)]
        [DataRow(2025)]
        public void ReleaseYear_Set_BoundaryValues_Success(int year)
        {
            // Act
            film.ReleaseYear = year;
            // Assert
            Assert.AreEqual(year, film.ReleaseYear);
        }

        [TestMethod]
        public void SetRating_ValidValue_Success()
        {
            // Arrange
            double validRating = 8.5;
            // Act
            film.SetRating(validRating);
            // Assert
            Assert.AreEqual(validRating, film.Rating);
        }

        [TestMethod]
        public void SetRating_NegativeValue_ThrowsException()
        {
            // Act
            bool exceptionThrown = false;
            try
            {
                film.SetRating(-1.0);
            }
            catch (ArgumentException)
            {
                exceptionThrown = true;
            }
            // Assert
            Assert.IsTrue(exceptionThrown);
        }

        [TestMethod]
        public void SetRating_TooHigh_ThrowsException()
        {
            // Act
            bool exceptionThrown = false;
            try
            {
                film.SetRating(10.1);
            }
            catch (ArgumentException)
            {
                exceptionThrown = true;
            }
            // Assert
            Assert.IsTrue(exceptionThrown);
        }

        [TestMethod]
        [DataRow(0.0)]
        [DataRow(5.0)]
        [DataRow(10.0)]
        public void SetRating_BoundaryValues_Success(double rating)
        {
            // Act
            film.SetRating(rating);
            // Assert
            Assert.AreEqual(rating, film.Rating);
        }

        [TestMethod]
        public void SetRating_UpdatesAverageRating_Correctly()
        {
            // Arrange
            Film.ResetCounter();
            Film film1 = new Film();
            film1.SetRating(8.0);
            Film film2 = new Film();
            film2.SetRating(6.0);
            double expectedAverage = 7.0;
            // Act
            double actualAverage = Film.AverageRating;
            // Assert
            Assert.AreEqual(expectedAverage, actualAverage);
        }

        [TestMethod]
        public void Genre_Set_ValidValue_Success()
        {
            // Arrange
            FilmGenre validGenre = FilmGenre.ACTION;
            // Act
            film.Genre = validGenre;
            // Assert
            Assert.AreEqual(validGenre, film.Genre);
        }

        [TestMethod]
        public void Genre_Set_InvalidValue_ThrowsException()
        {
            // Act
            bool exceptionThrown = false;
            try
            {
                film.Genre = (FilmGenre)999;
            }
            catch (ArgumentException)
            {
                exceptionThrown = true;
            }
            // Assert
            Assert.IsTrue(exceptionThrown);
        }

        //ТЕСТИ КОНСТРУКТОРІВ

        [TestMethod]
        public void Constructor_Default_CreatesObjectWithDefaultValues()
        {
            // Act
            Film defaultFilm = new Film();
            // Assert
            Assert.AreEqual("Без назви", defaultFilm.Title);
            Assert.AreEqual("Невідомий", defaultFilm.Director);
            Assert.AreEqual(DateTime.Now.Year, defaultFilm.ReleaseYear);
            Assert.AreEqual(0.0, defaultFilm.Rating);
            Assert.AreEqual(FilmGenre.DRAMA, defaultFilm.Genre);
        }

        [TestMethod]
        public void Constructor_Default_IncrementsFilmCounter()
        {
            // Arrange
            Film.ResetCounter();
            int initialCount = Film.FilmCounter;
            // Act
            Film newFilm = new Film();
            // Assert
            Assert.AreEqual(initialCount + 1, Film.FilmCounter);
        }

        [TestMethod]
        public void Constructor_ThreeParameters_CreatesObjectWithDefaults()
        {
            // Arrange
            string title = "Inception";
            string director = "Nolan";
            int year = 2010;
            // Act
            Film film3Params = new Film(title, director, year);
            // Assert
            Assert.AreEqual(title, film3Params.Title);
            Assert.AreEqual(director, film3Params.Director);
            Assert.AreEqual(year, film3Params.ReleaseYear);
            Assert.AreEqual(5.0, film3Params.Rating);
            Assert.AreEqual(FilmGenre.DRAMA, film3Params.Genre);
        }

        [TestMethod]
        public void Constructor_AllParameters_CreatesObjectWithAllValues()
        {
            // Arrange
            string title = "Interstellar";
            string director = "Nolan";
            int year = 2014;
            double rating = 8.6;
            FilmGenre genre = FilmGenre.ACTION;
            // Act
            Film filmFull = new Film(title, director, year, rating, genre);
            // Assert
            Assert.AreEqual(title, filmFull.Title);
            Assert.AreEqual(director, filmFull.Director);
            Assert.AreEqual(year, filmFull.ReleaseYear);
            Assert.AreEqual(rating, filmFull.Rating);
            Assert.AreEqual(genre, filmFull.Genre);
        }

        [TestMethod]
        public void Constructor_Copy_CreatesExactCopy()
        {
            // Arrange
            Film original = new Film("Matrix", "Wachowski", 1999, 8.7, FilmGenre.ACTION);
            // Act
            Film copy = new Film(original);
            // Assert
            Assert.AreEqual(original.Title, copy.Title);
            Assert.AreEqual(original.Director, copy.Director);
            Assert.AreEqual(original.ReleaseYear, copy.ReleaseYear);
            Assert.AreEqual(original.Rating, copy.Rating);
            Assert.AreEqual(original.Genre, copy.Genre);
        }

        [TestMethod]
        public void Constructor_Copy_NullObject_ThrowsException()
        {
            // Act
            bool exceptionThrown = false;
            try
            {
                Film copy = new Film(null);
            }
            catch (ArgumentNullException)
            {
                exceptionThrown = true;
            }
            // Assert
            Assert.IsTrue(exceptionThrown);
        }

        //ТЕСТИ СТАТИЧНИХ МЕТОДІВ

        [TestMethod]
        public void GetRatingRecommendation_HighRating_ReturnsMasterpiece()
        {
            // Arrange
            double rating = 9.5;
            string expected = "Шедевр! Обов'язково до перегляду!";
            // Act
            string actual = Film.GetRatingRecommendation(rating);
            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [DataRow(9.0, "Шедевр! Обов'язково до перегляду!")]
        [DataRow(8.5, "Відмінний фільм, рекомендується!")]
        [DataRow(7.5, "Хороший фільм, варто подивитись.")]
        [DataRow(6.0, "Середній фільм, на любителя.")]
        [DataRow(4.0, "Низький рейтинг, дивитись не рекомендується.")]
        public void GetRatingRecommendation_DifferentRatings_ReturnsCorrectRecommendation(double rating, string expected)
        {
            // Act
            string actual = Film.GetRatingRecommendation(rating);
            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void IsCultClassic_OldHighRatedFilm_ReturnsTrue()
        {
            // Arrange
            int year = 1994;
            double rating = 8.5;
            // Act
            bool actual = Film.IsCultClassic(year, rating);
            // Assert
            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void IsCultClassic_RecentFilm_ReturnsFalse()
        {
            // Arrange
            int year = 2020;
            double rating = 9.0;
            // Act
            bool actual = Film.IsCultClassic(year, rating);
            // Assert
            Assert.IsFalse(actual);
        }

        [TestMethod]
        public void IsCultClassic_OldLowRatedFilm_ReturnsFalse()
        {
            // Arrange
            int year = 1980;
            double rating = 6.0;
            // Act
            bool actual = Film.IsCultClassic(year, rating);
            // Assert
            Assert.IsFalse(actual);
        }

        [TestMethod]
        [DataRow(1994, 8.5, true)]
        [DataRow(2020, 9.0, false)]
        [DataRow(1980, 6.0, false)]
        [DataRow(2005, 7.5, true)]
        public void IsCultClassic_VariousInputs_ReturnsCorrectResult(int year, double rating, bool expected)
        {
            // Act
            bool actual = Film.IsCultClassic(year, rating);
            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ResetCounter_ResetsAllStaticFields()
        {
            // Arrange - створюємо кілька фільмів
            Film.ResetCounter();
            new Film();
            new Film();
            // Act
            Film.ResetCounter();
            // Assert
            Assert.AreEqual(0, Film.FilmCounter);
            Assert.AreEqual(0.0, Film.AverageRating);
        }

        [TestMethod]
        public void FilmCounter_MultipleFilms_CountsCorrectly()
        {
            // Arrange
            Film.ResetCounter();
            // Act
            Film f1 = new Film();
            Film f2 = new Film();
            Film f3 = new Film();
            // Assert
            Assert.AreEqual(3, Film.FilmCounter);
        }

        //ТЕСТИ Parse() та TryParse()

        [TestMethod]
        public void Parse_ValidString_CreatesFilmObject()
        {
            // Arrange
            string validString = "Титанік,Cameron,1997,8.5,2";
            // Act
            Film parsedFilm = Film.Parse(validString);
            // Assert
            Assert.AreEqual("Титанік", parsedFilm.Title);
            Assert.AreEqual("Cameron", parsedFilm.Director);
            Assert.AreEqual(1997, parsedFilm.ReleaseYear);
            Assert.AreEqual(8.5, parsedFilm.Rating);
            Assert.AreEqual(FilmGenre.DRAMA, parsedFilm.Genre);
        }

        [TestMethod]
        public void Parse_NullString_ThrowsException()
        {
            // Act
            bool exceptionThrown = false;
            try
            {
                Film.Parse(null);
            }
            catch (ArgumentNullException)
            {
                exceptionThrown = true;
            }
            // Assert
            Assert.IsTrue(exceptionThrown);
        }

        [TestMethod]
        public void Parse_EmptyString_ThrowsException()
        {
            // Act
            bool exceptionThrown = false;
            try
            {
                Film.Parse("");
            }
            catch (ArgumentNullException)
            {
                exceptionThrown = true;
            }
            // Assert
            Assert.IsTrue(exceptionThrown);
        }

        [TestMethod]
        public void Parse_InsufficientFields_ThrowsException()
        {
            // Arrange
            string invalidString = "Аватар,Cameron";

            // Act
            bool exceptionThrown = false;
            try
            {
                Film.Parse(invalidString);
            }
            catch (FormatException)
            {
                exceptionThrown = true;
            }
            // Assert
            Assert.IsTrue(exceptionThrown);
        }

        [TestMethod]
        public void Parse_InvalidYearFormat_ThrowsException()
        {
            // Arrange
            string invalidString = "Матриця,Wachowski,abc,9.0,0";

            // Act
            bool exceptionThrown = false;
            try
            {
                Film.Parse(invalidString);
            }
            catch (FormatException)
            {
                exceptionThrown = true;
            }
            // Assert
            Assert.IsTrue(exceptionThrown);
        }

        [TestMethod]
        public void TryParse_ValidString_ReturnsTrue()
        {
            // Arrange
            string validString = "Початок,Nolan,2010,8.8,0";
            Film result;
            // Act
            bool success = Film.TryParse(validString, out result);
            // Assert
            Assert.IsTrue(success);
            Assert.IsNotNull(result);
            Assert.AreEqual("Початок", result.Title);
        }

        [TestMethod]
        public void TryParse_InvalidString_ReturnsFalse()
        {
            // Arrange
            string invalidString = "Матриця,Wachowski,abc,9.0,0";
            Film result;
            // Act
            bool success = Film.TryParse(invalidString, out result);
            // Assert
            Assert.IsFalse(success);
            Assert.IsNull(result);
        }

        [TestMethod]
        [DataRow("Film,Director,2020,8.0,0", true)]
        [DataRow("Film,Director,abc,8.0,0", false)]
        [DataRow("Film,Director", false)]
        [DataRow(null, false)]
        public void TryParse_VariousInputs_ReturnsCorrectResult(string input, bool expected)
        {
            // Arrange
            Film result;
            // Act
            bool actual = Film.TryParse(input, out result);
            // Assert
            Assert.AreEqual(expected, actual);
        }

        //ТЕСТИ НЕСТАТИЧНИХ МЕТОДІВ

        [TestMethod]
        public void YearsFromPremiere_CalculatesCorrectly()
        {
            // Arrange
            film.ReleaseYear = 2020;
            int expected = 5;
            // Act
            int actual = film.YearsFromPremiere;
            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void IsClassic_OldFilm_ReturnsTrue()
        {
            // Arrange
            film.ReleaseYear = 2000;
            // Act
            bool actual = film.IsClassic();
            // Assert
            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void IsClassic_NewFilm_ReturnsFalse()
        {
            // Arrange
            film.ReleaseYear = 2020;
            // Act
            bool actual = film.IsClassic();
            // Assert
            Assert.IsFalse(actual);
        }

        [TestMethod]
        public void IsModern_RecentFilm_ReturnsTrue()
        {
            // Arrange
            film.ReleaseYear = 2022;
            // Act
            bool actual = film.IsModern();
            // Assert
            Assert.IsTrue(actual);
        }
        [TestMethod]
        public void IsModern_OldFilm_ReturnsFalse()
        {
            // Arrange
            film.ReleaseYear = 2010;
            // Act
            bool actual = film.IsModern();
            // Assert
            Assert.IsFalse(actual);
        }
        [TestMethod]
        [DataRow(2000, "Класичний")]
        [DataRow(2015, "Недавній")]
        [DataRow(2023, "Сучасний")]
        public void GetAgeCategory_DifferentYears_ReturnsCorrectCategory(int year, string expected)
        {
            // Arrange
            film.ReleaseYear = year;
            // Act
            string actual = film.GetAgeCategory();
            // Assert
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void GetGenreName_Action_ReturnsCorrectName()
        {
            // Arrange
            film.Genre = FilmGenre.ACTION;
            // Act
            string actual = film.GetGenreName();
            // Assert
            Assert.AreEqual("Бойовик", actual);
        }
        [TestMethod]
        [DataRow(FilmGenre.ACTION, "Бойовик")]
        [DataRow(FilmGenre.COMEDY, "Комедія")]
        [DataRow(FilmGenre.DRAMA, "Драма")]
        [DataRow(FilmGenre.HORROR, "Жахи")]
        [DataRow(FilmGenre.ROMANCE, "Романтика")]
        public void GetGenreName_AllGenres_ReturnsCorrectNames(FilmGenre genre, string expected)
        {
            // Arrange
            film.Genre = genre;
            // Act
            string actual = film.GetGenreName();
            // Assert
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void IsHighRated_HighRating_ReturnsTrue()
        {
            // Arrange
            film.SetRating(8.5);
            // Act
            bool actual = film.IsHighRated();
            // Assert
            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void IsHighRated_LowRating_ReturnsFalse()
        {
            // Arrange
            film.SetRating(7.5);
            // Act
            bool actual = film.IsHighRated();
            // Assert
            Assert.IsFalse(actual);
        }

        [TestMethod]
        [DataRow(8.0, true)]
        [DataRow(7.9, false)]
        [DataRow(9.5, true)]
        public void IsHighRated_BoundaryValues_ReturnsCorrectResult(double rating, bool expected)
        {
            // Arrange
            film.SetRating(rating);
            // Act
            bool actual = film.IsHighRated();
            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToString_ReturnsCorrectFormat()
        {
            // Arrange
            Film testFilm = new Film("Matrix", "Wachowski", 1999, 8.7, FilmGenre.ACTION);
            string expected = "Matrix,Wachowski,1999,8.7,0";
            // Act
            string actual = testFilm.ToString();
            // Assert
            Assert.AreEqual(expected, actual);
        }

        //ТЕСТИ ПЕРЕВАНТАЖЕНИХ МЕТОДІВ GetInfo()
        [TestMethod]
        public void GetInfo_NoParameters_ReturnsBasicInfo()
        {
            // Arrange
            Film testFilm = new Film("Matrix", "Wachowski", 1999, 8.7, FilmGenre.ACTION);
            // Act
            string result = testFilm.GetInfo();
            // Assert
            Assert.IsTrue(result.Contains("Matrix"));
            Assert.IsTrue(result.Contains("Wachowski"));
            Assert.IsTrue(result.Contains("1999"));
            Assert.IsTrue(result.Contains("8.7"));
        }
        [TestMethod]
        public void GetInfo_DetailedTrue_ReturnsDetailedInfo()
        {
            // Arrange
            Film testFilm = new Film("Matrix", "Wachowski", 1999, 8.7, FilmGenre.ACTION);
            // Act
            string result = testFilm.GetInfo(true);
            // Assert
            Assert.IsTrue(result.Contains("Назва: Matrix"));
            Assert.IsTrue(result.Contains("Режисер: Wachowski"));
            Assert.IsTrue(result.Contains("Років з прем'єри"));
            Assert.IsTrue(result.Contains("Високорейтинговий"));
        }
        [TestMethod]
        public void GetInfo_DetailedFalse_ReturnsShortInfo()
        {
            // Arrange
            Film testFilm = new Film("Matrix", "Wachowski", 1999, 8.7, FilmGenre.ACTION);
            // Act
            string result = testFilm.GetInfo(false);
            // Assert
            Assert.IsTrue(result.Contains("Matrix"));
            Assert.IsTrue(result.Contains("(1999)"));
            Assert.IsTrue(result.Contains("8.7"));
        }
        [TestMethod]
        public void GetInfo_WithNumber_ReturnsNumberedInfo()
        {
            // Arrange
            Film testFilm = new Film("Matrix", "Wachowski", 1999, 8.7, FilmGenre.ACTION);
            int number = 5;
            // Act
            string result = testFilm.GetInfo(number);
            // Assert
            Assert.IsTrue(result.StartsWith("5."));
        }
        [TestMethod]
        public void GetInfo_SelectiveFields_ReturnsOnlySelectedFields()
        {
            // Arrange
            Film testFilm = new Film("Matrix", "Wachowski", 1999, 8.7, FilmGenre.ACTION);
            // Act
            string result = testFilm.GetInfo(true, false, false, true);
            // Assert
            Assert.IsTrue(result.Contains("Назва: Matrix"));
            Assert.IsTrue(result.Contains("Рейтинг: 8.7"));
            Assert.IsFalse(result.Contains("Режисер"));
            Assert.IsFalse(result.Contains("Рік"));
        }
        [TestMethod]
        public void GetAdditionalInfo_ReturnsAllAdditionalDetails()
        {
            // Arrange
            Film testFilm = new Film("Matrix", "Wachowski", 1999, 8.7, FilmGenre.ACTION);
            // Act
            string result = testFilm.GetAdditionalInfo();
            // Assert
            Assert.IsTrue(result.Contains("Років з прем'єри"));
            Assert.IsTrue(result.Contains("Категорія за віком"));
            Assert.IsTrue(result.Contains("Високорейтинговий"));
            Assert.IsTrue(result.Contains("Класичний"));
            Assert.IsTrue(result.Contains("Сучасний"));
        }
    }
}