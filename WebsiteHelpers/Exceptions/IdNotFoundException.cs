﻿using System;

namespace WebsiteHelpers.Exceptions
{
    public class IdNotFoundException : Exception
    {
        public IdNotFoundException()
        {
        }

        public IdNotFoundException(string message) : base(message)
        {
        }

        public IdNotFoundException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
