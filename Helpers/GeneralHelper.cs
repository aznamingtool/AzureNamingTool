using AzNamingTool.Models;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AzNamingTool.Helpers
{
    public class GeneralHelper
    {
        //Function to get the Property Value
        public static object GetPropertyValue(object SourceData, string propName)
        {
            try
            {
                return SourceData.GetType().GetProperty(propName).GetValue(SourceData, null);
            }
            catch(Exception)
            {
                return null;
            }
        }

        public async static Task<List<T>> GetList<T>()
        {
            try
            {
                String data = String.Empty;
                data = typeof(T).Name switch
                {
                    nameof(ResourceComponent) => await FileSystemHelper.ReadFile("resourcecomponents.json"),
                    nameof(ResourceEnvironment) => await FileSystemHelper.ReadFile("resourceenvironments.json"),
                    nameof(ResourceLocation) => await FileSystemHelper.ReadFile("resourcelocations.json"),
                    nameof(ResourceOrg) => await FileSystemHelper.ReadFile("resourceorgs.json"),
                    nameof(ResourceProjAppSvc) => await FileSystemHelper.ReadFile("resourceprojappsvcs.json"),
                    nameof(ResourceType) => await FileSystemHelper.ReadFile("resourcetypes.json"),
                    nameof(ResourceUnitDept) => await FileSystemHelper.ReadFile("resourceunitdepts.json"),
                    nameof(ResourceVmRole) => await FileSystemHelper.ReadFile("resourcevmroles.json"),
                    nameof(ResourceDelimiter) => await FileSystemHelper.ReadFile("resourcedelimiters.json"),
                    _ => "[]",
                };
                var items = new List<T>();
                if (data != "[]")
                {
                    var options = new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                        PropertyNameCaseInsensitive = true
                    };

                    items = JsonSerializer.Deserialize<List<T>>(data, options);
                }

                return  items;
            }
            catch
            {
                throw;
            }
        }

        public async static Task WriteList<T>(List<T> items)
        {
            try
            {
                switch (typeof(T).Name)
                {
                    case nameof(ResourceComponent):
                        await FileSystemHelper.WriteConfiguation(items, "resourcecomponents.json");
                        break;
                    case nameof(ResourceEnvironment):
                        await FileSystemHelper.WriteConfiguation(items, "resourceenvironments.json");
                        break;
                    case nameof(ResourceLocation):
                        await FileSystemHelper.WriteConfiguation(items, "resourcelocations.json");
                        break;
                    case nameof(ResourceOrg):
                        await FileSystemHelper.WriteConfiguation(items, "resourceorgs.json");
                        break;
                    case nameof(ResourceProjAppSvc):
                        await FileSystemHelper.WriteConfiguation(items, "resourceprojappsvcs.json");
                        break;
                    case nameof(ResourceType):
                        await FileSystemHelper.WriteConfiguation(items, "resourcetypes.json");
                        break;
                    case nameof(ResourceUnitDept):
                        await FileSystemHelper.WriteConfiguation(items, "resourceunitdepts.json");
                        break;
                    case nameof(ResourceVmRole):
                        await FileSystemHelper.WriteConfiguation(items, "resourcevmroles.json");
                        break;
                    case nameof(ResourceDelimiter):
                        await FileSystemHelper.WriteConfiguation(items, "resourcedelimiters.json");
                        break;
                    default:
                        break;
                }
            }
            catch
            {
                throw;
            }
        }

        public static bool CheckNumeric(string value)
        {
            Regex regx = new Regex("^[0-9]+$");
            Match match = regx.Match(value);
            return match.Success;
        }

        public static bool CheckAlphanumeric(string value)
        {
            Regex regx = new Regex("^[a-zA-Z0-9]+$");
            Match match = regx.Match(value);
            return match.Success;
        }

        public static bool ValidatePassword(string text)
        {
            var hasNumber = new Regex(@"[0-9]+");
            var hasUpperChar = new Regex(@"[A-Z]+");
            var hasMinimum8Chars = new Regex(@".{8,}");

            var isValidated = hasNumber.IsMatch(text) && hasUpperChar.IsMatch(text) && hasMinimum8Chars.IsMatch(text);

            return isValidated;
        }

        public static string EncryptString(string text, string keyString)
        {
            byte[] iv = new byte[16];
            byte[] array;
            using (Aes aes = Aes.Create())
            {
                aes.KeySize = 256;
                aes.Key = Encoding.UTF8.GetBytes(keyString);
                aes.IV = iv;
                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                        {
                            streamWriter.Write(text);
                        }
                        array = memoryStream.ToArray();
                    }
                }
            }
            return Convert.ToBase64String(array);
        }

        public static string DecryptString(string cipherText, string keyString)
        {
            byte[] iv = new byte[16];
            byte[] buffer = Convert.FromBase64String(cipherText);
            using (Aes aes = Aes.Create())
            {
                aes.KeySize = 256;
                aes.Key = Encoding.UTF8.GetBytes(keyString);
                aes.IV = iv;
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
                using (MemoryStream memoryStream = new MemoryStream(buffer))
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                        {
                            return streamReader.ReadToEnd();
                        }
                    }
                }
            }
        }

        public static void VerifyConfiguration()
        {
            // Get all the files in teh repository folder
            DirectoryInfo dirRepository = new DirectoryInfo("repository");
            foreach(FileInfo file in dirRepository.GetFiles())
            {
                // Check if the file exists in the settings folder
                if(!File.Exists("settings/" + file.Name))
                {
                    // Copy the repository file to the settings folder
                    file.CopyTo("settings/" + file.Name);
                }
            }
        }

        public static void VerifySecurity(StateContainer state)
        {
            if (!state.Verified)
            {
                var config = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("settings/appsettings.json")
                .Build()
                .Get<Config>();

                if (config.SALTKey == "")
                {
                    // Create a new SALT key 
                    const string chars = "abcdefghijklmnopqrstuvwxyz0123456789";
                    Random random = new Random();
                    var salt = new string(Enumerable.Repeat(chars, 16)
                        .Select(s => s[random.Next(s.Length)]).ToArray());

                    config.SALTKey = salt.ToString();
                    config.APIKey = EncryptString(config.APIKey, salt.ToString());

                    if (config.AdminPassword != "")
                    {
                        config.AdminPassword = EncryptString(config.AdminPassword, config.SALTKey.ToString());
                        state.Password = true;
                    }
                    else
                    {
                        state.Password = false;
                    }

                }

                if (config.AdminPassword != "")
                {
                    state.Password = true;
                }
                else
                {
                    state.Password = false;
                }
                UpdateSettings(config);
            }
            state.SetVerified(true);
        }

        public static void UpdateSettings(Config config)
        {
            var jsonWriteOptions = new JsonSerializerOptions()
            {
                WriteIndented = true
            };
            jsonWriteOptions.Converters.Add(new JsonStringEnumConverter());

            var newJson = JsonSerializer.Serialize(config, jsonWriteOptions);

            var appSettingsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "settings/appsettings.json");
            File.WriteAllText(appSettingsPath, newJson);
        }

        public static void ResetState(StateContainer state)
        {
            state.SetVerified(false);
            state.SetAdmin(false);
            state.SetPassword(false);
        }

        public static bool ValidateShortName(string value, string type)
        {
            bool valid = false;

            switch (type)
            {
                case "ResourceEnvironment":
                    valid = true;
                    break;
                case "ResourceLocation":
                    valid = true;
                    break;
                case "ResourceOrg":
                    if (value.Length < 6)
                    {
                        valid = true;
                    }
                    break;
                case "ResourceProjAppSvc":
                    if (value.Length < 4)
                    {
                        valid = true;
                    }
                    break;
                case "ResourceType":
                    valid = true;
                    break;
                case "ResourceUnitDept":
                    if (value.Length < 4)
                    {
                        valid = true;
                    }
                    break;
                case "ResourceVmRole":
                    if (value.Length < 3)
                    {
                        valid = true;
                    }
                    break;
            }

            return valid;
        }
    }
}
