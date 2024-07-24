using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderingSystem.Application.Services;

namespace OrderingSystem.WebApi.Controllers
{
    [Route("api/file")]
    [ApiController]
    public class FileController(IFileService fileService): ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(200)]
        [Route("{id}")]
        public async Task<IActionResult> GetFile(Guid id, string? fileName)
        {
            var file = await fileService.GetPublicFileData(id);
            return File(file.Data, file.ContentType, fileDownloadName: fileName ?? file.FullName );
        }
    }
}
