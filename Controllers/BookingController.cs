using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
// using gym.Data;
using gymI.Exceptions; 

using gymI.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace gymI.Controllers
{




public class BookingController : Controller
{
    private readonly ApplicationDbContext _context;

    public BookingController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var bookings = _context.Bookings.Include(b => b.Slot).ToList();
        return View(bookings);
    }

    public IActionResult Book(int id)
    {
        var slot = _context.Slots.FirstOrDefault(s => s.SlotID == id);

        if (slot == null)
        {
            return NotFound();
        }

        return View(slot);
    }

    [HttpPost]
public IActionResult Book(int id, int userId)
{
    var slot = _context.Slots.FirstOrDefault(s => s.SlotID == id);

    if (slot == null)
    {
        return NotFound();
    }

    if (slot.Capacity <= 0)
    {
        throw new SlotBookingException("Slot is full.");
    }

    var existingBooking = _context.Bookings.FirstOrDefault(b => b.SlotID == id && b.UserID == userId);

    if (existingBooking != null)
    {
        throw new SlotBookingException("You have already booked this slot.");
    }

    else{

    var booking = new Booking
    {
        SlotID = id,
        UserID = userId
    };

    _context.Bookings.Add(booking);
    slot.Capacity--;
    var res=_context.SaveChanges();
    if (res > 0)
    { return RedirectToAction("Index");
    }   
    }
         return RedirectToAction("Book");

}

}



}