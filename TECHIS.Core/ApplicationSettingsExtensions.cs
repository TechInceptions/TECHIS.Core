using System;
using System.Collections.Generic;
using System.Text;

namespace TECHIS.Core
{
    public static class ApplicationSettingsExtensions
    {
        public static TValue Get<TValue>(IApplicationSettings applicationSettings, string key, TValue value) where TValue : struct
        {
            var obj = applicationSettings.Get(key);

            if (string.IsNullOrEmpty(obj))
            {
                return value;
            }
            else
            {
                return (TValue)ObjectReader.ParseObject(obj, typeof(TValue));
            }
        }
        public static TValue Get<TValue>(IApplicationSettings applicationSettings, string key) where TValue : struct
        {
            return (TValue)ObjectReader.ParseObject(applicationSettings.Get(key), typeof(TValue));
        }
    }
}
