using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FytSugar.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FytSugarGeneration.Pages
{
    public class DownModel : PageModel
    {
        public async Task<FileStreamResult> OnGet(string filename)
        {
            var memoryStream = new MemoryStream();
            using (var stream = new FileStream(FileHelper.MapPath("/wwwroot/generate/zip/" + filename + ".zip"), FileMode.Open))
            {
                await stream.CopyToAsync(memoryStream);
            }
            memoryStream.Seek(0, SeekOrigin.Begin);
            return new FileStreamResult(memoryStream, "application/octet-stream");
        }
    }
}
