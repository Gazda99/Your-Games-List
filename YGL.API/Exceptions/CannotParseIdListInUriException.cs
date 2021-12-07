using System;

namespace YGL.API.Exceptions; 

public class CannotParseIdListInUriException : Exception {
    public const string BadValue = "Bad Value";
    public const string TooManyItems = "Too many items";
    public const string NonPositiveId = "Non positive Id";
    public CannotParseIdListInUriException() { }

    public CannotParseIdListInUriException(string message) : base(message) { }

    public CannotParseIdListInUriException(string message, Exception inner) : base(message, inner) { }
}