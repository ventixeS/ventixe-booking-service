using BookingService.Models;
using Presentation.Models;

namespace BookingService.Services
{
    public interface IBookingServices
    {
        Task<BookingResult> CreateBookingAsync(CreateBookingRequest request);
        Task<RepositoryResult<IEnumerable<BookingEntity>>> GetUserBookingsAsync(string userId);
        Task<RepositoryResult<BookingEntity?>> GetBookingByIdAsync(string id);
        Task<RepositoryResult> UpdateBookingAsync(string id, BookingEntity booking);
        Task<RepositoryResult> CancelBookingAsync(string id);
    }
}