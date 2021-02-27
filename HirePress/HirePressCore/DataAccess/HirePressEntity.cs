using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace HirePressCore.DataAccess
{
    public partial class HirePressEntity : DbContext
    {
        public HirePressEntity()
            : base("name=HirePressEntity")
        {
        }

        public virtual DbSet<MasterFlags> MasterFlags { get; set; }
        public virtual DbSet<MasterSkills> MasterSkills { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MasterFlags>()
                .Property(e => e.FlagName)
                .IsUnicode(false);

            modelBuilder.Entity<AspNetRoles>()
                .HasMany(e => e.AspNetUsers)
                .WithMany(e => e.AspNetRoles)
                .Map(m => m.ToTable("AspNetUserRoles").MapLeftKey("RoleId").MapRightKey("UserId"));

            modelBuilder.Entity<AspNetUser>()
                .HasMany(e => e.AspNetUserClaims)
                .WithRequired(e => e.AspNetUser)
                .HasForeignKey(e => e.UserId);

        }
    }
}
