using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Version.EntityModels;
using Version.InfraStructure;

namespace Version.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    
    public class EmployeeController : ControllerBase
    {
        private readonly InfraStructure.IKerwaEmployeeRepo _kerwaEmployeeRepo;
        private readonly ILogger<EmployeeController> _logger;

        public EmployeeController(IKerwaEmployeeRepo kerwaEmployeeRepo, ILogger<EmployeeController> logger)
        {
            _kerwaEmployeeRepo = kerwaEmployeeRepo;
            _logger = logger;   
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<KerwaEmployee>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Get()
        {
            try
            {
                var employees = await _kerwaEmployeeRepo.GetAll();
                if(employees == null || employees.Count == 0)
                {
                    return NotFound();  
                }
                return Ok(employees);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Save(KerwaEmployee kerwaEmployee)
        {
            try
            {
                var res = await _kerwaEmployeeRepo.Save(kerwaEmployee);
                if (res <= 0)
                {
                    return BadRequest();
                }
                return Ok(res);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred");
            }
        }
    }
}
