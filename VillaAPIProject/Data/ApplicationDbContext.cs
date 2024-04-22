using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VillaAPIProject.Model;

namespace VillaAPIProject.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :base(options)
        {
            
        }
        //--------------USING APPLICATIONUSER UNDER DBCONTEXT
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        //-------------------------------------------------------------------------------------------------
        public DbSet<LocalUser> LocalUsers { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Villa> Villas { get; set; }
        public DbSet<VillaNumber> VillaNumbers { get; set; }
        //-------------------------------------------------------------------------------------------------
        //seed data
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Villa>().HasData(
                new Villa()
                {
                    Id=1,
                    Name="Beach view",
                    Details="Beautiful view for couples",
                    ImageUrl="",
                    Occupancy=1,
                    Rate=100,
                    Sqft=100,
                    Amenity="",
                    CreatedDate=DateTime.Now,
                },
                new Villa()
                {
                    Id = 2,
                    Name = "Lakeshore view",
                    Details = "Beautiful view for solo travel",
                    ImageUrl = "",
                    Occupancy = 1,
                    Rate = 50,
                    Sqft = 100,
                    Amenity = "",
                    CreatedDate = DateTime.Now,
                }
                );
            modelBuilder.Entity<VillaNumber>().HasData(
                new VillaNumber()
                {
                    VillaNo = 101,
                    SpecialDetails = "Guests love roses",
                    CreatedDate = DateTime.Now,
                },
                new VillaNumber()
                {
                    VillaNo=102,
                    SpecialDetails="Guests love daisy",
                    CreatedDate = DateTime.Now,
                }
                );
        }
        

    }
}
