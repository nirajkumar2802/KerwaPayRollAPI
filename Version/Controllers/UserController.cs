using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Version.EntityModels;
using Version.InfraStructure;

namespace Version.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly InfraStructure.IUserRepo _userRepo;
        private readonly ILogger<EmployeeController> _logger;

        public UserController(IUserRepo userRepo, ILogger<EmployeeController> logger)
        {
            _userRepo = userRepo;
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
                var user = await _userRepo.Get();
                if (user == null)
                {
                    return NotFound();
                }
                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred");
            }
        }

    }
}
