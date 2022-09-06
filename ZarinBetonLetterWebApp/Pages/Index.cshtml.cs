using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ZarinBetonLetterWebApp.Models;

namespace ZarinBetonLetterWebApp.Pages
{
    public class IndexModel : PageModel
    {
        AppDbContext _context;

        public IndexModel()
        {
            _context = new AppDbContext();
            _context.Database.EnsureCreated();
        }

        public async Task OnGet()
        {
            var wordPath = await _context.Defaults.FirstOrDefaultAsync();
            if (wordPath == null)
            {
                List<string> files, files2;
                files = new List<string>();
                files2 = new List<string>();

                try
                {
                    files = Directory.GetFiles(@"C:\Program Files\Microsoft Office", "WINWORD.EXE", SearchOption.AllDirectories).ToList();
                    files2 = Directory.GetFiles(@"C:\Program Files (x86)\Microsoft Office", "WINWORD.EXE", SearchOption.AllDirectories).ToList();
                }
                catch (System.Exception){}

                if (files.Count() > 0)
                {
                    Default _default = new Default();
                    _default.WordPath = files.FirstOrDefault();
                    await _context.Defaults.AddAsync(_default);
                    await _context.SaveChangesAsync();

                }
                else if(files2.Count() > 0)
                {
                    Default _default = new Default();
                    _default.WordPath = files2.FirstOrDefault();
                    await _context.Defaults.AddAsync(_default);
                    await _context.SaveChangesAsync();
                }
            }
            else if (wordPath.WordPath == null || string.IsNullOrEmpty(wordPath.WordPath))
            {
                List<string> files, files2;
                files = new List<string>();
                files2 = new List<string>();

                try
                {
                    files = Directory.GetFiles(@"C:\Program Files\Microsoft Office", "WINWORD.EXE", SearchOption.AllDirectories).ToList();
                    files2 = Directory.GetFiles(@"C:\Program Files (x86)\Microsoft Office", "WINWORD.EXE", SearchOption.AllDirectories).ToList();
                }
                catch (System.Exception) { }

                if (files.Count() > 0)
                {
                    wordPath.WordPath = files.FirstOrDefault();
                    _context.Defaults.Update(wordPath);
                    await _context.SaveChangesAsync();

                }
                else if (files2.Count() > 0)
                {
                    wordPath.WordPath = files2.FirstOrDefault();
                    _context.Defaults.Update(wordPath);
                    await _context.SaveChangesAsync();
                }
            }
            
        }
    }
}
