using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace gymI.Models
{

public class Slot
{
    public int SlotID { get; set; }
    public DateTime Time { get; set; }
    public int Duration { get; set; }
    public int Capacity { get; set; }
    public ICollection<Booking> Bookings { get; set; }
}
}