using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
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
            await _context.ReceivedMails.AddAsync(_receivedMail);
            await _context.SaveChangesAsync();
            return RedirectToPage("NewReceivedMail");
        }
    }
}
