using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class BookingManagementDbContext : DbContext
{
    public BookingManagementDbContext(DbContextOptions<BookingManagementDbContext> options) : base(options) { }

    public DbSet<Account> Accounts { get; set; }
    public DbSet<AccountRole> AccountRoles { get; set; }
    public DbSet<Booking> Bookings { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Education> Educations { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<University> Universities { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Employee>().HasIndex(e => new {
            e.Nik,
            e.Email,
            e.PhoneNumber
        }).IsUnique();
        
        // Relation between Education and University (1 to Many)
        modelBuilder.Entity<Education>()
                    .HasOne(e => e.University)
                    .WithMany(u => u.Educations)
                    .HasForeignKey(e => e.UniversityGuid);

        // Relation between Employee and Education (1 to 1)
        modelBuilder.Entity<Employee>()
                    .HasOne(e => e.Education)
                    .WithOne(e => e.Employee)
                    .HasForeignKey<Education>(e => e.Guid);

        // Relation between Employee and Account (1 to 1)
        modelBuilder.Entity<Employee>()
                    .HasOne(e => e.Account)
                    .WithOne(e => e.Employee)
                    .HasForeignKey<Account>(e => e.Guid);

        // Relation between Account and AccountRole (1 to Many)
        modelBuilder.Entity<Account>()
                    .HasMany(a => a.AccountRoles)
                    .WithOne(a => a.Account)
                    .HasForeignKey(a => a.AccountGuid);

        // Relation between Role and AccountRole (1 to Many)
        modelBuilder.Entity<Role>()
                    .HasMany(r => r.AccountRoles)
                    .WithOne(r => r.Role)
                    .HasForeignKey(r => r.RoleGuid);

        // Relation between Employee and Booking (1 to Many)
        modelBuilder.Entity<Employee>()
                    .HasMany(e => e.Bookings)
                    .WithOne(e => e.Employee)
                    .HasForeignKey(e => e.EmployeeGuid);

        // Relation between Room and Booking (1 to Many)
        modelBuilder.Entity<Room>()
                    .HasMany(r => r.Bookings)
                    .WithOne(r => r.Room)
                    .HasForeignKey(r => r.RoomGuid);
    }
}
