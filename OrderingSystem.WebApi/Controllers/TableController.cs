using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderingSystem.Application.Services;
using OrderingSystem.Infrastructure;
using OrderingSystem.Infrastructure.Dtos;
using System.ComponentModel.DataAnnotations;

namespace OrderingSystem.WebApi.Controllers
{
    [Route("api/table")]
    [ApiController]
    public class TableController(ITableService tableService) : ControllerBase
    {
        [Authorize(Roles = UserRole.Admin)]
        [HttpPost]
        [ProducesResponseType(typeof(TableDto), 200)]
        [Route("add")]
        public async Task<IActionResult> AddTable([FromBody] AddTableDto addTable)
        {
            var result = await tableService.AddTable(addTable);
            return Ok(result);
        }
        [HttpGet]
        [ProducesResponseType(typeof(List<TableDto>), 200)]
        [Route("all")]
        public async Task<IActionResult> GetAllTables()
        {
            var result = await tableService.GetAllTables();
            return Ok(result);
        }
        [HttpGet]
        [ProducesResponseType(typeof(TableDto), 200)]
        [Route("{tableId}")]
        public async Task<IActionResult> GetTableById(Guid tableId)
        {
            var result = await tableService.GetTableById(tableId);
            return Ok(result);
        }
        [HttpPut]
        [Authorize(Roles = UserRole.Admin)]
        [ProducesResponseType(typeof(TableDto), 200)]
        [Route("{tableId}")]
        public async Task<IActionResult> UpdateTable(Guid tableId, [FromBody] UpdateTableDto updateTable)
        {
            var result = await tableService.UpdateTable(updateTable, tableId);
            return Ok(result);
        }
        [HttpDelete]
        [ProducesResponseType(204)]
        [Route("{tableId}")]
        public async Task<IActionResult> DeleteTable(Guid tableId)
        {
            await tableService.DeleteTable(tableId);
            return NoContent();
        }
        [HttpGet]
        [Authorize]
        [Route("{tableId}/qrcode")]
        [ProducesResponseType(200)]
        public IActionResult GetTableQrCode(Guid tableId, [Required]string link)
        {
            var file = tableService.CreateTableQrCode(tableId, link);
            return File(file.Data, file.ContentType, fileDownloadName:  file.FullName);
        }
    }
}
