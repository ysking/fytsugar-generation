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
        public async void OnGetAsync(string folder)
        {
            var path = FileHelper.MapPath(folder);
            using var stream = HttpContext.Response.BodyWriter.AsStream();
            await FileHelper.ReadDirectoryToZipStreamAsync(new DirectoryInfo(path), stream);
        }
    }
}
