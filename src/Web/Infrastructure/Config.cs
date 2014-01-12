﻿namespace Web.Infrastructure
{
    using System;
    using System.Web.Configuration;

    public static class Config
    {
        public static Func<string, object> GetValueFunc { get; set; }

        public static string GetValue(string key)
        {
            return GetValue<string>(key);
        }

        public static TValue GetValue<TValue>(string key)
        {
            EnsureGetValueFuncSet();

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

        private static void EnsureGetValueFuncSet()
        {
            if (GetValueFunc == null)
                GetValueFunc = WebConfigurationManager.AppSettings.Get;
        }
    }
}