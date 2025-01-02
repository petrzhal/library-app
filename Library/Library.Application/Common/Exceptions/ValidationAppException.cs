﻿namespace Library.Application.Common.Exceptions
{
    public class ValidationAppException(IReadOnlyDictionary<string, string[]> errors) : Exception("One or more validation errors occured")
    {
        public IReadOnlyDictionary<string, string[]> Errors { get; set; } = errors;
    }
}
