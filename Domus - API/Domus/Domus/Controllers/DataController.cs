using Domus.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Domus.Controllers
{
    [Route("Domus/[controller]")]
    [ApiController]
    public class DataController : ControllerBase
    {
        private readonly IHubContext<NotificationHub> _hubContext;

        public DataController(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        [HttpPost("Update")]
        public async Task<IActionResult> UpdateData()
        {
            // Simula a atualização dos dados no banco
            await Task.Delay(500); // Simulando processamento

            // Notifica os clientes conectados
            await _hubContext.Clients.All.SendAsync("UpdateList");

            return Ok("Atualização enviada!");
        }
    }
}
