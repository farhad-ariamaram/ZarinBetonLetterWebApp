using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ZarinBetonLetterWebApp.Models;

namespace ZarinBetonLetterWebApp.Pages
{
    public class NewSentMailModel : PageModel
    {
        AppDbContext _context;

        public NewSentMailModel()
        {
            _context = new AppDbContext();
            _context.Database.EnsureCreated();
        }

        [BindProperty]
        public SentMail _sentMail { get; set; }

        public async Task OnGet()
        {
            _sentMail = new SentMail();

            DateTime NowDate = DateTime.Now;
            PersianCalendar pc = new PersianCalendar();
            int nowYear = pc.GetYear(NowDate);
            string Date = $"{pc.GetDayOfMonth(NowDate)}/{pc.GetMonth(NowDate)}/{nowYear}";
            _sentMail.Date = Date;

            int lastNoOfYear = 0;
            var LastYearNumber = await _context.YearLastNos.Where(a => a.Year.Equals(nowYear)).FirstOrDefaultAsync();
            if (LastYearNumber == null)
            {
                YearLastNo yearLastNo = new YearLastNo
                {
                    Year = nowYear,
                    LastNo = 1000
                };
                await _context.YearLastNos.AddAsync(yearLastNo);
                await _context.SaveChangesAsync();
                lastNoOfYear = 1000;
            }
            else
            {
                LastYearNumber.LastNo++;
                _context.YearLastNos.Update(LastYearNumber);
                await _context.SaveChangesAsync();
                lastNoOfYear = LastYearNumber.LastNo;
            }
            _sentMail.Number = $"{lastNoOfYear}/{nowYear}";

        }

        public async Task<IActionResult> OnPost()
        {
            await _context.SentMails.AddAsync(_sentMail);
            await _context.SaveChangesAsync();
            return RedirectToPage("NewSentMail");
        }
    }
}
