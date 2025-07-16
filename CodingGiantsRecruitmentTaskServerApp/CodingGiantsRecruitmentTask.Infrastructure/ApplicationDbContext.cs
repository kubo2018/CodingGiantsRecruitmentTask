using CodingGiantsRecruitmentTask.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CodingGiantsRecruitmentTask.Infrastructure
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        { 
        }

        public DbSet<ChatMessage> ChatMessages => Set<ChatMessage>();
        public DbSet<ChatMessageRating> ChatMessageRatings => Set<ChatMessageRating>();
        public DbSet<ChatMessageRatingType> ChatMessageRatingTypes => Set<ChatMessageRatingType>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ChatMessage>(entity =>
            {
                entity.ToTable("ChatMessages");

                entity.HasKey(x => x.Id);

                entity.Property(x => x.Text)
                    .IsRequired()
                    .HasMaxLength(4000);

                entity.Property(x => x.IsFromBot)
                    .IsRequired();

                entity.Property(x => x.CreationDate)
                    .IsRequired();

                entity.HasOne(x => x.ChatMessageRating)
                    .WithOne(r => r.ChatMessage)
                    .HasForeignKey<ChatMessageRating>(r => r.ChatMessageId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<ChatMessageRating>(entity =>
            {
                entity.ToTable("ChatMessageRatings");

                entity.HasKey(x => x.Id);

                entity.Property(x => x.CreationDate)
                    .IsRequired();

                entity.HasOne(x => x.ChatMessageRatingType)
                    .WithMany(t => t.ChatMessageRatings)
                    .HasForeignKey(r => r.ChatMessageRatingTypeId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<ChatMessageRatingType>(entity =>
            {
                entity.ToTable("ChatMessageRatingTypes");

                entity.HasKey(x => x.Id);

                entity.Property(x => x.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(x => x.Icon)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasData(
                    new ChatMessageRatingType { Id = 1, Name = "Dobra odpowiedź", Icon = "thumb_up" },
                    new ChatMessageRatingType { Id = 2, Name = "Zła odpowiedź", Icon = "thumb_down" }
                );
            });
        }
    }
}
