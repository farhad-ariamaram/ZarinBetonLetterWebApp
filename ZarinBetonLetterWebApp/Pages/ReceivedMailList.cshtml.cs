using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Xceed.Words.NET;
using ZarinBetonLetterWebApp.Models;

namespace ZarinBetonLetterWebApp.Pages
{
    public class ReceivedMailListModel : PageModel
    {

        AppDbContext _context;

        public ReceivedMailListModel()
        {
            _context = new AppDbContext();
            _context.Database.EnsureCreated();
        }

        public void OnGet()
        {
        }

        public async Task<JsonResult> OnGetJson(int year, int month)
        {
            var _receivedMails = await _context.ReceivedMails.Where(a => a.Date.EndsWith($"{month}/{year}")).Select(a => new SentMailDto { Id = a.Id, Number = a.Number, Subject = a.Subject, Date = a.Date, Sender = a.Sender }).ToListAsync();
            return new JsonResult(new { data = _receivedMails });
        }

        public async Task<IActionResult> OnGetView(int id)
        {
            return Content("عکس نامه نمایش داده شود");
        }

        public async Task OnGetDelete(int id)
        {
            var letter = await _context.ReceivedMails.FindAsync(id);
            if (letter != null)
            {
                _context.ReceivedMails.Remove(letter);
                await _context.SaveChangesAsync();
            }
        }

        public class SentMailDto
        {
            public int Id { get; set; }
            public string Number { get; set; }
            public string Subject { get; set; }
            public string Date { get; set; }
            public string Sender { get; set; }
        }

    }


}
