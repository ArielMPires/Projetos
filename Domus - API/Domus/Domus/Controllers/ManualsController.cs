using Domus.Models.DB;
using Domus.Models;
using Domus.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Domus.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class ManualsController : ControllerBase {

        #region Property
        public readonly IManuals _manual;
        public ManualsController(IManuals manuals) => _manual = manuals;

        #endregion

        #region CheckList
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<Manuals>> ById(int id) => await _manual.Manuals_SearchBy(id);
        [Authorize]
        [HttpGet("List")]
        public async Task<ActionResult<List<Manuals>>> List() => await _manual.ListManuals();
        [Authorize]
        [HttpPut("Update")]
        public async Task<ActionResult<Return>> Update([FromBody] Manuals update) => await _manual.UpdateManuals(update);
        [Authorize]
        [HttpPost("New")]
        public async Task<ActionResult<Return>> New([FromBody] Manuals newChecklist) => await _manual.NewManuals(newChecklist);
        [Authorize]
        [HttpDelete("Delete/{id}")]
        public async Task<ActionResult<Return>> Delete(int id) => await _manual.DeleteManuals(id);

        #endregion
    }
}
