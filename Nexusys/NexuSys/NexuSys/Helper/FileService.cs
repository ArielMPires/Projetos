using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace NexuSys.Helper
{
    public class FileService
    {
        private readonly IWebHostEnvironment _env;

        public FileService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public async Task<List<string>> SaveFilesAsync(
            List<byte[]> files,
            string folder,
            long maxSize = 5 * 1024 * 1024)
        {
            var savedPaths = new List<string>();
            try
            {


                if (files == null || files.Count == 0)
                    return savedPaths;

                var basePath = Path.Combine(
                    _env.WebRootPath,
                    "uploads",
                    folder
                );

                if (!Directory.Exists(basePath))
                    Directory.CreateDirectory(basePath);

                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };

                foreach (var file in files)
                {
                    if (file.Length > maxSize)
                        throw new Exception("Arquivo excede o limite permitido.");

                    // 🔹 Definir extensão (aqui você precisa decidir)
                    var extension = ".jpg"; // padrão (ou passe separado)

                    var fileName = $"{Guid.NewGuid()}{extension}";
                    var fullPath = Path.Combine(basePath, fileName);

                    await File.WriteAllBytesAsync(fullPath, file);

                    var relativePath = $"/uploads/{folder}/{fileName}";
                    savedPaths.Add(relativePath);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.InnerException.Message);
            }
            return savedPaths;

        }
    }
}
