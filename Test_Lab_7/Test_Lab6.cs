using Lab_7;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test_Lab_7
{
    [TestClass]
    public class ProgramTest
    {
        private List<Film> testFilms;

        [TestInitialize]
        public void Setup()
        {
            Film.ResetCounter();
            testFilms = new List<Film>();
        }

        [TestCleanup]
        public void Cleanup()
        {
            testFilms.Clear();
            testFilms = null;
            Film.ResetCounter();
        }

        // ТЕСТИ ДОДАВАННЯ ФІЛЬМІВ (Add)
        [TestMethod]
        public void AddFilm_ToEmptyList_IncreasesCount()
        {
            // Arrange
            Film film = new Film("Matrix", "Wachowski", 1999, 8.7, FilmGenre.ACTION);
            int initialCount = testFilms.Count;

            // Act
            testFilms.Add(film);

            // Assert
            Assert.AreEqual(initialCount + 1, testFilms.Count);
        }

        [TestMethod]
        public void AddFilm_ToList_FilmIsPresent()
        {
            // Arrange
            Film film = new Film("Inception", "Nolan", 2010, 8.8, FilmGenre.ACTION);

            // Act
            testFilms.Add(film);

            // Assert
            Assert.IsTrue(testFilms.Contains(film));
        }

        [TestMethod]
        public void AddMultipleFilms_IncreasesCountCorrectly()
        {
            // Arrange
            Film film1 = new Film("Matrix", "Wachowski", 1999);
            Film film2 = new Film("Inception", "Nolan", 2010);
            Film film3 = new Film("Interstellar", "Nolan", 2014);

            // Act
            testFilms.Add(film1);
            testFilms.Add(film2);
            testFilms.Add(film3);

            // Assert
            Assert.AreEqual(3, testFilms.Count);
        }

        // ТЕСТИ ВИДАЛЕННЯ ФІЛЬМІВ ЗА ІНДЕКСОМ (RemoveAt)
        [TestMethod]
        public void RemoveFilmByIndex_ValidIndex_RemovesFilm()
        {
            // Arrange
            testFilms.Add(new Film("Matrix", "Wachowski", 1999));
            testFilms.Add(new Film("Inception", "Nolan", 2010));
            testFilms.Add(new Film("Interstellar", "Nolan", 2014));
            int initialCount = testFilms.Count;

            // Act
            testFilms.RemoveAt(1);

            // Assert
            Assert.AreEqual(initialCount - 1, testFilms.Count);
            Assert.AreEqual("Matrix", testFilms[0].Title);
            Assert.AreEqual("Interstellar", testFilms[1].Title);
        }

        [TestMethod]
        public void RemoveFilmByIndex_FirstElement_RemovesCorrectly()
        {
            // Arrange
            testFilms.Add(new Film("Matrix", "Wachowski", 1999));
            testFilms.Add(new Film("Inception", "Nolan", 2010));

            // Act
            testFilms.RemoveAt(0);

            // Assert
            Assert.AreEqual(1, testFilms.Count);
            Assert.AreEqual("Inception", testFilms[0].Title);
        }

        [TestMethod]
        public void RemoveFilmByIndex_LastElement_RemovesCorrectly()
        {
            // Arrange
            testFilms.Add(new Film("Matrix", "Wachowski", 1999));
            testFilms.Add(new Film("Inception", "Nolan", 2010));

            // Act
            testFilms.RemoveAt(testFilms.Count - 1);

            // Assert
            Assert.AreEqual(1, testFilms.Count);
            Assert.AreEqual("Matrix", testFilms[0].Title);
        }

        [TestMethod]
        public void RemoveFilmByIndex_InvalidIndex_ThrowsException()
        {
            // Arrange
            testFilms.Add(new Film("Matrix", "Wachowski", 1999));

            // Act
            bool exceptionThrown = false;
            try
            {
                testFilms.RemoveAt(5);
            }
            catch (ArgumentOutOfRangeException)
            {
                exceptionThrown = true;
            }

            // Assert
            Assert.IsTrue(exceptionThrown);
        }

        [TestMethod]
        public void RemoveFilmByIndex_NegativeIndex_ThrowsException()
        {
            // Arrange
            testFilms.Add(new Film("Matrix", "Wachowski", 1999));

            // Act
            bool exceptionThrown = false;
            try
            {
                testFilms.RemoveAt(-1);
            }
            catch (ArgumentOutOfRangeException)
            {
                exceptionThrown = true;
            }

            // Assert
            Assert.IsTrue(exceptionThrown);
        }
        // ТЕСТИ ВИДАЛЕННЯ ФІЛЬМІВ ЗА ЖАНРОМ (RemoveAll)
        [TestMethod]
        public void RemoveFilmsByGenre_ExistingGenre_RemovesAllMatching()
        {
            // Arrange
            testFilms.Add(new Film("Matrix", "Wachowski", 1999, 8.7, FilmGenre.ACTION));
            testFilms.Add(new Film("Titanic", "Cameron", 1997, 7.8, FilmGenre.DRAMA));
            testFilms.Add(new Film("Inception", "Nolan", 2010, 8.8, FilmGenre.ACTION));
            testFilms.Add(new Film("Avatar", "Cameron", 2009, 7.8, FilmGenre.ACTION));

            // Act
            int removedCount = testFilms.RemoveAll(f => f.Genre == FilmGenre.ACTION);

            // Assert
            Assert.AreEqual(3, removedCount);
            Assert.AreEqual(1, testFilms.Count);
            Assert.AreEqual(FilmGenre.DRAMA, testFilms[0].Genre);
        }

        [TestMethod]
        public void RemoveFilmsByGenre_NonExistingGenre_RemovesNothing()
        {
            // Arrange
            testFilms.Add(new Film("Matrix", "Wachowski", 1999, 8.7, FilmGenre.ACTION));
            testFilms.Add(new Film("Inception", "Nolan", 2010, 8.8, FilmGenre.ACTION));

            // Act
            int removedCount = testFilms.RemoveAll(f => f.Genre == FilmGenre.HORROR);

            // Assert
            Assert.AreEqual(0, removedCount);
            Assert.AreEqual(2, testFilms.Count);
        }

        [TestMethod]
        public void RemoveFilmsByGenre_AllSameGenre_RemovesAll()
        {
            // Arrange
            testFilms.Add(new Film("Matrix", "Wachowski", 1999, 8.7, FilmGenre.DRAMA));
            testFilms.Add(new Film("Titanic", "Cameron", 1997, 7.8, FilmGenre.DRAMA));
            testFilms.Add(new Film("Forrest Gump", "Zemeckis", 1994, 8.8, FilmGenre.DRAMA));

            // Act
            int removedCount = testFilms.RemoveAll(f => f.Genre == FilmGenre.DRAMA);

            // Assert
            Assert.AreEqual(3, removedCount);
            Assert.AreEqual(0, testFilms.Count);
        }

        // ТЕСТИ ПОШУКУ ФІЛЬМІВ ЗА НАЗВОЮ (FindAll)
        [TestMethod]
        public void FindFilmsByTitle_ExactMatch_ReturnsFilm()
        {
            // Arrange
            testFilms.Add(new Film("Matrix", "Wachowski", 1999));
            testFilms.Add(new Film("Inception", "Nolan", 2010));
            testFilms.Add(new Film("Interstellar", "Nolan", 2014));

            // Act
            List<Film> found = testFilms.FindAll(f => f.Title.ToLower().Contains("matrix"));

            // Assert
            Assert.AreEqual(1, found.Count);
            Assert.AreEqual("Matrix", found[0].Title);
        }

        [TestMethod]
        public void FindFilmsByTitle_PartialMatch_ReturnsAllMatching()
        {
            // Arrange
            testFilms.Add(new Film("Matrix", "Wachowski", 1999));
            testFilms.Add(new Film("Matrix Reloaded", "Wachowski", 2003));
            testFilms.Add(new Film("Inception", "Nolan", 2010));

            // Act
            List<Film> found = testFilms.FindAll(f => f.Title.ToLower().Contains("matrix"));

            // Assert
            Assert.AreEqual(2, found.Count);
        }

        [TestMethod]
        public void FindFilmsByTitle_NoMatch_ReturnsEmptyList()
        {
            // Arrange
            testFilms.Add(new Film("Matrix", "Wachowski", 1999));
            testFilms.Add(new Film("Inception", "Nolan", 2010));

            // Act
            List<Film> found = testFilms.FindAll(f => f.Title.ToLower().Contains("avatar"));

            // Assert
            Assert.AreEqual(0, found.Count);
        }

        [TestMethod]
        public void FindFilmsByTitle_CaseInsensitive_ReturnsFilm()
        {
            // Arrange
            testFilms.Add(new Film("Matrix", "Wachowski", 1999));

            // Act
            List<Film> found = testFilms.FindAll(f => f.Title.ToLower().Contains("MATRIX".ToLower()));

            // Assert
            Assert.AreEqual(1, found.Count);
        }

        // ТЕСТИ ПОШУКУ ФІЛЬМІВ ЗА РЕЖИСЕРОМ (FindAll)
        [TestMethod]
        public void FindFilmsByDirector_ExactMatch_ReturnsFilms()
        {
            // Arrange
            testFilms.Add(new Film("Matrix", "Wachowski", 1999));
            testFilms.Add(new Film("Inception", "Nolan", 2010));
            testFilms.Add(new Film("Interstellar", "Nolan", 2014));

            // Act
            List<Film> found = testFilms.FindAll(f => f.Director.ToLower().Contains("nolan"));

            // Assert
            Assert.AreEqual(2, found.Count);
            Assert.IsTrue(found.TrueForAll(f => f.Director == "Nolan"));
        }

        [TestMethod]
        public void FindFilmsByDirector_NoMatch_ReturnsEmptyList()
        {
            // Arrange
            testFilms.Add(new Film("Matrix", "Wachowski", 1999));
            testFilms.Add(new Film("Inception", "Nolan", 2010));

            // Act
            List<Film> found = testFilms.FindAll(f => f.Director.ToLower().Contains("spielberg"));

            // Assert
            Assert.AreEqual(0, found.Count);
        }

        [TestMethod]
        public void FindFilmsByDirector_PartialMatch_ReturnsFilms()
        {
            // Arrange
            testFilms.Add(new Film("Matrix", "Wachowski", 1999));
            testFilms.Add(new Film("Inception", "Nolan", 2010));

            // Act
            List<Film> found = testFilms.FindAll(f => f.Director.ToLower().Contains("nol"));

            // Assert
            Assert.AreEqual(1, found.Count);
            Assert.AreEqual("Nolan", found[0].Director);
        }

        // ТЕСТИ ФІЛЬТРАЦІЇ ВИСОКОРЕЙТИНГОВИХ ФІЛЬМІВ (FindAll)
        [TestMethod]
        public void FindHighRatedFilms_ReturnsOnlyHighRated()
        {
            // Arrange
            testFilms.Add(new Film("Matrix", "Wachowski", 1999, 8.7, FilmGenre.ACTION));
            testFilms.Add(new Film("Titanic", "Cameron", 1997, 7.5, FilmGenre.DRAMA));
            testFilms.Add(new Film("Inception", "Nolan", 2010, 8.8, FilmGenre.ACTION));

            // Act
            List<Film> highRated = testFilms.FindAll(f => f.IsHighRated());

            // Assert
            Assert.AreEqual(2, highRated.Count);
            Assert.IsTrue(highRated.TrueForAll(f => f.Rating >= 8.0));
        }

        [TestMethod]
        public void FindHighRatedFilms_NoHighRated_ReturnsEmptyList()
        {
            // Arrange
            testFilms.Add(new Film("Film1", "Director", 2000, 7.0, FilmGenre.DRAMA));
            testFilms.Add(new Film("Film2", "Director", 2001, 6.5, FilmGenre.COMEDY));

            // Act
            List<Film> highRated = testFilms.FindAll(f => f.IsHighRated());

            // Assert
            Assert.AreEqual(0, highRated.Count,
                $"Expected 0 high-rated films, but found {highRated.Count}. " +
                $"Films: [{string.Join(", ", highRated.Select(f => $"{f.Title}({f.Rating})"))}]");
        }

        // ТЕСТИ ФІЛЬТРАЦІЇ ЗА ЖАНРОМ (FindAll)
        [TestMethod]
        public void FindFilmsByGenre_ExistingGenre_ReturnsAllMatching()
        {
            // Arrange
            testFilms.Add(new Film("Matrix", "Wachowski", 1999, 8.7, FilmGenre.ACTION));
            testFilms.Add(new Film("Titanic", "Cameron", 1997, 7.8, FilmGenre.DRAMA));
            testFilms.Add(new Film("Inception", "Nolan", 2010, 8.8, FilmGenre.ACTION));

            // Act
            List<Film> actionFilms = testFilms.FindAll(f => f.Genre == FilmGenre.ACTION);

            // Assert
            Assert.AreEqual(2, actionFilms.Count);
            Assert.IsTrue(actionFilms.TrueForAll(f => f.Genre == FilmGenre.ACTION));
        }

        [TestMethod]
        public void FindFilmsByGenre_NonExistingGenre_ReturnsEmptyList()
        {
            // Arrange
            testFilms.Add(new Film("Matrix", "Wachowski", 1999, 8.7, FilmGenre.ACTION));
            testFilms.Add(new Film("Inception", "Nolan", 2010, 8.8, FilmGenre.ACTION));

            // Act
            List<Film> horrorFilms = testFilms.FindAll(f => f.Genre == FilmGenre.HORROR);

            // Assert
            Assert.AreEqual(0, horrorFilms.Count);
        }

        // ТЕСТИ ФІЛЬТРАЦІЇ КЛАСИЧНИХ ФІЛЬМІВ (FindAll)
        [TestMethod]
        public void FindClassicFilms_ReturnsOnlyClassic()
        {
            // Arrange
            testFilms.Add(new Film("Matrix", "Wachowski", 1999));
            testFilms.Add(new Film("Inception", "Nolan", 2010));
            testFilms.Add(new Film("Titanic", "Cameron", 1997));

            // Act
            List<Film> classicFilms = testFilms.FindAll(f => f.IsClassic());

            // Assert
            Assert.AreEqual(2, classicFilms.Count);
            Assert.IsTrue(classicFilms.TrueForAll(f => f.YearsFromPremiere >= 20));
        }

        [TestMethod]
        public void FindClassicFilms_NoClassic_ReturnsEmptyList()
        {
            // Arrange
            testFilms.Add(new Film("Film1", "Director", 2020));
            testFilms.Add(new Film("Film2", "Director", 2021));

            // Act
            List<Film> classicFilms = testFilms.FindAll(f => f.IsClassic());

            // Assert
            Assert.AreEqual(0, classicFilms.Count);
        }

        // ТЕСТИ ФІЛЬТРАЦІЇ СУЧАСНИХ ФІЛЬМІВ (FindAll)
        [TestMethod]
        public void FindModernFilms_ReturnsOnlyModern()
        {
            // Arrange
            testFilms.Add(new Film("Matrix", "Wachowski", 1999, 8.0, FilmGenre.ACTION));
            testFilms.Add(new Film("Film1", "Director", 2023, 7.0, FilmGenre.DRAMA));
            testFilms.Add(new Film("Film2", "Director", 2025, 7.0, FilmGenre.COMEDY));

            // Act
            List<Film> modernFilms = testFilms.FindAll(f => f.IsModern());

            // Assert
            Assert.AreEqual(2, modernFilms.Count, $"Expected 2 modern films but found {modernFilms.Count}");
            Assert.IsTrue(modernFilms.TrueForAll(f => f.YearsFromPremiere <= 5));
        }

        [TestMethod]
        public void FindModernFilms_NoModern_ReturnsEmptyList()
        {
            // Arrange
            testFilms.Add(new Film("Matrix", "Wachowski", 1999));
            testFilms.Add(new Film("Inception", "Nolan", 2010));

            // Act
            List<Film> modernFilms = testFilms.FindAll(f => f.IsModern());

            // Assert
            Assert.AreEqual(0, modernFilms.Count);
        }

        // ТЕСТИ ПЕРЕБИРАННЯ КОЛЕКЦІЇ (foreach)
        [TestMethod]
        public void ForeachIteration_ProcessesAllElements()
        {
            // Arrange
            testFilms.Add(new Film("Matrix", "Wachowski", 1999));
            testFilms.Add(new Film("Inception", "Nolan", 2010));
            testFilms.Add(new Film("Interstellar", "Nolan", 2014));
            int count = 0;

            // Act
            foreach (var film in testFilms)
            {
                count++;
            }

            // Assert
            Assert.AreEqual(testFilms.Count, count);
        }

        [TestMethod]
        public void ForeachIteration_CanAccessProperties()
        {
            // Arrange
            testFilms.Add(new Film("Matrix", "Wachowski", 1999, 8.7, FilmGenre.ACTION));
            testFilms.Add(new Film("Inception", "Nolan", 2010, 8.8, FilmGenre.ACTION));
            bool allAreAction = true;

            // Act
            foreach (var film in testFilms)
            {
                if (film.Genre != FilmGenre.ACTION)
                {
                    allAreAction = false;
                    break;
                }
            }

            // Assert
            Assert.IsTrue(allAreAction);
        }

        // ТЕСТИ ВЛАСТИВОСТІ Count
        [TestMethod]
        public void Count_EmptyList_ReturnsZero()
        {
            // Act & Assert
            Assert.AreEqual(0, testFilms.Count);
        }

        [TestMethod]
        public void Count_AfterAdding_ReturnsCorrectCount()
        {
            // Arrange & Act
            testFilms.Add(new Film("Matrix", "Wachowski", 1999));
            testFilms.Add(new Film("Inception", "Nolan", 2010));

            // Assert
            Assert.AreEqual(2, testFilms.Count);
        }

        [TestMethod]
        public void Count_AfterRemoving_ReturnsCorrectCount()
        {
            // Arrange
            testFilms.Add(new Film("Matrix", "Wachowski", 1999));
            testFilms.Add(new Film("Inception", "Nolan", 2010));
            testFilms.Add(new Film("Interstellar", "Nolan", 2014));

            // Act
            testFilms.RemoveAt(1);

            // Assert
            Assert.AreEqual(2, testFilms.Count);
        }

        // ТЕСТИ МЕТОДУ Contains
        [TestMethod]
        public void Contains_ExistingFilm_ReturnsTrue()
        {
            // Arrange
            Film film = new Film("Matrix", "Wachowski", 1999);
            testFilms.Add(film);

            // Act
            bool result = testFilms.Contains(film);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Contains_NonExistingFilm_ReturnsFalse()
        {
            // Arrange
            Film film1 = new Film("Matrix", "Wachowski", 1999);
            Film film2 = new Film("Inception", "Nolan", 2010);
            testFilms.Add(film1);

            // Act
            bool result = testFilms.Contains(film2);

            // Assert
            Assert.IsFalse(result);
        }

        // ТЕСТИ ІНДЕКСАТОРА
        [TestMethod]
        public void Indexer_ValidIndex_ReturnsCorrectFilm()
        {
            // Arrange
            testFilms.Add(new Film("Matrix", "Wachowski", 1999));
            testFilms.Add(new Film("Inception", "Nolan", 2010));

            // Act
            Film film = testFilms[1];

            // Assert
            Assert.AreEqual("Inception", film.Title);
        }

        [TestMethod]
        public void Indexer_InvalidIndex_ThrowsException()
        {
            // Arrange
            testFilms.Add(new Film("Matrix", "Wachowski", 1999));

            // Act
            bool exceptionThrown = false;
            try
            {
                Film film = testFilms[5];
            }
            catch (ArgumentOutOfRangeException)
            {
                exceptionThrown = true;
            }

            // Assert
            Assert.IsTrue(exceptionThrown, "Expected ArgumentOutOfRangeException was not thrown");
        }

        // ТЕСТИ МЕТОДУ Clear
        [TestMethod]
        public void Clear_RemovesAllElements()
        {
            // Arrange
            testFilms.Add(new Film("Matrix", "Wachowski", 1999));
            testFilms.Add(new Film("Inception", "Nolan", 2010));
            testFilms.Add(new Film("Interstellar", "Nolan", 2014));

            // Act
            testFilms.Clear();

            // Assert
            Assert.AreEqual(0, testFilms.Count);
        }

        // КОМПЛЕКСНІ ТЕСТИ
        [TestMethod]
        public void ComplexScenario_AddSearchRemove_WorksCorrectly()
        {
            // Arrange & Act - додавання
            testFilms.Add(new Film("Matrix", "Wachowski", 1999, 8.7, FilmGenre.ACTION));
            testFilms.Add(new Film("Inception", "Nolan", 2010, 8.8, FilmGenre.ACTION));
            testFilms.Add(new Film("Titanic", "Cameron", 1997, 7.5, FilmGenre.DRAMA));
            Assert.AreEqual(3, testFilms.Count);

            // Act - пошук
            List<Film> nolanFilms = testFilms.FindAll(f => f.Director.ToLower().Contains("nolan"));
            Assert.AreEqual(1, nolanFilms.Count);

            // Act - видалення
            testFilms.RemoveAll(f => f.Genre == FilmGenre.ACTION);

            // Assert
            Assert.AreEqual(1, testFilms.Count);
            Assert.AreEqual("Titanic", testFilms[0].Title);
        }

        [TestMethod]
        public void ComplexScenario_MultipleOperations_MaintainsConsistency()
        {
            // Додавання
            testFilms.Add(new Film("Gladiator", "Scott", 2000, 8.0, FilmGenre.ACTION));
            testFilms.Add(new Film("Hangover", "Phillips", 2010, 7.0, FilmGenre.COMEDY));
            testFilms.Add(new Film("Mad Max", "Miller", 2015, 9.0, FilmGenre.ACTION));

            // Перевірка кількості
            Assert.AreEqual(3, testFilms.Count);

            // Пошук за режисером
            List<Film> scottFilms = testFilms.FindAll(f => f.Director == "Scott");
            Assert.AreEqual(1, scottFilms.Count);

            // Видалення за індексом
            testFilms.RemoveAt(1);
            Assert.AreEqual(2, testFilms.Count);

            // Пошук високорейтингових
            List<Film> highRated = testFilms.FindAll(f => f.IsHighRated());
            Assert.AreEqual(2, highRated.Count);

            // Видалення за жанром
            int removed = testFilms.RemoveAll(f => f.Genre == FilmGenre.ACTION);
            Assert.AreEqual(2, removed);
            Assert.AreEqual(0, testFilms.Count);
        }
    }
}