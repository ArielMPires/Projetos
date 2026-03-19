using Domus.DTO.Purchase_Order;
using Domus.DTO.Request;
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

    public class RequestController : ControllerBase
    {

        public readonly IRequest _request;
        private readonly IHubContext<NotificationHub> _hubContext;
        public RequestController(IRequest request, IHubContext<NotificationHub> hubContext)
        {
            _request = request;
            _hubContext = hubContext;
        }

        #region Request
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<Request>> ById(int id) => await _request.Request_SearchByPc(id);
        [Authorize]
        [HttpGet("List")]
        public async Task<ActionResult<IEnumerable<RequestDTO>>> List() => Ok(await _request.RequestList());
        [Authorize]
        [HttpGet("ListPending")]
        public async Task<ActionResult<IEnumerable<RequestDTO>>> ListP() => Ok(await _request.RequestListPending());
        [Authorize]
        [HttpPut("Update")]
        public async Task<ActionResult<Return>> Update([FromBody] Request update)
        {
            var result = await _request.UpdateRequest(update);
            await _hubContext.Clients.All.SendAsync("UpdateRequest");
            return result;
        }

        [Authorize]
        [HttpPost("New")]
        public async Task<ActionResult<Return>> New([FromBody] Request New)
        {
            var result = await _request.NewRequest(New);
            await _hubContext.Clients.All.SendAsync("UpdateRequest");
            return result;
        }

        [Authorize]
        [HttpDelete("Delete/{id}")]
        public async Task<ActionResult<Return>> Delete(int id) => await _request.DeleteRequest(id);


        #endregion

        #region Aprroval
        [Authorize]
        [HttpGet("Aprroval/{id}")]
        public async Task<ActionResult<Request_Approval>> byid(int id) => await _request.Approval_SearchByPc(id);
        [Authorize]
        [HttpGet("Approval/List")]
        public async Task<ActionResult<List<Request_Approval>>> Lists() => await _request.AprrovalList();
        [Authorize]
        [HttpPut("Approval/Update")]
        public async Task<ActionResult<Return>> Updats([FromBody] Request_Approval update) => await _request.UpdateAprroval(update);
        [Authorize]
        [HttpPost("Approval/New")]
        public async Task<ActionResult<Return>> NewApproval([FromBody] Request New)
        {
            var result = await _request.NewAprroval(New);
            await _hubContext.Clients.All.SendAsync("UpdateData");
            return result;
        }
        [Authorize]
        [HttpDelete("Approval/Delete/{id}")]
        public async Task<ActionResult<Return>> Deletes(int id) => await _request.DeleteAprroval(id);

        #endregion

        #region Items
        [Authorize]
        [HttpGet("Items/{id}")]
        public async Task<ActionResult<Request_Items>> byiditems(int id) => await _request.Items_SearchByPc(id);
        [Authorize]
        [HttpGet("Items/List")]
        public async Task<ActionResult<List<Request_Items>>> ItemList() => await _request.ItemsList();
        [Authorize]
        [HttpPut("Items/Update")]
        public async Task<ActionResult<Return>> UpdateItems([FromBody] Request_Items update) => await _request.UpdateItems(update);
        [Authorize]
        [HttpPost("Items/New")]
        public async Task<ActionResult<Return>> NewItems([FromBody] Request_Items New) => await _request.New_Items(New);
        [Authorize]
        [HttpDelete("Items/Delete/{id}")]
        public async Task<ActionResult<Return>> DeleteItems(int id) => await _request.DeleteItems(id);

        #endregion

        #region Usage
        [Authorize]
        [HttpGet("Usage/{id}")]
        public async Task<ActionResult<Request_Usage>> ByIdUsage(int id) => await _request.Usage_SearchByPc(id);
        [Authorize]
        [HttpGet("Usage/List")]
        public async Task<ActionResult<List<Request_Usage>>> UsageLista() => await _request.UsageList();
        [Authorize]
        [HttpPut("Usage/Update")]
        public async Task<ActionResult<Return>> UsageItems([FromBody] Request_Usage update)
        {
            var result = await _request.UpdateUsage(update);
            await _hubContext.Clients.All.SendAsync("UpdateUse");
            return result;
        }
        [Authorize]
        [HttpPost("Usage/New")]
        public async Task<ActionResult<Return>> NewUsage([FromBody] Request_Usage New)
        {
            var result = await _request.New_Usage(New);
            await _hubContext.Clients.All.SendAsync("UpdateUse");
            return result;
        }
        [Authorize]
        [HttpDelete("Usage/Delete/{id}")]
        public async Task<ActionResult<Return>> DeleteUsage(int id) => await _request.DeleteUsage(id);

        #endregion

        #region Order
        [Authorize]
        [HttpGet("Order/{id}")]
        public async Task<ActionResult<Purchase_Order>> ByIdOrder(int id) => await _request.Purchase_SearchByPc(id);
        [Authorize]
        [HttpGet("Order/List")]
        public async Task<ActionResult<IEnumerable<Purchase_OrderDTO>>> OrderLista() => Ok(await _request.PurchaseList());
        [Authorize]
        [HttpGet("Order/ListPending")]
        public async Task<ActionResult<IEnumerable<Purchase_OrderDTO>>> OrderListaP() => Ok(await _request.PurchaseListDelivered());
        [Authorize]
        [HttpPut("Order/Update")]
        public async Task<ActionResult<Return>> OrderItems([FromBody] Purchase_Order update)
        {
            var result = await _request.UpdateOrder(update);
            await _hubContext.Clients.All.SendAsync("UpdateROrder");
            return result;
        }
        [Authorize]
        [HttpPost("Order/New")]
        public async Task<ActionResult<Return>> NewOrder([FromBody] Purchase_Order New)
        {
            var result = await _request.New_Order(New);
            await _hubContext.Clients.All.SendAsync("UpdateROrder");
            return result;
        }
        [Authorize]
        [HttpDelete("Order/Delete/{id}")]
        public async Task<ActionResult<Return>> DeleteOrder(int id) => await _request.DeleteOrder(id);

        #endregion

    }
}
