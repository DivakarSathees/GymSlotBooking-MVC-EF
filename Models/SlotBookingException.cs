using System;

namespace gymI.Exceptions
{
public class SlotBookingException : Exception
{
    public SlotBookingException(string message) : base(message)
    {
    }
}
}