using Agnus.Models.DB;
using Agnus.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;
using Permissions = Agnus.Models.DB.Permissions;
using Agnus.Interfaces;
using Agnus.Interfaces.Repositories;
using Newtonsoft.Json.Linq;
using Microsoft.IdentityModel.Tokens;

namespace Agnus.Helpers
{
    public class TokenProcessor
    {
        public static async void ProcessToken(string token, DateTime expiration, string tenantId, List<Permissions>? Permissions)
        {
            var handler = new JwtSecurityTokenHandler();
            var jsontoken = handler.ReadToken(token) as JwtSecurityToken;

            var jwt = handler.ReadJwtToken(token);

            string userID = jsontoken.Claims.FirstOrDefault(f => f.Type == JwtRegisteredClaimNames.NameId)?.Value;
            string name = jsontoken.Claims.FirstOrDefault(f => f.Type == JwtRegisteredClaimNames.Name)?.Value;

            try
            {
                var credencial = new Credencial
                {
                    Nome = name,
                    Token = token,
                    TokenExpired = expiration,
                    ID = userID,
                    Permissions = Permissions,
                    tenantId = tenantId
                };

                // Serializa a credencial para JSON e a armazena usando criptografia.
                string userBasicInfoStr = JsonConvert.SerializeObject(credencial);
                await SaveToSecureStorageAsync("Credencial", userBasicInfoStr);

            }
            catch
            {

            }
        }

        public static async Task SaveToSecureStorageAsync(string key, string value)
        {
            if (DeviceInfo.Platform == DevicePlatform.Android || DeviceInfo.Platform == DevicePlatform.iOS)
            {
                // 🔹 Remove antes de sobrescrever, garantindo flush
                SecureStorage.Remove(key);
                await SecureStorage.SetAsync(key, value);
            }
            else if (DeviceInfo.Platform == DevicePlatform.WinUI)
            {
                Clear(key);
                byte[] data = Encoding.UTF8.GetBytes(value);
                byte[] encryptedData = ProtectedData.Protect(data, null, DataProtectionScope.CurrentUser);
                string filePath = System.IO.Path.Combine(FileSystem.AppDataDirectory, key);
                await File.WriteAllBytesAsync(filePath, encryptedData);
            }
        }

        public static string LoadFromSecureStorageAsync(string key)
        {
            if (DeviceInfo.Platform == DevicePlatform.Android || DeviceInfo.Platform == DevicePlatform.iOS)
            {
                // 🔹 Aguarda leitura real, sem cache antigo
                var result = SecureStorage.GetAsync(key).Result;
                return string.IsNullOrEmpty(result) ? null : result;
            }
            else if (DeviceInfo.Platform == DevicePlatform.WinUI)
            {
                string filePath = System.IO.Path.Combine(FileSystem.AppDataDirectory, key);
                if (!File.Exists(filePath))
                    return null;

                byte[] encryptedData = File.ReadAllBytes(filePath);
                byte[] data = ProtectedData.Unprotect(encryptedData, null, DataProtectionScope.CurrentUser);
                return Encoding.UTF8.GetString(data);
            }

            return null;
        }

        public static void Clear(string key)
        {
            if (DeviceInfo.Platform == DevicePlatform.Android || DeviceInfo.Platform == DevicePlatform.iOS)
            {
                SecureStorage.Remove(key);
            }
            else if (DeviceInfo.Platform == DevicePlatform.WinUI)
            {
                string filePath = System.IO.Path.Combine(FileSystem.AppDataDirectory, key);
                if (File.Exists(filePath))
                    File.Delete(filePath);
            }
        }
    }
}
