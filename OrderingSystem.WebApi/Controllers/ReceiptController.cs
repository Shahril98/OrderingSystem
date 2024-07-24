using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OrderingSystem.Application.Services;
using OrderingSystem.Infrastructure;
using OrderingSystem.Infrastructure.Dtos;

namespace OrderingSystem.WebApi.Controllers
{
    [Route("api/receipt")]
    [ApiController]
    [Authorize(Roles = UserRole.Admin)]
    public class ReceiptController(ReceiptService receiptService, UserManager<IdentityUser<Guid>> userManager) : ControllerBase
    {
        [HttpGet]
        [Route("{receiptId}")]
        [ProducesResponseType(typeof(ReceiptDto), 200)]
        public async Task<IActionResult> GetReceipt(Guid receiptId)
        {
            var result = await receiptService.GetReceipt(receiptId);
            return Ok(result);
        }
        [HttpPost]
        [Route("add")]
        [ProducesResponseType(typeof(Guid), 200)]
        public async Task<IActionResult> AddReceipt(AddReceiptDto addReceipt)
        {
            Guid userId = new Guid(userManager.GetUserId(User));
            var result = await receiptService.AddReceipt(addReceipt, userId);
            return Ok(result);
        }
        [HttpGet]
        [Route("{receiptId}/fulldetails")]
        [ProducesResponseType(typeof(ReceiptDto), 200)]
        public async Task<IActionResult> GetReceiptFullDetails(Guid receiptId)
        {
            var result = await receiptService.GetReceiptFullDetails(receiptId);
            return Ok(result);
        }
    }
}
