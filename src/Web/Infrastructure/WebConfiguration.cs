﻿namespace Web.Infrastructure
{
    using System;

    public static class Config
    {
        public static Func<string, object> GetValueFunc { get; set; }

        public static string GetValue(string key)
        {
            return GetValue<string>(key);
        }

        public static TValue GetValue<TValue>(string key)
        {
            if (GetValueFunc == null)
            {
                throw new InvalidOperationException(
                    "You must set ReadValue method before trying to get value from configuration.");
            }

            object value = GetValueFunc(key);

            if (value == null)
            {
                return default(TValue);
            }

            // slight optimization
            if (value is TValue)
            {
                return (TValue) value;
            }

            return (TValue) Convert.ChangeType(value, typeof (TValue));
        }
    }
}