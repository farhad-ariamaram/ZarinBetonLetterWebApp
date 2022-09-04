using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ZarinBetonLetterWebApp.Pages
{
    public class BackupModel : PageModel
    {

        public void OnGet()
        {

        }

        public FileResult OnGetBackup()
        {
            byte[] bytes = System.IO.File.ReadAllBytes("ZarinBetonLetters.db");
            return File(bytes, "application/octet-stream", $"ZarinBetonLetters_{DateTime.Now.ToShortDateString().Replace("/","-")}.db");
        }
    }
}
