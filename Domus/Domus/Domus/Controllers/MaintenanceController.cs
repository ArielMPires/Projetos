using Domus.Models.DB;
using Domus.Models;
using Domus.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Domus.Controllers {

    [Route("Domus/[controller]")]
    [ApiController]

    public class MaintenanceController : ControllerBase {

        public readonly IMaintenance _maintenance;
        public MaintenanceController(IMaintenance maintenance) => _maintenance = maintenance;

        #region Maintenance
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<Maintenance>> Byid(int id) => await _maintenance.Maintenance_SearchByPc(id);
        [Authorize]
        [HttpGet("List")]
        public async Task<ActionResult<List<Maintenance>>> List() => await _maintenance.MaintenanceList();
        [Authorize]
        [HttpPut("Update")]
        public async Task<ActionResult<Return>> Update([FromBody] Maintenance update) => await _maintenance.UpdateMaintenance(update);
        [Authorize]
        [HttpPost("New")]
        public async Task<ActionResult<Return>> New([FromBody] Maintenance newMaintenance) => await _maintenance.NewMaintenance(newMaintenance);
        [Authorize]
        [HttpDelete("Delete/{id}")]
        public async Task<ActionResult<Return>> Delete(int id) => await _maintenance.DeleteMaintenance(id);
        #endregion

        #region CheckList
        [Authorize]
        [HttpGet("CheckList/{id}")]
        public async Task<ActionResult<Maintenance_CheckList>> ById(int id) => await _maintenance.CheckList_SearchByPc(id);
        [Authorize]
        [HttpGet("CheckList/List")]
        public async Task<ActionResult<List<Maintenance_CheckList>>> Lists() => await _maintenance.Checklist_List();
        [Authorize]
        [HttpPut("CheckList/Update")]
        public async Task<ActionResult<Return>> Updates([FromBody] Maintenance_CheckList update) => await _maintenance.Update_CheckList(update);
        [Authorize]
        [HttpPost("CheckList/New")]
        public async Task<ActionResult<Return>> NewCheck([FromBody] Maintenance_CheckList newCheck) => await _maintenance.New_CheckList(newCheck);
        [Authorize]
        [HttpDelete("CheckList/Delete/{id}")]
        public async Task<ActionResult<Return>> DeleteCheck(int id) => await _maintenance.Delete_CheckList(id);
        #endregion

        #region Scheduled
        [Authorize]
        [HttpGet("Scheduled/{id}")]
        public async Task<ActionResult<Scheduled_Maintenance>> byid(int id) => await _maintenance.Scheduled_SearchByPc(id);
        [Authorize]
        [HttpGet("Scheduled/List")]
        public async Task<ActionResult<List<Scheduled_Maintenance>>> list() => await _maintenance.ScheduledList();
        [Authorize]
        [HttpPut("Scheduled/Update")]
        public async Task<ActionResult<Return>> update([FromBody] Scheduled_Maintenance update) => await _maintenance.UpdateScheduled(update);
        [Authorize]
        [HttpPost("Scheduled/New")]
        public async Task<ActionResult<Return>> NewScheduled([FromBody] Scheduled_Maintenance newScheduled) => await _maintenance.NewScheduled(newScheduled);
        [Authorize]
        [HttpDelete("Scheduled/Delete/{id}")]
        public async Task<ActionResult<Return>> DeleteScheduled(int id) => await _maintenance.DeleteScheduled(id);
        #endregion
    }
}
