using System.Collections.Generic;

namespace YGL.API.Errors {
public interface IErrorContainer<TEnum> {
    public Dictionary<TEnum, string> Errors { get; set; }
}
}