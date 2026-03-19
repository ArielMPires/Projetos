using Domus.Models;
using Domus.Models.DB;
using Domus.Repositories.Interfaces;
using Domus.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Domus.Controllers {
    [Route("Domus/[controller]")]
    [ApiController]
    public class CheckListController : ControllerBase {

        public readonly ICheckList _checklist;
        private readonly IHubContext<NotificationHub> _hubContext;
        public CheckListController(ICheckList checklist, IHubContext<NotificationHub> hubContext) 
        {
            _checklist = checklist;
            _hubContext = hubContext;
        }

        #region CheckList

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<Checklist>> ById(int id) => await _checklist.CheckList_SearchByPc(id);
        [Authorize]
        [HttpGet("List")]
        public async Task<ActionResult<List<Checklist>>> List() => await _checklist.List();
        [Authorize]
        [HttpPut("Update")]
        public async Task<ActionResult<Return>> Update([FromBody]Checklist update)
        {
            var result = await _checklist.UpdateCheckList(update);
            await _hubContext.Clients.All.SendAsync("UpdateCheck");
            return result;
        }
        [Authorize]
        [HttpPost("New")]
        public async Task<ActionResult<Return>> New([FromBody]Checklist newChecklist)
        {
            var result = await _checklist.NewCheckList(newChecklist);
            await _hubContext.Clients.All.SendAsync("UpdateCheck");
            return result;
        }
        [Authorize]
        [HttpDelete("Delete/{id}")]
        public async Task<ActionResult<Return>> Delete(int id) => await _checklist.DeleteCheckList(id);

        #endregion
    }
}
