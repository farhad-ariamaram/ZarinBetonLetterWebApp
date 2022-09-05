using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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
            _context.SentMails.Update(_sentMail);
            await _context.SaveChangesAsync();
            return RedirectToPage("EditSentMAil",new { id = _sentMail.Id});
        }
    }
}
