using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MiddlewareUnitTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContextDetailsController : ControllerBase
    {
        [HttpGet]
        public Dictionary<string, string> Get()
        {
            return this.HttpContext.Items
                .ToDictionary(x => x.Key.ToString(), x => x.Value.ToString());
        }
    }
}
