using IndividualsDirectory.Models.Response;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace IndividualsDirectory.Helpers.Extensions
{
    public static class FileUploadExtensions
    {
        public static async Task<Response<string>> UploadFileIfExistsAsync(this IFormFileCollection files, IHostingEnvironment hostingEnvironment)
        {
            var response = new Response<string>();

            var file = files.FirstOrDefault();
            if (file == null && file.Length <= 0)
            {
                response.SetErrorMessages("Uploading image failed. Please try again.");
                return response;
            }

            var uploadPath = Path.Combine(hostingEnvironment.WebRootPath, "assets\\img");
            var fileName = Path.GetRandomFileName() + Path.GetExtension(file.FileName);

            using (var fileStream = new FileStream(Path.Combine(uploadPath, fileName), FileMode.Create))
            {
                await file.CopyToAsync(fileStream);

                response.SetSuccess();
                response.SetModel(fileName);

                return response;
            }
        }

        public static void DeleteFileIfExists(this IHostingEnvironment hostingEnvironment, string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return;

            var path = Path.Combine(hostingEnvironment.WebRootPath, "assets\\img", fileName);
            if (File.Exists(path) && !fileName.Equals("no-avatar.png"))
                File.Delete(path);
        }
    }
}
