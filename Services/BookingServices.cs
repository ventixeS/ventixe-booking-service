using BookingService.Data.Repositories;
using BookingService.Models;
using Presentation.Models;

namespace BookingService.Services
{
    public class BookingServices(IBookingRepository bookingRepository) : IBookingServices
    {
        private readonly IBookingRepository _bookingRepository = bookingRepository;

        public async Task<BookingResult> CreateBookingAsync(CreateBookingRequest request)
        {
            var bookingEntity = new BookingEntity
            {
                EventId = request.EventId,
                UserId = request.UserId,
                BookingDate = DateTime.Now,
                TicketQuantity = request.TicketQuantity,
                BookingOwner = new BookingOwnerEntity
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Email = request.Email,
                    Address = new BookingAddressEntity
                    {
                        StreetName = request.StreetName,
                        PostalCode = request.PostalCode,
                        City = request.City,
                    }
                }
            };

            var result = await _bookingRepository.AddAsync(bookingEntity);
            return result.Success
                ? new BookingResult { Success = true, BookingId = bookingEntity.Id }
                : new BookingResult { Success = false, Error = result.Error };
        }

        public async Task<RepositoryResult<IEnumerable<BookingEntity>>> GetUserBookingsAsync(string userId)
        {
            var allBookingsResult = await _bookingRepository.GetAllAsync();
            if (!allBookingsResult.Success)
            {
                return new RepositoryResult<IEnumerable<BookingEntity>>
                {
                    Success = false,
                    Error = allBookingsResult.Error
                };
            }

            var userBookings = allBookingsResult.Result?.Where(b => b.UserId == userId) ?? Enumerable.Empty<BookingEntity>();
            return new RepositoryResult<IEnumerable<BookingEntity>>
            {
                Success = true,
                Result = userBookings
            };
        }

        public async Task<RepositoryResult<BookingEntity?>> GetBookingByIdAsync(string id)
        {
            return await _bookingRepository.GetAsync(b => b.Id == id);
        }

        public async Task<RepositoryResult> UpdateBookingAsync(string id, BookingEntity booking)
        {
            var existingBookingResult = await _bookingRepository.GetAsync(b => b.Id == id);
            if (!existingBookingResult.Success || existingBookingResult.Result == null)
            {
                return new RepositoryResult { Success = false, Error = "Booking not found" };
            }

            return await _bookingRepository.UpdateAsync(booking);
        }

        public async Task<RepositoryResult> CancelBookingAsync(string id)
        {
            var bookingResult = await _bookingRepository.GetAsync(b => b.Id == id);
            if (!bookingResult.Success || bookingResult.Result == null)
            {
                return new RepositoryResult { Success = false, Error = "Booking not found" };
            }

            return await _bookingRepository.DeleteAsync(bookingResult.Result);
        }
    }
}