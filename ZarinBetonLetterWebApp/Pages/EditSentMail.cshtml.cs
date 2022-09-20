using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    public class EditSentMailModel : PageModel
    {
        AppDbContext _context;

        public EditSentMailModel()
        {
            _context = new AppDbContext();
            _context.Database.EnsureCreated();
        }

        [BindProperty]
        public SentMail _sentMail { get; set; }

        [BindProperty]
        public List<IFormFile> UploadAttaches { get; set; }

        public async Task<IActionResult> OnGet(int id)
        {
            _sentMail = await _context.SentMails.FindAsync(id);
            if (_sentMail == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (_sentMail.HasAttach)
            {
                if (UploadAttaches != null && UploadAttaches.Count > 0)
                {
                    _sentMail.Attaches = "";
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
            
            _context.SentMails.Update(_sentMail);
            await _context.SaveChangesAsync();
            return RedirectToPage("EditSentMail",new { id = _sentMail.Id});
        }

        public async Task<IActionResult> OnGetView(int id)
        {
            var wordPath = await _context.Defaults.FirstOrDefaultAsync();
            if (wordPath != null && !string.IsNullOrEmpty(wordPath.WordPath))
            {
                var letter = await _context.SentMails.FindAsync(id);
                var yearOdLetter = letter.Date.Split("/")[2];
                var slogan = await _context.Slogans.Where(a => a.Year == int.Parse(yearOdLetter)).FirstOrDefaultAsync();
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

            var allDocs = Directory.GetFiles(".", "*.docx", SearchOption.TopDirectoryOnly).ToList();

            foreach (var item in allDocs)
            {
                if (!item.Equals(".\\TempA5.docx") && !item.Equals(".\\TempA4.docx"))
                {
                    try
                    {
                        System.IO.File.Delete(item);
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }
            }

            string tempFileName = fileName + DateTime.Now.ToString().Replace("/", "-").Replace(":", "-").Replace(" ", "") + ".docx";

            System.IO.File.Copy(fileName, tempFileName);

            var doc = DocX.Load(tempFileName);
            doc.ReplaceText("[تاریخ - برنامه]", date ?? "");
            doc.ReplaceText("[شماره - برنامه]", number ?? "");
            doc.ReplaceText("[پیوست - برنامه]", hasAttach ?? "");
            doc.ReplaceText("[شعار سال - برنامه]", slogan ?? "");
            doc.ReplaceText("[تیتر اول - برنامه]", title1 ?? "");
            doc.ReplaceText("[تیتر دوم - برنامه]", title2 ?? "");
            doc.ReplaceText("[موضوع - برنامه]", "موضوع: " + subject);
            doc.ReplaceText("[متن نامه - برنامه]", body ?? "");


            doc.Save();

            Process.Start(wordPath, tempFileName);
        }
    }
}
