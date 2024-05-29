using Asp.Versioning;
using FoundationaLLM.Common.Models.Configuration.Branding;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace FoundationaLLM.Core.API.Controllers
{
    /// <summary>
    /// Provides branding information for the client.
    /// </summary>
    [Authorize(Policy = "DefaultPolicy")]
    [ApiController]
    [Route("[controller]")]
    public class AttachmentsController(ILogger<AttachmentsController> logger) : ControllerBase
    {
        /// <summary>
        /// Retrieves the uploaded attachments.
        /// </summary>
        [HttpGet(Name = "Get")]
        public IActionResult Index() =>
            Ok();

        /// <summary>
        /// Uploads an attachment.
        /// </summary>
        /// <param name="file">The file sent with the HTTP request.</param>
        /// <returns></returns>
        [HttpPost("Upload")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File not selected.");

            var fileName = file.FileName;
            var contentType = file.ContentType;

            //using (var stream = file.OpenReadStream())
            //{
            //}
            return Ok();
        }
    }
}
