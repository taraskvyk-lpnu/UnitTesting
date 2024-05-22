using Bongo.Core.Services.IServices;
using Bongo.Models.Model;
using Bongo.Models.Model.VM;
using Bongo.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bongo.Web.Tests
{
    [TestFixture]
    public class RoomBookingControllerTests
    {
        private Mock<IStudyRoomBookingService> _studyRoomBookingService;
        private RoomBookingController _roomBookingController;

        [SetUp]
        public void SrtUp()
        {
            _studyRoomBookingService = new Mock<IStudyRoomBookingService>();
            _roomBookingController = new RoomBookingController(_studyRoomBookingService.Object);
        }

        [Test]
        public void IndexPage_InvokeGetAll_Checked()
        {
            _roomBookingController.Index();
            _studyRoomBookingService.Verify(u => u.GetAllBooking(), Times.Once);
        }

        [Test]
        public void BookMethod_InvalidModelState_ReturnsViewWithNameBook()
        {
            _roomBookingController.ModelState.AddModelError("test", "test");

            var result = _roomBookingController.Book(new Models.Model.StudyRoomBooking());

            ViewResult viewResult = result as ViewResult;
            Assert.AreEqual("Book", viewResult.ViewName);
        }

        [Test]
        public void BookMethod_ValidModelStateAndSuccessCode_ReturnsViewWithNameBook()
        {
            _studyRoomBookingService.Setup(u => u.BookStudyRoom(It.IsAny<StudyRoomBooking>()))
                .Returns((StudyRoomBooking booking) => new StudyRoomBookingResult() 
                { 
                    Code = StudyRoomBookingCode.Success,
                    Email = booking.Email,
                    FirstName = booking.FirstName,
                    LastName = booking.LastName,
                    Date = booking.Date
                });

            var result = _roomBookingController.Book(new StudyRoomBooking()
            {
                FirstName = "Taras",
                LastName = "Kvyk",
                Email = "tasik981@gmail.com",
                Date = new DateTime(2024, 1, 1),
                StudyRoomId = 1
            });

            Assert.IsInstanceOf<RedirectToActionResult>(result);

            RedirectToActionResult actionResult = result as RedirectToActionResult;
            Assert.AreEqual(StudyRoomBookingCode.Success, actionResult.RouteValues["Code"]);
            Assert.AreEqual("Taras", actionResult.RouteValues["FirstName"]);
            Assert.AreEqual("BookingConfirmation", actionResult.ActionName);
        }

        [Test]
        public void BookMethod_ValidModelStateAndNoRoomAvailable_ReturnsErrorViewData()
        {
            // Arrange
            _studyRoomBookingService.Setup(u => u.BookStudyRoom(It.IsAny<StudyRoomBooking>()))
                .Returns(new StudyRoomBookingResult()
                { 
                    Code = StudyRoomBookingCode.NoRoomAvailable 
                });

            // Act
            var result = _roomBookingController.Book(new StudyRoomBooking());

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
            ViewResult actionResult = result as ViewResult;

            Assert.AreEqual("No Study Room available for selected date", actionResult.ViewData["Error"]);
        }
    }
}
