using NUnit.Framework;
using Bongo.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bongo.DataAccess.Repository;
using Bongo.Models.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace Bongo.DataAccess.Tests
{
    [TestFixture]
    public class StudyRoomBookingRepositoryTests
    {
        private StudyRoomBooking studyRoomBooking_One;
        private StudyRoomBooking studyRoomBooking_Two;
        private DbContextOptions<ApplicationDbContext> options;

        public StudyRoomBookingRepositoryTests()
        {
            studyRoomBooking_One = new StudyRoomBooking()
            {
                FirstName = "Taras1",
                LastName = "Kvyk1",
                Date = new DateTime(2024, 1, 1),
                Email = "tasik981@gmail.com",
                BookingId = 11,
                StudyRoomId = 1
            };

            studyRoomBooking_Two = new StudyRoomBooking()
            {
                FirstName = "Taras2",
                LastName = "Kvyk2",
                Date = new DateTime(2024, 2, 2),
                Email = "tasik982@gmail.com",
                BookingId = 22,
                StudyRoomId = 2
            };
        }

        [SetUp]
        public void SetUp()
        {
            options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "temp_Bongo").Options;
        }

        [Test]
        //[Order(0)]
        public void SaveBooking_InputBookingOne_CheckTheValuesFromDatabase()
        {
            // Arrange

            // Act
            using(var context = new ApplicationDbContext(options))
            {
                context.Database.EnsureDeleted();
                var repository = new StudyRoomBookingRepository(context);
                repository.Book(studyRoomBooking_One);
            }

            // Assert
            using (var context = new ApplicationDbContext(options))
            {
                var bookingFromDb = context.StudyRoomBookings.FirstOrDefault(u => u.BookingId == 11);

                Assert.AreEqual(studyRoomBooking_One.BookingId, bookingFromDb.BookingId);
                Assert.AreEqual(studyRoomBooking_One.FirstName, bookingFromDb.FirstName);
                Assert.AreEqual(studyRoomBooking_One.LastName, bookingFromDb.LastName);
                Assert.AreEqual(studyRoomBooking_One.Email, bookingFromDb.Email);
                Assert.AreEqual(studyRoomBooking_One.Date, bookingFromDb.Date);
            }
        }

        [Test]
        //[Order(1)]
        public void GetAllBooking_InputBookingOneAndTwo_CheckBookingsFromDatabase()
        {
            // Arrange
            var expectedResult = new List<StudyRoomBooking> { studyRoomBooking_One, studyRoomBooking_Two};

            using (var context = new ApplicationDbContext(options))
            {
                context.Database.EnsureDeleted();
                var repository = new StudyRoomBookingRepository(context);
                repository.Book(studyRoomBooking_One);
                repository.Book(studyRoomBooking_Two);
            }

            // Act
            List<StudyRoomBooking> actualList;

            using (var context = new ApplicationDbContext(options))
            {
                var repository = new StudyRoomBookingRepository(context);
                actualList = repository.GetAll(null).ToList();
            }

            // Assert
            using (var context = new ApplicationDbContext(options))
            {
                var bookingFromDb = context.StudyRoomBookings.FirstOrDefault(u => u.BookingId == 11);

                CollectionAssert.AreEqual(expectedResult, actualList, new BookingCompaarer());
            }
        }

        private class BookingCompaarer : IComparer
        {
            public int Compare(object? x, object? y)
            {
                var bookingOne = x as StudyRoomBooking;
                var bookingTwo = y as StudyRoomBooking;

                if(bookingOne.BookingId != bookingTwo.BookingId)
                    return 1;
                else
                    return 0;
            }
        }
    }
}
