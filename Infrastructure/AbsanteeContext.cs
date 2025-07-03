using Infrastructure.DataModel;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class AbsanteeContext : DbContext
    {
        public virtual DbSet<CollaboratorDataModel> Collaborators { get; set; }
        public DbSet<UserDataModel> ValidUserIds { get; set; }
        public virtual DbSet<CollaboratorTempDataModel> CollaboratorsTemp { get; set; }

        public AbsanteeContext(DbContextOptions<AbsanteeContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CollaboratorDataModel>()
                .OwnsOne(a => a.PeriodDateTime);

            modelBuilder.Entity<CollaboratorTempDataModel>().OwnsOne(a => a.PeriodDateTime);

            modelBuilder.Entity<UserDataModel>().HasKey(v => v.Id);

            base.OnModelCreating(modelBuilder);
        }
    }
}