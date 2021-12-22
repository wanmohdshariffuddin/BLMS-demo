using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using BLMS.Models.SOP;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BLMS.Context
{
    public partial class CompetentDBContext: DbContext
    {
        public CompetentDBContext(DbContextOptions<CompetentDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<CompetentPersonnel> CompetentPersonnel { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                //optionsBuilder.UseSqlServer("Server=10.249.1.125;Initial Catalog=BLMSDev;User Id=AppSa;Password=Opuswebsql2017;Encrypt=True;TrustServerCertificate=True");
                optionsBuilder.UseSqlServer("Data Source=10.49.45.40; Database=BLMS; User ID = Appsa; Password=Opuswebsql2018; Encrypt=False;TrustServerCertificate=False;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<CompetentPersonnel>(entity =>
            {
                entity.Property(e => e.PersonnelId).ValueGeneratedOnAdd();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
