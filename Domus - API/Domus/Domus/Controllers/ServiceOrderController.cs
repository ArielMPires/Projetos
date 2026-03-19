using Domus.Models.DB;
using Domus.Models;
using Domus.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Data;
using Domus.Services;
using Domus.DTO.Service_Type;
using System.Collections;
using Domus.DTO.Service_Order;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Domus.Controllers
{
    [Route("Domus/[controller]")]
    [ApiController]
    public class ServiceOrderController : ControllerBase
    {
        #region Property
        public readonly IService_Order _service;
        private readonly IHubContext<NotificationHub> _hubContext;
        #endregion
        public ServiceOrderController(IService_Order service, IHubContext<NotificationHub> hubContext)
        {
            _service = service;
            _hubContext = hubContext;
        }

        #region Service Order
        [Authorize]
        [HttpPost("New")]
        public async Task<ActionResult<Return>> NewOrder([FromBody] Service_Order order)
        {
            var result = await _service.NewOrder(order);
            await _hubContext.Clients.All.SendAsync("UpdateOS");
            return result;
        }
        [Authorize]
        [HttpPut("Update")]
        public async Task<ActionResult<Return>> UpdateOrder([FromBody] Service_Order order)
        {
            var result = await _service.UpdateOrder(order);
            await _hubContext.Clients.All.SendAsync("UpdateOS");
            return result;
        }
        [Authorize]
        [HttpPatch("EndOrder/{id}")]
        public async Task<ActionResult<Return>> EndOrder(int id,[FromBody] EndOrderDTO order)
        {
            var result = await _service.EndOrder(id,order);
            await _hubContext.Clients.All.SendAsync("UpdateOS");
            return result;
        }
        [HttpPatch("Order/{id}")]
        public async Task<ActionResult<Return>> CatchOrder(int id,[FromBody] CatchOrderDTO order)
        {
            var result = await _service.CatchOrder(id, order);
            await _hubContext.Clients.All.SendAsync("UpdateOS");
            return result;
        }
        [HttpPatch("Contact/{id}")]
        public async Task<ActionResult<Return>> ContactOrder(int id)
        {
            var result = await _service.ContactOrder(id);
            await _hubContext.Clients.All.SendAsync("UpdateOS");
            return result;
        }
        [Authorize]
        [HttpGet("ListByTechnical/{id}")]
        public async Task<ActionResult<IEnumerable<ServiceOrderDTO>>> ListTechnical(int id) => Ok(await _service.ListTechnicalOrder(id));
        [Authorize]
        [HttpGet("ListAll")]
        public async Task<ActionResult<IEnumerable<ServiceOrderDTO>>> ListAll() => Ok(await _service.ListAllOrder());
        [Authorize]
        [HttpGet("ListPending")]
        public async Task<ActionResult<IEnumerable<ServiceOrderDTO>>> ProductById() => Ok(await _service.ListPendingOrder());
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<Service_Order>> OrderById(int id) => await _service.Order(id);
        #endregion

        #region Service Items
        [Authorize]
        [HttpPost("Items/New")]
        public async Task<ActionResult<Return>> NewItem([FromBody] Service_Items item) => await _service.NewItemOrder(item);
        [Authorize]
        [HttpPut("Items/Update")]
        public async Task<ActionResult<Return>> UpdateItem([FromBody] Service_Items item) => await _service.NewItemOrder(item);
        [Authorize]
        [HttpDelete("Items/Delete/{id}")]
        public async Task<ActionResult<Return>> DeleteItem(int id) => await _service.DeleteItem(id);
        [Authorize]
        [HttpGet("Items/ListByOrder/{id}")]
        public async Task<ActionResult<List<Service_Items>>> ListItem(int id) => await _service.ListAllItemsByOrder(id);
        [Authorize]
        [HttpGet("Items/{id}")]
        public async Task<ActionResult<Service_Items>> ItemById(int id) => await _service.ItemById(id);
        #endregion

        #region Service Type
        [Authorize]
        [HttpPost("Type/New")]
        public async Task<ActionResult<Return>> NewType([FromBody] Service_Type type)
        {
            var result = await _service.NewType(type);
            await _hubContext.Clients.All.SendAsync("UpdateType");
            return result;
        }
        [Authorize]
        [HttpPut("Type/Update")]
        public async Task<ActionResult<Return>> UpdateType([FromBody] Service_Type type)
        {
            var result = await _service.UpdateType(type);
            await _hubContext.Clients.All.SendAsync("UpdateType");
            return result;
        }
        [Authorize]
        [HttpGet("Type/Delete/{id}")]
        public async Task<ActionResult<Return>> DeleteType(int id) => await _service.DeleteType(id);
        [Authorize]
        [HttpGet("Type/List")]
        public async Task<ActionResult<IEnumerable<TypeDTO>>> ListType() => Ok(await _service.ListAllType());
        [Authorize]
        [HttpGet("Type/List/{id}")]
        public async Task<ActionResult<IEnumerable<TypeDTO>>> ListTypeby(int id) => Ok(await _service.ListAllTypeByCategory(id));

        [Authorize]
        [HttpGet("Type/{id}")]
        public async Task<ActionResult<Service_Type>> TypeById(int id) => await _service.TypeById(id);
        #endregion

        #region Service Category
        [Authorize]
        [HttpPost("Category/New")]
        public async Task<ActionResult<Return>> NewCategory([FromBody] Service_Category category)
        {
            var result = await _service.NewCategory(category);
            await _hubContext.Clients.All.SendAsync("UpdateOSCategory");
            return result;
        }
        [Authorize]
        [HttpPut("Category/Update")]
        public async Task<ActionResult<Return>> UpdateCategory([FromBody] Service_Category category)
        {
            var result = await _service.UpdateCategory(category);
            await _hubContext.Clients.All.SendAsync("UpdateOSCategory");
            return result;
        }
        [Authorize]
        [HttpGet("Category/Delete/{id}")]
        public async Task<ActionResult<Return>> DeleteCategory(int id) => await _service.DeleteCategory(id);
        [Authorize]
        [HttpGet("Category/List")]
        public async Task<ActionResult<List<Service_Category>>> ListCategory() => await _service.ListAllCategory();
        [Authorize]
        [HttpGet("Category/{id}")]
        public async Task<ActionResult<Service_Category>> CategoryById(int id) => await _service.CategoryById(id);
        #endregion

        #region Service Execute
        [Authorize]
        [HttpPost("Execute/New")]
        public async Task<ActionResult<Return>> NewExecute([FromBody] Service_Execute execute) => await _service.NewExecute(execute);
        [Authorize]
        [HttpPut("Execute/Update")]
        public async Task<ActionResult<Return>> UpdateExecute([FromBody] Service_Execute execute) => await _service.UpdateExecute(execute);
        [Authorize]
        [HttpDelete("Execute/Delete/{id}")]
        public async Task<ActionResult<Return>> DeleteExecute(int id) => await _service.DeleteExecute(id);
        [Authorize]
        [HttpGet("Execute/List")]
        public async Task<ActionResult<List<Service_Execute>>> ListExecute() => await _service.ListAllExecute();
        [Authorize]
        [HttpGet("Execute/ListByOrder/{id}")]
        public async Task<ActionResult<List<Service_Execute>>> ListExecutebyorder(int id) => await _service.ListAllServiceByOrder(id);
        [Authorize]
        [HttpGet("Execute/{id}")]
        public async Task<ActionResult<Service_Execute>> ExecuteById(int id) => await _service.ExecuteById(id);
        #endregion

        #region Services
        [Authorize]
        [HttpPost("Services/New")]
        public async Task<ActionResult<Return>> NewServices([FromBody] Domus.Models.DB.Services servico)
        {
            var result = await _service.NewService(servico);
            await _hubContext.Clients.All.SendAsync("UpdateServices");
            return result;
        }
        [Authorize]
        [HttpPut("Services/Update")]
        public async Task<ActionResult<Return>> UpdateServices([FromBody] Domus.Models.DB.Services servico)
        {
            var result = await _service.UpdateService(servico);
            await _hubContext.Clients.All.SendAsync("UpdateServices");
            return result;
        }
        [Authorize]
        [HttpGet("Services/Delete/{id}")]
        public async Task<ActionResult<Return>> DeleteServices(int id) => await _service.DeleteService(id);
        [Authorize]
        [HttpGet("Services/List")]
        public async Task<ActionResult<List<Domus.Models.DB.Services>>> ListServices() => await _service.ListAllService();
        [Authorize]
        [HttpGet("Services/{id}")]
        public async Task<ActionResult<Domus.Models.DB.Services>> ServicesById(int id) => await _service.ServiceById(id);
        #endregion

        #region Service Checklist
        [Authorize]
        [HttpPost("Checklist/New")]
        public async Task<ActionResult<Return>> NewChecklist([FromBody] Service_CheckList check) => await _service.NewChecklist(check);
        [Authorize]
        [HttpPut("Checklist/Update")]
        public async Task<ActionResult<Return>> UpdateChecklist([FromBody] Service_CheckList check) => await _service.UpdateChecklist(check);
        [Authorize]
        [HttpDelete("Checklist/Delete/{id}")]
        public async Task<ActionResult<Return>> DeleteChecklist(int id) => await _service.DeleteChecklist(id);
        [Authorize]
        [HttpGet("Checklist/List")]
        public async Task<ActionResult<List<Service_CheckList>>> ListChecklist() => await _service.ListAllChecklist();
        [Authorize]
        [HttpGet("ExecuChecklistte/{id}")]
        public async Task<ActionResult<Service_CheckList>> ChecklistById(int id) => await _service.ChecklistById(id);
        [Authorize]
        [HttpGet("Checklist/ListByOrder/{id}")]
        public async Task<ActionResult<List<Service_CheckList>>> ListItembyorder(int id) => await _service.ListAllCheckByOrder(id);
        #endregion

        #region Service Rate
        [HttpPost("Rate/New")]
        public async Task<ActionResult<bool>> NewRate([FromBody] Service_Rate rate) => await _service.NewRate(rate);
        [Authorize]
        [HttpGet("Rate/List")]
        public async Task<ActionResult<List<Service_Rate>>> ListRate() => await _service.ListAllRate();
        [Authorize]
        [HttpGet("Rate/List/{id}")]
        public async Task<ActionResult<List<Service_Rate>>> ListRateByTechnical(int id) => await _service.ListByTechnical(id);
        #endregion
    }
}
