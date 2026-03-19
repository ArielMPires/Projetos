using Domus.DataBase;
using Domus.DTO.Computer;
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
    public class PatrimonyController : ControllerBase
    {

        public readonly IPatrimony _patrimony;
        private readonly IHubContext<NotificationHub> _hubContext;

        public PatrimonyController(IPatrimony patrimony, IHubContext<NotificationHub> hubContext)
        {
            _patrimony = patrimony;
            _hubContext = hubContext;
        }


        #region Patrimony
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<Patrimony>> ById(int id) => await _patrimony.Patrimony_SearchBy(id);
        [Authorize]
        [HttpGet("List")]
        public async Task<ActionResult<IEnumerable<Patrimony>>> List() => Ok(await _patrimony.PatrimonyList());
        [Authorize]
        [HttpGet("PCRegister")]
        public async Task<ActionResult<IEnumerable<Patrimony>>> RegisterPC() => Ok(await _patrimony.PCRegister());
        [Authorize]
        [HttpPut("Update")]
        public async Task<ActionResult<Return>> Update([FromBody] Patrimony update)
        {
            var result = await _patrimony.UpdatePatrimony(update);
            await _hubContext.Clients.All.SendAsync("UpdatePatrimony");
            return result;
        }
        [Authorize]
        [HttpPost("New")]
        public async Task<ActionResult<Return>> New([FromBody] Patrimony New)
        {
            var result = await _patrimony.NewPatrimony(New);
            await _hubContext.Clients.All.SendAsync("UpdatePatrimony");
            return result;
        }
        [Authorize]
        [HttpDelete("Delete/{id}")]
        public async Task<ActionResult<Return>> Delete(int id) => await _patrimony.DeletePatrimony(id);

        #endregion

        #region Category
        [Authorize]
        [HttpGet("Category/{id}")]
        public async Task<ActionResult<Patrimony_Category>> CategoryById(int id) => await _patrimony.Category_SearchBy(id);
        [Authorize]
        [HttpGet("Category/List")]
        public async Task<ActionResult<List<Patrimony_Category>>> CategoryList() => await _patrimony.CategoryList();
        [Authorize]
        [HttpPut("Category/Update")]
        public async Task<ActionResult<Return>> CategoryUpdate([FromBody] Patrimony_Category update)
        {
            var result = await _patrimony.UpdateCategory(update);
            await _hubContext.Clients.All.SendAsync("UpdatePCat");
            return result;
        }
        [Authorize]
        [HttpPost("Category/New")]
        public async Task<ActionResult<Return>> CategoryNew([FromBody] Patrimony_Category New)
        {
            var result =await _patrimony.NewCategory(New);
            await _hubContext.Clients.All.SendAsync("UpdatePCat");
            return result;
        }
        [Authorize]
        [HttpDelete("Category/Delete/{id}")]
        public async Task<ActionResult<Return>> CategoryDelete(int id) => await _patrimony.DeleteCategory(id);


        #endregion

        #region Computer
        [Authorize]
        [HttpGet("Computer/{id}")]
        public async Task<ActionResult<Computer>> ComputerById(int id) => await _patrimony.Computer_SearchByPc(id);
        [Authorize]
        [HttpGet("Computer/List")]
        public async Task<ActionResult<List<Computer>>> ComputerList() => await _patrimony.ComputerList();
        [Authorize]
        [HttpGet("Computer/ListByOwner/{id}")]
        public async Task<ActionResult<IEnumerable<PCDTO>>> ComputerListBy(int id) => Ok(await _patrimony.ComputerListByOwner(id));
        [Authorize]
        [HttpPut("Computer/Update")]
        public async Task<ActionResult<Return>> ComputerUpdate([FromBody] Computer update)
        {
            var result = await _patrimony.UpdateComputer(update);
            await _hubContext.Clients.All.SendAsync("UpdatePC");
            return result;
        }
        [Authorize]
        [HttpPost("Computer/New")]
        public async Task<ActionResult<Return>> ComputerNew([FromBody] Computer New)
        {
            var result = await _patrimony.NewComputer(New);
            await _hubContext.Clients.All.SendAsync("UpdatePC");
            return result;
        }
        [Authorize]
        [HttpDelete("Computer/Delete/{id}")]
        public async Task<ActionResult<Return>> ComputerDelete(int id) => await _patrimony.DeleteComputer(id);


        #endregion

    }
}
