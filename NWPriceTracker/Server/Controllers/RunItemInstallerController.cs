using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NWPriceTracker.Server.Logic;

namespace NWPriceTracker.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RunItemInstallerController : ControllerBase
    {
        private readonly ILogger<RunItemInstallerController> _logger;

        public RunItemInstallerController(ILogger<RunItemInstallerController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<string> Get()
        {
            await InstallationHelper.InstallUpdateItemsAsync();
            return "success";
        }
    }
}
