using Domus.Models.DB;
using Domus.Models;
using Domus.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.AspNetCore.Authorization;
using Domus.DTO.Passwords;

namespace Domus.Controllers {
    [Route("Domus/[controller]")]
    [ApiController]
    public class PasswordController : ControllerBase {

        public readonly IPassword _password;
        public PasswordController(IPassword passwords) => _password = passwords;

        #region Passoword
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<Passwords>> ById(int id) => await _password.Password_SearchBy(id);
        [Authorize]
        [HttpGet("List")]
        public async Task<ActionResult<IEnumerable<PasswordDTO>>> List() => Ok(await _password.PasswordsList());
        [Authorize]
        [HttpPut("Update")]
        public async Task<ActionResult<Return>> Update([FromBody] Passwords update) => await _password.UpdatePassoword(update);
        [Authorize]
        [HttpPost("New")]
        public async Task<ActionResult<Return>> New([FromBody] PasswordDTO New) => await _password.NewPassoword(New);
        [Authorize]
        [HttpDelete("Delete/{id}")]
        public async Task<ActionResult<Return>> Delete(int id) => await _password.DeletePassoword(id);

        #endregion

        #region Type_Passoword
        [Authorize]
        [HttpGet("Type/{id}")]
        public async Task<ActionResult<Type_Passwords>> TypeByid(int id) => await _password.Type_SearchBy(id);
        [Authorize]
        [HttpGet("Type/List")]
        public async Task<ActionResult<List<Type_Passwords>>> TypeList() => await _password.TypeList();
        [Authorize]
        [HttpPut("Type/Update")]
        public async Task<ActionResult<Return>> TypeUpdate([FromBody] Type_Passwords update) => await _password.UpdateType(update);
        [Authorize]
        [HttpPost("Type/New")]
        public async Task<ActionResult<Return>> TypeNew([FromBody] Type_Passwords New) => await _password.NewType(New);
        [Authorize]
        [HttpDelete("Type/Delete/{id}")]
        public async Task<ActionResult<Return>> TypeDelete(int id) => await _password.DeleteType(id);

        #endregion
    }
}
