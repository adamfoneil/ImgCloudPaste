﻿using JsonSettings;
using JsonSettings.Library;
using System;
using System.Security.Cryptography;

namespace ImgCloudPaste.Models
{
    public class Settings : SettingsBase, ICloneable
    {
        /// <summary>
        /// azure storage connection string
        /// </summary>
        [JsonProtect(DataProtectionScope.CurrentUser)]
        public string ConnectionString { get; set; }

        /// <summary>
        /// upload container
        /// </summary>
        [JsonProtect(DataProtectionScope.CurrentUser)]
        public string ContainerName { get; set; }

        public override string Filename => BuildPath(Environment.SpecialFolder.LocalApplicationData, "ImgCloudPaste", "settings.json");

        /// <summary>
        /// this is so we can display settings in a dialog and cancel out of it without affecting the original settings
        /// </summary>        
        public object Clone()
        {
            return new Settings()
            {
                ConnectionString = this.ConnectionString,
                ContainerName = this.ContainerName
            };
        }
    }
}
