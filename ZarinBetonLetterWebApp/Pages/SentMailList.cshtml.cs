using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Xceed.Words.NET;
using ZarinBetonLetterWebApp.Models;

namespace ZarinBetonLetterWebApp.Pages
{
    public class SentMailListModel : PageModel
    {
        AppDbContext _context;

        public SentMailListModel()
        {
            _context = new AppDbContext();
            _context.Database.EnsureCreated();
        }

        public void OnGet()
        {
        }


        public async Task<JsonResult> OnGetJson(int year, int month)
        {
            var _sentMails2 = await _context.SentMails.Where(a => a.Date.EndsWith($"{month}/{year}")).Select(a => new SentMailDto { Id = a.Id, Number = a.Number, Subject = a.Subject, Date = a.Date, Receiver = a.Receiver }).ToListAsync();
            return new JsonResult(new { data = _sentMails2 });
        }

        public async Task<IActionResult> OnGetView(int id)
        {
            var wordPath = await _context.Defaults.FirstOrDefaultAsync();
            if(wordPath!=null && !string.IsNullOrEmpty(wordPath.WordPath))
            {
                var letter = await _context.SentMails.FindAsync(id);
                var yearOdLetter = letter.Date.Split("/")[2];
                var slogan = await _context.Slogans.Where(a => a.Year.Equals(yearOdLetter)).FirstOrDefaultAsync();
                CreateDocument(wordPath.WordPath, letter.Number, letter.Format, letter.Date, slogan == null ? "" : slogan.Slogan1, letter.HasAttach ? "دارد" : "ندارد", letter.Title1, letter.Title2, letter.Subject, letter.Body);
                return Content("در حال باز کردن در ورد ...");
            }
            else
            {
                return Content("ابتدا مسیر اجرای ورد را در تنظیمات برنامه مشخص کنید");
            }
        }

        public void CreateDocument(string wordPath, string number, int format, string date, string slogan, string hasAttach, string title1, string title2, string subject, string body)
        {
            string fileName = @"TempA5.docx";
            if (format == 1)
            {
                fileName = @"TempA4.docx";
            }

            string tempFileName = fileName + "-temp.docx";

            if (System.IO.File.Exists(tempFileName))
            {
                System.IO.File.Delete(tempFileName);
            }

            System.IO.File.Copy(fileName, tempFileName);

            var doc = DocX.Load(tempFileName);

            doc.ReplaceText("[تاریخ - برنامه]", date);
            doc.ReplaceText("[شماره - برنامه]", number);
            doc.ReplaceText("[پیوست - برنامه]", hasAttach);
            doc.ReplaceText("[شعار سال - برنامه]", slogan);
            doc.ReplaceText("[تیتر اول - برنامه]", title1);
            doc.ReplaceText("[تیتر دوم - برنامه]", title2);
            doc.ReplaceText("[موضوع - برنامه]", "موضوع: " + subject);
            doc.ReplaceText("[متن نامه - برنامه]", body);

            doc.Save();

            Process.Start(wordPath, tempFileName);
        }

        public async Task OnGetDelete(int id)
        {
            var letter = await _context.SentMails.FindAsync(id);
            if (letter != null)
            {
                _context.SentMails.Remove(letter);
                await _context.SaveChangesAsync();
            }
        }

        public class SentMailDto
        {
            public int Id { get; set; }
            public string Number { get; set; }
            public string Subject { get; set; }
            public string Date { get; set; }
            public string Receiver { get; set; }
        }
    }
}
