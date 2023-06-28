using System.Collections.Generic;
using System.Linq;
using gymI.Controllers;
using gymI.Exceptions;
using gymI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace gymI.Tests
{
    [TestFixture]
    public class BookingControllerTests
    {
        private ApplicationDbContext _context;
        private BookingController _controller;
        private SlotController _slotcontroller;


        [SetUp]
        public void Setup()
        {
            // In-memory database for testing
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            _context = new ApplicationDbContext(options);
            _context.Database.EnsureCreated();

            _controller = new BookingController(_context);
            _slotcontroller = new SlotController(_context);

        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public void Index_ReturnsViewWithBookings()
        {
            // Arrange
            var bookings = new List<Booking>
            {
                new Booking { Slot = new Slot() },
                new Booking { Slot = new Slot() }
            };
            _context.Bookings.AddRange(bookings);
            _context.SaveChanges();

            // Act
            var result = _controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(bookings.Count, (result.Model as List<Booking>)?.Count);
        }

        [Test]
        public void Book_WithValidSlotId_ReturnsViewWithSlot()
        {
            // Arrange
            var slot = new Slot { SlotID = 1 };
            _context.Slots.Add(slot);
            _context.SaveChanges();

            // Act
            var result = _controller.Book(slot.SlotID) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(slot, result.Model as Slot);
        }

        [Test]
        public void Book_WithInvalidSlotId_ReturnsNotFound()
        {
            // Act
            var result = _controller.Book(1) as NotFoundResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void Book_WithValidSlotAndUser_BookingsUpdatedAndRedirectsToIndex()
        {
            // Arrange
            var slot = new Slot { SlotID = 1, Capacity = 2 };
            var userId = 1;
            _context.Slots.Add(slot);
            _context.SaveChanges();

            // Act
            var result = _controller.Book(slot.SlotID, userId) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
            Assert.AreEqual(1, _context.Bookings.Count());
            Assert.AreEqual(1, _context.Bookings.First().SlotID);
            Assert.AreEqual(1, _context.Bookings.First().UserID);
            // Assert.AreEqual(1, _context.Slots.First().Capacity);
        }

         [Test]
        public void Book_WithValidSlotAndCheckReduce_Capacity()
        {
            // Arrange
            var slot = new Slot { SlotID = 1, Capacity = 4 };
            var userId = 1;
            _context.Slots.Add(slot);
            _context.SaveChanges();

            // Act
            var result = _controller.Book(slot.SlotID, userId) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            // Assert.AreEqual("Index", result.ActionName);
            // Assert.AreEqual(1, _context.Bookings.Count());
            // Assert.AreEqual(1, _context.Bookings.First().SlotID);
            // Assert.AreEqual(1, _context.Bookings.First().UserID);
            Assert.AreEqual(3, _context.Slots.First().Capacity);
        }

        [Test]
        public void Book_WithInvalidSlot_ReturnsNotFound()
        {
            // Act
            var result = _controller.Book(1, 1) as NotFoundResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void Book_WithFullSlot_ThrowsSlotBookingException()
        {
            // Arrange
            var slot = new Slot { SlotID = 1, Capacity = 0 };
            var userId = 1;
            _context.Slots.Add(slot);
            _context.SaveChanges();

            // Act & Assert
            Assert.Throws<SlotBookingException>(() => _controller.Book(slot.SlotID, userId));
            // Console.WriteLine(ex.Message);
        }

        [Test]
        public void Book_WithExistingBooking_ThrowsSlotBookingException()
        {
            // Arrange
            var slot = new Slot { SlotID = 1, Capacity = 1 };
            var userId = 1;
            var booking = new Booking { SlotID = slot.SlotID, UserID = userId };
            _context.Slots.Add(slot);
            _context.Bookings.Add(booking);
            _context.SaveChanges();

            // Act & Assert
            Assert.Throws<SlotBookingException>(() => _controller.Book(slot.SlotID, userId));
        }

        [Test]
        public void Book_WithFullSlot_ThrowsSlotBookingExceptionWithMessage()
        {
            // Arrange
            var slot = new Slot { SlotID = 1, Capacity = 0 };
            var userId = 1;
            _context.Slots.Add(slot);
            _context.SaveChanges();

            // Act & Assert
            var exception=Assert.Throws<SlotBookingException>(() => _controller.Book(slot.SlotID, userId));

            // Assert
            Assert.AreEqual("Slot is full.", exception.Message);
        }

        [Test]
        public void Book_WithExistingBooking_ThrowsSlotBookingExceptionWithMessage()
        {
            // Arrange
            var slot = new Slot { SlotID = 1, Capacity = 1 };
            var userId = 1;
            var booking = new Booking { SlotID = slot.SlotID, UserID = userId };
            _context.Slots.Add(slot);
            _context.Bookings.Add(booking);
            _context.SaveChanges();

            // Act & Assert
            var ex= Assert.Throws<SlotBookingException>(() => _controller.Book(slot.SlotID, userId));
            // Console.WriteLine(ex);
            Assert.AreEqual("You have already booked this slot.", ex.Message);
        }

        [Test]
        public void Index_ReturnsViewWithListOfSlots()
        {
            // Arrange
            var slots = new List<Slot>
            {
                new Slot { SlotID = 1, Time = DateTime.Parse("10:00 AM"), Duration = 60, Capacity = 5 },
                new Slot { SlotID = 2, Time = DateTime.Parse("10:00 AM"), Duration = 45, Capacity = 3 }
            };
            _context.Slots.AddRange(slots);
            _context.SaveChanges();

            // Act
            var result = _slotcontroller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(slots.Count, (result.Model as List<Slot>)?.Count);
        }        
    }
}
