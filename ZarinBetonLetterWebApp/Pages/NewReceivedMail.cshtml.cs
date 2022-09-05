using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ZarinBetonLetterWebApp.Models;

namespace ZarinBetonLetterWebApp.Pages
{
    public class NewReceivedMailModel : PageModel
    {
        AppDbContext _context;

        public NewReceivedMailModel()
        {
            _context = new AppDbContext();
            _context.Database.EnsureCreated();
        }

        [BindProperty]
        public ReceivedMail _receivedMail { get; set; }

        [BindProperty]
        public IFormFile Upload { get; set; }

        [BindProperty]
        public List<IFormFile> UploadAttaches { get; set; }

        public async Task OnGet()
        {
            _receivedMail = new ReceivedMail();

            DateTime NowDate = DateTime.Now;
            PersianCalendar pc = new PersianCalendar();
            int nowYear = pc.GetYear(NowDate);
            string Date = $"{pc.GetDayOfMonth(NowDate)}/{pc.GetMonth(NowDate)}/{nowYear}";
            _receivedMail.Date = Date;
        }

        public async Task<IActionResult> OnPost()
        {
            if (_receivedMail.HasAttach)
            {
                if (UploadAttaches != null && UploadAttaches.Count > 0)
                {
                    foreach (var item in UploadAttaches)
                    {
                        using (var reader = new BinaryReader(item.OpenReadStream()))
                        {
                            byte[] imageBytes = reader.ReadBytes((int)item.Length);
                            string base64String = Convert.ToBase64String(imageBytes);
                            _receivedMail.Attaches = _receivedMail.Attaches + "," + base64String;
                        }
                    }
                }
            }
            else
            {
                _receivedMail.Attaches = null;
            }

            if (Upload != null)
            {
                using (var reader = new BinaryReader(Upload.OpenReadStream()))
                {
                    byte[] imageBytes = reader.ReadBytes((int)Upload.Length);
                    string base64String = Convert.ToBase64String(imageBytes);
                    _receivedMail.LetterImage = base64String;
                }
            }
            await _context.ReceivedMails.AddAsync(_receivedMail);
            await _context.SaveChangesAsync();
            return RedirectToPage("NewReceivedMail");
        }
    }
}
