namespace AlexVanWolferen.Events.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    public static class StringExtensions
    {
        public static bool IsNullOrWhiteSpace(this string input)
        {
            return string.IsNullOrWhiteSpace(input);
        }
    }
}