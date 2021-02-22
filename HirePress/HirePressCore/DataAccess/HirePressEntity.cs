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

        public virtual DbSet<Master_Flag> Master_Flag { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Master_Flag>()
                .Property(e => e.FlagName)
                .IsUnicode(false);
        }
    }
}
