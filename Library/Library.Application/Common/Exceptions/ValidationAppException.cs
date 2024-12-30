﻿namespace Library.Application.Common.Exceptions
{
    public class ValidationAppException : Exception
    {
        public IReadOnlyDictionary<string, string[]> Errors { get; set; }
        public ValidationAppException(IReadOnlyDictionary<string, string[]> errors)
            : base ("One or more validation errors occured")
            => Errors = errors;
    }
}
