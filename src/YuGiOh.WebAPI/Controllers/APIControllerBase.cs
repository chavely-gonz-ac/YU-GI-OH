using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace YuGiOh.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public abstract class APIControllerBase : ControllerBase
    {
        private readonly ISender _sender;
        public ISender Sender => _sender;
        public APIControllerBase(ISender sender) => _sender = sender;
    }
}