using BookingService.Models;
using System.Linq.Expressions;

namespace BookingService.Data.Repositories
{
    public interface IBookingRepository : IBaseRepository<BookingEntity>
    {
    }
}