﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace AzNamingTool.Helpers
{
    public class FileSystemHelper
    {
        public static async Task<string> ReadFile(string fileName)
        {
            await CheckFile(fileName);
            string data = await File.ReadAllTextAsync("settings/" + fileName);
            return data;
        }

        public static async Task WriteFile(string fileName, string content)
        {
            await CheckFile(fileName);
            File.WriteAllText("settings/" + fileName, content);
        }

        public static async Task CheckFile(string fileName)
        {
            if (!File.Exists("settings/" + fileName))
            {
                var file = File.Create("settings/" + fileName);
                file.Close();

                for (int numTries = 0; numTries < 10; numTries++)
                {
                    try
                    {
                        await File.WriteAllTextAsync("settings/" + fileName, "[]");
                        return;
                    }
                    catch (IOException)
                    {
                        Thread.Sleep(50);
                    }
                }
            }
        }

        public static async Task<object> WriteConfiguation(object configdata, string configFileName)
        {
            try
            {

                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };
                
                await FileSystemHelper.WriteFile(configFileName, JsonSerializer.Serialize(configdata, options));
                return "Config updated.";
            }
            catch (Exception ex)
            {
                return ex;
            }
        }

        public static string UpdateAppSettings(IConfiguration config)
        {
            var jsonWriteOptions = new JsonSerializerOptions()
            {
                WriteIndented = true
            };
            jsonWriteOptions.Converters.Add(new JsonStringEnumConverter());

            var newJson = JsonSerializer.Serialize(config, jsonWriteOptions);

            var appSettingsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "settings/appsettings.json");
            File.WriteAllText(appSettingsPath, newJson);

            return "Success!";
        }
    }
}
