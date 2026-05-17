using CricketGroundBookingApi.Data;
using CricketGroundBookingApi.DTOs.Feedbacks;
using CricketGroundBookingApi.Entities;
using CricketGroundBookingApi.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CricketGroundBookingApi.Services;

public class FeedbackService : IFeedbackService
{
    private readonly ApplicationDbContext _context;

    public FeedbackService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<FeedbackResponse> CreateAsync(
        long userId,
        CreateFeedbackRequest request
    )
    {
        var booking = await _context.Bookings
            .Include(b => b.Ground)
            .FirstOrDefaultAsync(b =>
                b.Id == request.BookingId &&
                b.UserId == userId
            );

        if (booking == null)
        {
            throw new Exception("Booking not found");
        }

        if (booking.Status != "Confirmed")
        {
            throw new Exception(
                "Feedback allowed only for confirmed bookings"
            );
        }

        var alreadyReviewed = await _context.Feedbacks
            .AnyAsync(f => f.BookingId == booking.Id);

        if (alreadyReviewed)
        {
            throw new Exception(
                "Feedback already submitted for this booking"
            );
        }

        if (request.Rating < 1 || request.Rating > 5)
        {
            throw new Exception(
                "Rating must be between 1 and 5"
            );
        }

        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == userId);

        var feedback = new Feedback
        {
            UserId = userId,
            GroundId = booking.GroundId,
            BookingId = booking.Id,
            Rating = request.Rating,
            Comment = request.Comment,
            IsVisible = true
        };

        _context.Feedbacks.Add(feedback);

        await _context.SaveChangesAsync();

        return new FeedbackResponse
        {
            Id = feedback.Id,
            GroundId = booking.GroundId,
            GroundName = booking.Ground.Name,
            Rating = feedback.Rating,
            Comment = feedback.Comment,
            UserName = user?.FullName ?? "User",
            CreatedAt = feedback.CreatedAt
        };
    }

    public async Task<List<FeedbackResponse>> GetByGroundAsync(
        long groundId
    )
    {
        return await _context.Feedbacks
            .Where(f =>
                f.GroundId == groundId &&
                f.IsVisible
            )
            .Include(f => f.User)
            .Include(f => f.Ground)
            .OrderByDescending(f => f.CreatedAt)
            .Select(f => new FeedbackResponse
            {
                Id = f.Id,
                GroundId = f.GroundId,
                GroundName = f.Ground.Name,
                Rating = f.Rating,
                Comment = f.Comment,
                UserName = f.User.FullName,
                CreatedAt = f.CreatedAt
            })
            .ToListAsync();
    }
}