using CricketGroundBookingApi.DTOs.Feedbacks;

namespace CricketGroundBookingApi.Interfaces;

public interface IFeedbackService
{
    Task<FeedbackResponse> CreateAsync(
        long userId,
        CreateFeedbackRequest request
    );

    Task<List<FeedbackResponse>> GetByGroundAsync(
        long groundId
    );
}