using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
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

        [BindProperty]
        public YearMonthModel _yearMonth { get; set; }

        public void OnGet()
        {
        }

        //public List<SentMailDto> _sentMails { get; set; }

        public string _sentMails { get; set; }

        public async Task OnPost()
        {

        }

        public async Task<JsonResult> OnGetJson()
        {
            var _sentMails2 = await _context.SentMails/*.Where(a => a.Date.EndsWith($"{_yearMonth.Month}/{_yearMonth.Year}"))*/.Select(a => new SentMailDto { Id = a.Id, Number = a.Number, Subject = a.Subject, Date = a.Date, Receiver = a.Receiver }).ToListAsync();
            _sentMails = JsonSerializer.Serialize(_sentMails2);
            return new JsonResult(new { data = _sentMails2 });
        }

        public class YearMonthModel
        {
            public int Month { get; set; }
            public int Year { get; set; }
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
