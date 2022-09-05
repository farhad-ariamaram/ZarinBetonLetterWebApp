using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ZarinBetonLetterWebApp.Pages
{
    public class BackupModel : PageModel
    {

        public void OnGet()
        {

        }

        [BindProperty]
        public IFormFile Upload { get; set; }

        public async Task OnPost()
        {
            if (System.IO.File.Exists("ZarinBetonLetters.db"))
            {
                System.IO.File.Move("ZarinBetonLetters.db", $"ZarinBetonLetters_{DateTime.Now.ToString().Replace("/", "-").Replace(":", "-")}.db");
            }
            using (var fileStream = new FileStream("ZarinBetonLetters.db", FileMode.Create))
            {
                await Upload.CopyToAsync(fileStream);
            }
        }

        public FileResult OnGetBackup()
        {
            byte[] bytes = System.IO.File.ReadAllBytes("ZarinBetonLetters.db");
            return File(bytes, "application/octet-stream", $"ZarinBetonLetters_{DateTime.Now.ToShortDateString().Replace("/","-")}.db");
        }
    }
}
