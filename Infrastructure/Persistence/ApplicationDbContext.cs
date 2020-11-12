using Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Score> Scores { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<PromoCode> PromoCodes { get; set; }
        public DbSet<ScoreBoard> ScoreBoards { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Game>()
            .HasMany(c => c.ScoreBoards)
            .WithOne(e => e.Game);

            builder.Entity<Score>()
            .HasOne(c => c.ScoreBoard)
            .WithMany(x => x.Scores)
            .HasForeignKey(x => x.ScoreBoardId)
            .IsRequired();

            builder.Entity<Score>()
               .HasIndex(x => x.CreatedAt);

            builder.Entity<Score>()
              .HasIndex(x => x.ScoreBoardIdentifier);

            builder.Entity<Score>()
              .HasIndex(x => x.PlayerName);

            builder.Entity<Score>()
             .HasIndex(x => x.ScoreAmount);

            builder.Entity<Score>()
             .HasIndex(x => x.TimeAmount);

            builder.Entity<Score>()
             .HasIndex(x => x.Description);
        }

    }
}
