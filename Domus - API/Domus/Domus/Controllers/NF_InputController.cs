using Domus.DTO.NF;
using Domus.Models;
using Domus.Models.DB;
using Domus.Repositories.Interfaces;
using Domus.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Domus.Controllers
{
    [Route("Domus/[controller]")]
    [ApiController]
    public class NF_InputController : ControllerBase
    {

        public readonly INF_Input _nf_input;
        private readonly IHubContext<NotificationHub> _hubContext;

        public NF_InputController(INF_Input nf_input, IHubContext<NotificationHub> hubContext)
        {
            _nf_input = nf_input;
            _hubContext = hubContext;
        }

        #region NF_Input
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<NF_Input>> ById(int id) => await _nf_input.Input_SearchByNf(id);
        [Authorize]
        [HttpGet("List")]
        public async Task<ActionResult<IEnumerable<InputDTO>>> List() => Ok(await _nf_input.InputList());
        [Authorize]
        [HttpPost("New")]
        public async Task<ActionResult<Return>> New([FromBody] NF_Input nf_Input)
        {
            var result = await _nf_input.InputNew(nf_Input);
            await _hubContext.Clients.All.SendAsync("UpdateNFI");
            return result;
        }
        #endregion

        #region NF_Output
        [Authorize]
        [HttpGet("Output/{id}")]
        public async Task<ActionResult<NF_Output>> byId(int id) => await _nf_input.OutputSearchByNf(id);
        [Authorize]
        [HttpGet("Output/List")]
        public async Task<ActionResult<IEnumerable<OutputDTO>>> Lists() => Ok(await _nf_input.OutputList());
        [Authorize]
        [HttpPost("Output/New")]
        public async Task<ActionResult<Return>> New([FromBody] NF_Output nf_output)
        {
            var result = await _nf_input.OutputNew(nf_output);
            await _hubContext.Clients.All.SendAsync("UpdateNFO");
            return result;
        }

        #endregion
    }
}
