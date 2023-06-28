
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace gymI.Models

{

public class Booking
{
    public int BookingID { get; set; }
    public int SlotID { get; set; }
    public int UserID { get; set; }
    public Slot Slot { get; set; }
}
}