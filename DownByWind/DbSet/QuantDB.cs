using DownByWind.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownByWind.DbSet
{
    public class QuantDBContext : DbContext
    {
        /// <summary>
        /// K线
        /// </summary>
        public DbSet<Bar> Bars { get; set; }

        /// <summary>
        /// 每秒的tick
        /// </summary>

        public DbSet<Tick> Ticks { get; set; }

        /// <summary>
        /// 代码的信息
        /// </summary>
        public DbSet<CodeInfo> CodeInfos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL("server=192.168.1.60;userid=root;pwd=;port=3306;database=QuantDB;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Bar>()
               .HasNoKey();

            modelBuilder.Entity<Tick>()
               .HasNoKey();


            modelBuilder.Entity<CodeInfo>()
               .HasKey("WindCode");

        }
    }
}
