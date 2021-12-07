using System;

namespace YGL.API.Exceptions; 

public class CannotSendEmailException : Exception {
    public CannotSendEmailException() { }

    public CannotSendEmailException(string message) : base(message) { }

    public CannotSendEmailException(string message, Exception inner) : base(message, inner) { }
}