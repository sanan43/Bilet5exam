using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace bilet5exam.Utilities.extension
{
    public static class FileExtensions
    {
        public static bool CheckContentType(this IFormFile file,string contentType)
        {
            return file.ContentType.ToLower().Trim().Contains(contentType.ToLower().Trim());
        }
        public static bool CheckFileSize(this IFormFile file,double size)
        {
            return file.Length / 1024 < size;
        }
        public async static Task<string> SaveAsync(this IFormFile file,string rootPath)
        {
            string fileName = Guid.NewGuid().ToString() + file.FileName;
            using(FileStream fileStream = new FileStream(Path.Combine(rootPath,fileName),
                FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }
            return fileName;
        }
    }
}
