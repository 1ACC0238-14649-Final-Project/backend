using gigu_back_end.User.Domain.Models.Entities;
using Gigs.Domain.Models.Entities;
using Chats.Domain.Models.Entities;
using gigu_back_end.Briefcases.Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace gigu_back_end.Shared.Infrastructure.Persistence.Configuration
{
    public class GigUContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<User.Domain.Models.Entities.User> Users { get; set; }
        public DbSet<Gig> Gigs { get; set; }
        public DbSet<Pull> Pulls { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<Briefcase> Briefcases { get; set; }
        public DbSet<Project> Projects { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            base.OnConfiguring(builder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // User Configuration
            builder.Entity<User.Domain.Models.Entities.User>(entity =>
            {
                entity.ToTable("Users");
                entity.HasKey(u => u.Id);

                entity.Property(u => u.Name).IsRequired().HasMaxLength(50);
                entity.Property(u => u.Lastname).IsRequired().HasMaxLength(50);
                entity.Property(u => u.Email).IsRequired().HasMaxLength(100);
                entity.HasIndex(u => u.Email).IsUnique();
                entity.Property(u => u.Password).IsRequired().HasMaxLength(100);
                entity.Property(u => u.Role).IsRequired().HasMaxLength(20);
                entity.Property(u => u.Image).IsRequired().HasMaxLength(255);

                // Fecha y auditoría
                entity.Property(u => u.CreatedDate).HasColumnType("datetime").IsRequired();
                entity.Property(u => u.ModifiedDate).HasColumnType("datetime");
                entity.Property(u => u.UserId).IsRequired();
                entity.Property(u => u.UpdatedUserId);
                entity.Property(u => u.IsActive).IsRequired();
            });

            // Gig Configuration
            builder.Entity<Gig>(entity =>
            {
                entity.ToTable("Gigs");
                entity.HasKey(g => g.Id);

                entity.Property(g => g.Image)
                    .IsRequired()
                    .HasColumnType("LONGTEXT");

                entity.Property(g => g.Title).IsRequired().HasMaxLength(200);
                entity.Property(g => g.Description).IsRequired().HasMaxLength(2000);
                entity.Property(g => g.SellerId).IsRequired();

                entity.Property(g => g.Price).HasColumnType("decimal(18,2)");

                entity.Property(g => g.Tags)
                    .HasConversion(
                        v => string.Join(',', v),
                        v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList()
                    )
                    .HasColumnType("TEXT");

                entity.Property(g => g.Category).IsRequired().HasMaxLength(100);

                entity.Property(g => g.ExtraFeatures)
                    .HasConversion(
                        v => string.Join(',', v),
                        v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList()
                    )
                    .HasColumnType("TEXT");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("timestamp")
                    .ValueGeneratedOnAdd();

                entity.Property(g => g.IsResponsive).IsRequired();
                entity.Property(g => g.RevisionCount).IsRequired();
                entity.Property(g => g.PageCount).IsRequired();
                entity.Property(g => g.CustomAnimations).IsRequired();
                entity.Property(g => g.DeliveryDays).IsRequired();

                entity.HasIndex(g => g.SellerId);
                entity.HasIndex(g => g.Category);
                entity.HasIndex(g => new { g.IsResponsive, g.CustomAnimations });
            });

            // Pull Configuration
            builder.Entity<Pull>(entity =>
            {
                entity.ToTable("Pulls");
                entity.HasKey(p => p.Id);

                entity.Property(p => p.SellerId).IsRequired();
                entity.Property(p => p.BuyerId);
                entity.Property(p => p.GigId).IsRequired();
                entity.Property(p => p.PriceInit).IsRequired().HasColumnType("decimal(10,2)");
                entity.Property(p => p.PriceUpdate).HasColumnType("decimal(10,2)");
                entity.Property(p => p.State).IsRequired().HasMaxLength(20);
            });

            // Chat Configuration
            builder.Entity<Chat>(entity =>
            {
                entity.ToTable("Chats");
                entity.HasKey(c => c.Id);

                entity.Property(c => c.SenderId).IsRequired();
                entity.Property(c => c.ReceiverId).IsRequired();
                entity.Property(c => c.Content).IsRequired().HasMaxLength(1000);
                entity.Property(c => c.SentAt).IsRequired().HasColumnType("datetime");
                entity.Property(c => c.IsRead).IsRequired();
                entity.Property(c => c.CreatedDate).IsRequired().HasColumnType("datetime");
                entity.Property(c => c.ModifiedDate).HasColumnType("datetime");
                entity.Property(c => c.UserId).IsRequired();
                entity.Property(c => c.UpdatedUserId);
                entity.Property(c => c.IsActive).IsRequired();
            });

            // Briefcase Configuration
            builder.Entity<Briefcase>(entity =>
            {
                entity.ToTable("Briefcases");
                entity.HasKey(c => c.Id);

                entity.Property(c => c.Name).IsRequired().HasMaxLength(20);
                entity.Property(c => c.PublishDate).IsRequired().HasColumnType("datetime");
                entity.Property(c => c.Description).IsRequired().HasMaxLength(100);
                entity.Property(c => c.CreatedDate).IsRequired().HasColumnType("datetime");
                entity.Property(c => c.ModifiedDate).HasColumnType("datetime");

                entity.HasIndex(c => c.Name).IsUnique();
            });

            // Project Configuration
            builder.Entity<Project>(entity =>
            {
                entity.ToTable("Projects");
                entity.HasKey(c => c.Id);

                entity.Property(c => c.CreatedDate).IsRequired().HasColumnType("datetime");
                entity.Property(c => c.ModifiedDate).HasColumnType("datetime");
            });
        }
    }
}
