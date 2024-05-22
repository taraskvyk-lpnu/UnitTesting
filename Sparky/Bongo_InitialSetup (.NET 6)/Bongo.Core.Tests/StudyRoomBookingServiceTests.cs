using Bongo.Core.Services;
using Bongo.DataAccess.Repository;
using Bongo.DataAccess.Repository.IRepository;
using Bongo.Models.Model;
using Bongo.Models.Model.VM;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bongo.Core.Tests
{
    [TestFixture]
    public class StudyRoomBookingServiceTests
    {
        StudyRoomBooking _request;
        private List<StudyRoom> _availableStudyRooms;
        private Mock<IStudyRoomBookingRepository> _studyRoomBookingRepoMock;
        private Mock<IStudyRoomRepository> _studyRoomRepoMock;
        private StudyRoomBookingService _bookingService;

        [SetUp]
        public void SetUp()
        {
            _request = new StudyRoomBooking()
            {
                FirstName = "Taras",
                LastName = "Kvyk",
                Email = "tasik981@gmail.com",
                Date = new DateTime(2024, 1, 1)
            };

            _availableStudyRooms = new()
            {
                new StudyRoom()
                {
                    Id = 10,
                    RoomName = "Lviv",
                    RoomNumber = "A22"
                }
            };

            _studyRoomBookingRepoMock = new Mock<IStudyRoomBookingRepository>();
            _studyRoomRepoMock = new();
            _studyRoomRepoMock.Setup(x => x.GetAll()).Returns(_availableStudyRooms);

            _bookingService = new StudyRoomBookingService(_studyRoomBookingRepoMock.Object, _studyRoomRepoMock.Object);
        }

        [Test]
        public void GetAllBooking_InvokedOnce_ReturnsTrue()
        {
            _bookingService.GetAllBooking();

            _studyRoomBookingRepoMock.Verify(u => u.GetAll(null), Times.AtLeastOnce);
        }

        [Test]
        public void BookStudyRoom_NullRequestParameter_ThrowsArgumentNullException()
        {
            _bookingService.GetAllBooking();

            var exception = Assert.Throws<ArgumentNullException>(() => _bookingService.BookStudyRoom(null));
            Assert.AreEqual("request", exception.ParamName);
        }

        [Test]
        public void BookStudyRoom_InvokeGetAll_VerifyChecked()
        {
            _bookingService.BookStudyRoom(_request);

            _studyRoomBookingRepoMock.Verify(u => u.GetAll(_request.Date), Times.AtLeastOnce);
            _studyRoomRepoMock.Verify(u => u.GetAll(), Times.Once);
        }

        [Test]
        public void AddBooking_InvokeGetAll_AddedToDb()
        {
            StudyRoomBooking savedRoomBooking = null;

            _studyRoomBookingRepoMock.Setup(x => x.Book(It.IsAny<StudyRoomBooking>()))
                .Callback<StudyRoomBooking>(booking =>
                {
                    savedRoomBooking = booking;
                });

            // Act
            _bookingService.BookStudyRoom(_request);

            //Assert
            _studyRoomBookingRepoMock.Verify(u => u.Book(It.IsAny<StudyRoomBooking>()), Times.Once);

            Assert.IsNotNull(savedRoomBooking);
            Assert.AreEqual(_request.FirstName, savedRoomBooking.FirstName);
            Assert.AreEqual(_request.LastName, savedRoomBooking.LastName);
            Assert.AreEqual(_request.Email, savedRoomBooking.Email);
            Assert.AreEqual(_request.Date, savedRoomBooking.Date);
            Assert.AreEqual(_availableStudyRooms.First().Id, savedRoomBooking.StudyRoomId);
        }

        [Test]
        public void StudyRoomBookingResultChecked_InputRequest_ValuesMatchResult()
        {
            StudyRoomBookingResult result = _bookingService.BookStudyRoom(_request);

            Assert.NotNull(result);
            Assert.AreEqual(_request.FirstName, result.FirstName);
            Assert.AreEqual(_request.LastName, result.LastName);
            Assert.AreEqual(_request.Email, result.Email);
            Assert.AreEqual(_request.Date, result.Date);
        }

        [TestCase(true, ExpectedResult = StudyRoomBookingCode.Success)]
        [TestCase(false, ExpectedResult = StudyRoomBookingCode.NoRoomAvailable)]
        public StudyRoomBookingCode ResultCode_RoomAvailable_ReturnsCode(bool availability)
        {
            if (!availability)
            {
                _availableStudyRooms.Clear();
            }

            return _bookingService.BookStudyRoom(_request).Code;
        }

        [TestCase(5, true)]
        public void AddBooking_BookRoomWithAvailability_ReturnsBookingId(int expectedBookingId, bool availability)
        {
            if (!availability)
            {
                _availableStudyRooms.Clear();
            }

            _studyRoomBookingRepoMock.Setup(x => x.Book(It.IsAny<StudyRoomBooking>()))
                .Callback<StudyRoomBooking>(booking =>
                {
                    booking.BookingId = 5;
                });

            var result = _bookingService.BookStudyRoom(_request);
            Assert.AreEqual(expectedBookingId, result.BookingId);
        }

        [Test]
        public void BookingNotInvoked_BookingNotAvailable_BookMethodNotInvoked()
        {
            _availableStudyRooms.Clear();
            var result = _bookingService.BookStudyRoom(_request);
            _studyRoomBookingRepoMock.Verify(u => u.Book(It.IsAny<StudyRoomBooking>()), Times.Never);
        }
    }
}