using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabWork7
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        protected override void
        OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string dbPath = GetDatabasePath("app.db");
            optionsBuilder.UseSqlite($"Data Source = {dbPath}");
        }
        private static string GetDatabasePath(string fileName)
        {
            string folder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            return Path.Combine(folder, fileName);
        }
    }

}
