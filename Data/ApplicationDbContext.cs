using CricketGroundBookingApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace CricketGroundBookingApi.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Ground> Grounds => Set<Ground>();
    public DbSet<Slot> Slots => Set<Slot>();
    public DbSet<Booking> Bookings => Set<Booking>();
    public DbSet<Addon> Addons => Set<Addon>();
    public DbSet<BookingAddon> BookingAddons => Set<BookingAddon>();
    public DbSet<Package> Packages => Set<Package>();
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<Feedback> Feedbacks { get; set; }


    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // USER
        builder.Entity<User>().HasIndex(u => u.Email).IsUnique();
        builder.Entity<User>().HasIndex(u => u.Phone).IsUnique();

        // BOOKINGS
        builder.Entity<Booking>().HasIndex(b => new { b.GroundId, b.BookingDate, b.SlotType }).IsUnique();

        // USER -> BOOKINGS
        builder.Entity<Booking>().HasOne(b => b.User).WithMany(u => u.Bookings).HasForeignKey(b => b.UserId).OnDelete(DeleteBehavior.Restrict);

        // GROUND -> BOOKINGS
        builder.Entity<Booking>().HasOne(b => b.Ground).WithMany(g => g.Bookings).HasForeignKey(b => b.GroundId).OnDelete(DeleteBehavior.Restrict);

        // SLOT -> BOOKINGS
        builder.Entity<Booking>().HasOne(b => b.Slot).WithMany(s => s.Bookings).HasForeignKey(b => b.SlotId).OnDelete(DeleteBehavior.Restrict);

        // GROUND -> SLOTS
        builder.Entity<Slot>().HasOne(s => s.Ground).WithMany(g => g.Slots).HasForeignKey(s => s.GroundId).OnDelete(DeleteBehavior.Cascade);

        // BOOKING -> BOOKING ADDONS
        builder.Entity<BookingAddon>().HasOne(ba => ba.Booking).WithMany(b => b.BookingAddons).HasForeignKey(ba => ba.BookingId).OnDelete(DeleteBehavior.Cascade);

        // ADDON -> BOOKING ADDONS
        builder.Entity<BookingAddon>().HasOne(ba => ba.Addon).WithMany(a => a.BookingAddons).HasForeignKey(ba => ba.AddonId).OnDelete(DeleteBehavior.Restrict);

        // BOOKING -> PAYMENT (One-to-One)
        builder.Entity<Payment>().HasOne(p => p.Booking).WithOne(b => b.Payment).HasForeignKey<Payment>(p => p.BookingId).OnDelete(DeleteBehavior.Cascade);

        // DECIMAL PRECISION
        builder.Entity<Slot>().Property(s => s.BasePrice).HasColumnType("decimal(18,2)");

        builder.Entity<Addon>().Property(a => a.Price).HasColumnType("decimal(18,2)");

        builder.Entity<Package>().Property(p => p.Price).HasColumnType("decimal(18,2)");

        builder.Entity<Booking>().Property(b => b.BaseAmount).HasColumnType("decimal(18,2)");

        builder.Entity<Booking>().Property(b => b.AddonAmount).HasColumnType("decimal(18,2)");

        builder.Entity<Booking>().Property(b => b.TotalAmount).HasColumnType("decimal(18,2)");

        builder.Entity<BookingAddon>().Property(ba => ba.UnitPrice).HasColumnType("decimal(18,2)");

        builder.Entity<BookingAddon>().Property(ba => ba.TotalPrice).HasColumnType("decimal(18,2)");

        builder.Entity<Payment>().Property(p => p.Amount).HasColumnType("decimal(18,2)");

        builder.Entity<Ground>().Property(g => g.Latitude).HasColumnType("decimal(9,6)");

        builder.Entity<Ground>().Property(g => g.Longitude).HasColumnType("decimal(9,6)");
    }
}
