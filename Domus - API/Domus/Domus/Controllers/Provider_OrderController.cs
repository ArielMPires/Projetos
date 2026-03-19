using Domus.Models.DB;
using Domus.Models;
using Domus.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Domus.Controllers {
    [Route("Domus/[controller]")]
    [ApiController]
    public class Provider_OrderController : ControllerBase {

        #region Property
        public readonly IProvider_Order _providerorder;
        public Provider_OrderController(IProvider_Order provider) => _providerorder = provider;

        #endregion

        #region ProviderOrder
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<Provider_Order>> Byid(int id) => await _providerorder.Provider_SearchById(id);
        [Authorize]
        [HttpGet("List")]
        public async Task<ActionResult<List<Provider_Order>>> List() => await _providerorder.ListProvider();
        [Authorize]
        [HttpPut("Update")]
        public async Task<ActionResult<Return>> Update([FromBody] Provider_Order update) => await _providerorder.UpdateProvider(update);
        [Authorize]
        [HttpPost("New")]
        public async Task<ActionResult<Return>> New([FromBody] Provider_Order newprovider) => await _providerorder.NewProvider(newprovider);
        [Authorize]
        [HttpDelete("Delete/{id}")]
        public async Task<ActionResult<Return>> Delete(int id) => await _providerorder.DeleteProvider(id);

        #endregion

        #region ServiceProvider
        [Authorize]
        [HttpGet("Service/{id}")]
        public async Task<ActionResult<Service_Providers>> ServiceByid(int id) => await _providerorder.Service_SearchById(id);
        [Authorize]
        [HttpGet("Service/List")]
        public async Task<ActionResult<List<Service_Providers>>> ServiceList() => await _providerorder.ListServices();
        [Authorize]
        [HttpPut("Service/Update")]
        public async Task<ActionResult<Return>> ServiceUpdate([FromBody] Service_Providers Update) => await _providerorder.UpdateService(Update);
        [Authorize]
        [HttpPost("Service/New")]
        public async Task<ActionResult<Return>> ServiceNew([FromBody] Service_Providers New) => await _providerorder.NewService(New);
        [Authorize]
        [HttpDelete("Service/Delete/{id}")]
        public async Task<ActionResult<Return>> ServiceDelete(int id) => await _providerorder.DeleteService(id);

        #endregion

    }
}
