using System;
using System.Collections.Generic;
using System.Text;
// Helpers/Settings.cs
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace App.Helpers
{
    public static class Settings
    {
        static string mtsrouter = "http://192.168.1.106/WebAPI/";

        public static string connection = mtsrouter;

        private static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }


        public static string UserName
        {
            get
            {
                return AppSettings.GetValueOrDefault("UserName", "");
            }
            set
            {
                AppSettings.AddOrUpdateValue("UserName", value);
            }
        }

        public static string Password
        {
            get
            {
                return AppSettings.GetValueOrDefault("Password", "");
            }
            set
            {
                AppSettings.AddOrUpdateValue("Password", value);
            }
        }

        public static int UserID
        {
            get
            {
                return AppSettings.GetValueOrDefault("UserID", -1);
            }
            set
            {
                AppSettings.AddOrUpdateValue("UserID", value);
            }
        }

        public static string Token
        {
            get
            {
                return AppSettings.GetValueOrDefault("Token", "");
            }
            set
            {
                AppSettings.AddOrUpdateValue("Token", value);
            }
        }

    }
}
