using CRUD_asp.netMVC.Data;
using CRUD_asp.netMVC.Models.Order;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRUD_asp.netMVC.Controllers
{
    public class WebhookController : Controller
    {
        private readonly AppDBContext _dbContext;

        public WebhookController(AppDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost("ghn")]
        public async Task<IActionResult> ReceiveGhnWebhook([FromBody] GhnWebhookPayload payload)
        {
            var order = await _dbContext.Orders.FirstOrDefaultAsync(o => o.TrackingNumber == payload.OrderCode);
            if (order == null) return NotFound("Order not found");

            order.Status = payload.Status switch
            {
                "ready_to_pick" => OrderStatus.InTransit.ToString(),
                "delivering" => OrderStatus.OutForDelivery.ToString(),
                "delivered" => OrderStatus.Delivered.ToString(),
                "cancel" => OrderStatus.Failed.ToString(),
                _ => order.Status
            };
            order.StatusTime = DateTime.UtcNow;
            await _dbContext.SaveChangesAsync();

            return Ok();
        }
    }
    public class GhnWebhookPayload
    {
        public string OrderCode { get; set; }
        public string Status { get; set; }
        public string Reason { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
