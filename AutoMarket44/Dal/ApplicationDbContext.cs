using AutoMarket44.Domain.Entity;
using AutoMarket44.Domain.Enum;
using AutoMarket44.Domain.Helpers;
using Microsoft.EntityFrameworkCore;

namespace AutoMarket44.Dal
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }
        public DbSet<Car> Cars { get; set; }

        public DbSet<Profile> Profiles { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Basket> Baskets { get; set; }

        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(builder =>
            {
                builder.ToTable("Users").HasKey(x => x.Id);
                builder.HasData(new User[]
                    {
                        new User()
                        {
                             Id = 1,
                             Name = "Admin",
                             Password = HashPasswordHelpers.HashPassword("12345"),
                             Role = Role.Admin
                        },

                        new User()
                        {
                             Id = 2,
                             Name = "Moderator",
                             Password = HashPasswordHelpers.HashPassword("54321"),
                             Role = Role.Moderator
                        },
                    });

                builder.Property(x => x.Id).ValueGeneratedOnAdd();
                builder.Property(x => x.Password).IsRequired();
                builder.Property(x => x.Name).HasMaxLength(100).IsRequired();

                builder.HasOne(x => x.Profile)
                       .WithOne(p => p.User)
                       .HasPrincipalKey<User>(x => x.Id)
                       .OnDelete(DeleteBehavior.Cascade);
                builder.HasOne(x => x.Basket)
                       .WithOne(b => b.User)
                       .HasPrincipalKey<User>(x => x.Id)
                       .OnDelete(DeleteBehavior.Cascade);
            });
            modelBuilder.Entity<Car>(builder =>
            {
                builder.ToTable("Cars").HasKey(x => x.Id);

                builder.HasData(new Car
                {
                    Id = 1,
                    Name = "Sechka",
                    Description = new string('A', 50),
                    DateCreate = DateTime.Now.ToUniversalTime(),
                    Speed = 230,
                    Model = "Mers",
                    Avatar = null,
                    TypeCar = TypeCar.PassengerCar

                });
            });
            modelBuilder.Entity<Profile>(builder =>
            {
                builder.ToTable("Profiles").HasKey(x => x.Id);
                builder.Property(x => x.Id).ValueGeneratedOnAdd();
                builder.Property(x => x.Age);
                builder.Property(x => x.Address).HasMaxLength(200).IsRequired(false);

                builder.HasData(new Profile
                {
                    Id = 1,
                    UserId = 1
                });
            });
            modelBuilder.Entity<Basket>(builder =>
            {

                builder.ToTable("Baskets").HasKey(x => x.Id);
                builder.HasData(new Basket
                {
                    Id = 1,
                    UserId = 1
                });
            });
            modelBuilder.Entity<Order>(builder =>
            {
                builder.ToTable("Orders").HasKey(x => x.Id);
                builder.HasOne(x => x.Basket)
                       .WithMany(b => b.Orders)
                       .HasForeignKey(r => r.BasketId);

            });
        }
    }
}
