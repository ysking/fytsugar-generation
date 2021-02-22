using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using FytSugar.Builder;
using System.IO;

namespace FytSugarGeneration.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IBuilderService _builderService;

        public IndexModel(IBuilderService builderService)
        {
            _builderService = builderService;
        }

        public void OnGet()
        {

        }

        public IActionResult OnGetTable([FromQuery] BuilderConnection param)
        {
            return new JsonResult(_builderService.InitConnection(param));
        }

        public async Task<FileStreamResult> OnGetDownAsync([FromQuery] string filename)
        {
            var memoryStream = new MemoryStream();
            using (var stream = new FileStream(FileHelper.MapPath("/wwwroot/generate/zip/"+filename+".zip"), FileMode.Open))
            {
                await stream.CopyToAsync(memoryStream);
            }
            memoryStream.Seek(0, SeekOrigin.Begin);
            return new FileStreamResult(memoryStream, "application/octet-stream");
        }

        public IActionResult OnPostGenerate([FromBody] BuilderModel createModel)
        {
            return new JsonResult(_builderService.CreateCode(createModel));
        }

        public void OnGetDelete()
        {
            FileHelper.DeleteDir("/wwwroot/generate");
        }
    }
}
