using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using FytSugar.Builder;

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

        public IActionResult OnPostGenerate([FromBody] BuilderModel createModel)
        {
            return new JsonResult(_builderService.CreateCode(createModel));
        }
    }
}
