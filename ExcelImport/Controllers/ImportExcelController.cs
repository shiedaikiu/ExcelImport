using ExcelImport.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExcelImport.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImportExcelController : ControllerBase
    {
        private readonly ExcelImportService _service;
        public ImportExcelController(ExcelImportService service)
        {
            _service = service;
        }
        [HttpPost("import")]
        public async Task<IActionResult> Import(IFormFile file)
        {
            try
            {
                using var stream = file.OpenReadStream();
                await _service.ImportAsync(stream);
                return Ok("فایل با موفقیت ثبت شد");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
