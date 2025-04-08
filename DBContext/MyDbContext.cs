using FMSBay.Models.Entitys;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
namespace FMSBay.DBContext
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<UserEntity>().ToTable("Users");
            builder.Entity<UserTypeMasterEntity>().ToTable("UserTypeMaster");
            // builder.Entity<ContactFormEntity>().ToTable("ContactForm");
            builder.Entity<ContactFormEntity>().ToTable("ContactForm");
            builder.Entity<LoanTypeEntity>().ToTable("LoanType");
            builder.Entity<ContactFormEntity>()
           .HasOne(cf => cf.LoanType)
           .WithMany()
           .HasForeignKey(cf => cf.LoanTypeId);

        }
        public DbSet<UserEntity> UserEntity { get; set; }
        public DbSet<UserTypeMasterEntity> UserTypeMasterEntity { get; set; }
        public DbSet<LoanTypeEntity> LoanTypeEntity { get; set; }
        public DbSet<ContactFormEntity> ContactFormEntity { get; set; }
         
    }
}
