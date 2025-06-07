using BookingService.Services;
using BookingService.Models;
using Microsoft.AspNetCore.Mvc;
using Presentation.Models;


namespace Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BookingsController(IBookingServices bookingsService) : ControllerBase
{
    private readonly IBookingServices _bookingsService = bookingsService;

    [HttpPost]
    public async Task<IActionResult> Create(CreateBookingRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _bookingsService.CreateBookingAsync(request);
        return result.Success
            ? Ok(new { message = "Booking created successfully", bookingId = result.BookingId })
            : StatusCode(StatusCodes.Status500InternalServerError, "Unable to create booking.");
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetUserBookings(string userId)
    {
        if (string.IsNullOrEmpty(userId))
            return BadRequest("User ID is required");

        var result = await _bookingsService.GetUserBookingsAsync(userId);
        return result.Success ? Ok(result.Result) : StatusCode(500, result.Error);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetBooking(string id)
    {
        var result = await _bookingsService.GetBookingByIdAsync(id);
        return result.Success ? Ok(result.Result) : NotFound();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBooking(string id, BookingEntity booking)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _bookingsService.UpdateBookingAsync(id, booking);
        return result.Success ? Ok(new { message = "Booking updated successfully" }) : BadRequest(result.Error);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> CancelBooking(string id)
    {
        var result = await _bookingsService.CancelBookingAsync(id);
        return result.Success ? Ok(new { message = "Booking cancelled successfully" }) : BadRequest(result.Error);
    }
}
