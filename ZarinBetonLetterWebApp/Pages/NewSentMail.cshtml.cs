using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Xceed.Words.NET;
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

        [BindProperty]
        public List<IFormFile> UploadAttaches { get; set; }

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
            if (_sentMail.HasAttach)
            {
                if (UploadAttaches != null && UploadAttaches.Count > 0)
                {
                    foreach (var item in UploadAttaches)
                    {
                        using (var reader = new BinaryReader(item.OpenReadStream()))
                        {
                            byte[] imageBytes = reader.ReadBytes((int)item.Length);
                            string base64String = Convert.ToBase64String(imageBytes);
                            _sentMail.Attaches = _sentMail.Attaches + "," + base64String;
                        }
                    }
                }

            }
            else
            {
                _sentMail.Attaches = null;
            }
            
            await _context.SentMails.AddAsync(_sentMail);
            await _context.SaveChangesAsync();

            var wordPath = await _context.Defaults.FirstOrDefaultAsync();
            if (wordPath != null && !string.IsNullOrEmpty(wordPath.WordPath))
            {
                var letter = await _context.SentMails.FindAsync(_sentMail.Id);
                var yearOdLetter = letter.Date.Split("/")[2];
                var slogan = await _context.Slogans.Where(a => a.Year.Equals(yearOdLetter)).FirstOrDefaultAsync();
                CreateDocument(wordPath.WordPath, letter.Number, letter.Format, letter.Date, slogan == null ? "" : slogan.Slogan1, letter.HasAttach ? "دارد" : "ندارد", letter.Title1, letter.Title2, letter.Subject, letter.Body);
            }

            return RedirectToPage("NewSentMail");
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
    }
}
