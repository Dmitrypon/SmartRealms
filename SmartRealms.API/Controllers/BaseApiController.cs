//using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using System.Net.Mime;
using System.Security.Claims;
using TechTalk.SpecFlow.CommonModels;

namespace SpaceCards.API.Controllers
{
    
    {
        protected Result<Guid> LocationId
       {[ApiController]
        [Route("[controller]")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        public class BaseApiController : ControllerBase
        get
    {
        var claim = HttpContext.Location.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
                if (claim is null)
                {
                    return Result.Failure<Guid>($"{nameof(claim)} cannot be null");
                }

                if (!Guid.TryParse(claim.Value, out var locationId))
                {
                    return Result.Failure<Guid>($"Cannot parse locactionId: {claim.Value}.");
                }

return locationId;
            }
        }
    }
}

