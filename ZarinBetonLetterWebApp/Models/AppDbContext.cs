using Microsoft.EntityFrameworkCore;

namespace ZarinBetonLetterWebApp.Models
{
    public class AppDbContext : DbContext
    {
        public DbSet<Default> Defaults { get; set; }
        public DbSet<ReceivedMail> ReceivedMails { get; set; }
        public DbSet<SentMail> SentMails { get; set; }
        public DbSet<Slogan> Slogans { get; set; }
        public DbSet<YearLastNo> YearLastNos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=ZarinBetonLetters.db");
        }
    }
}
