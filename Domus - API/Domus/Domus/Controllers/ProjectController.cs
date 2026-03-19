using Domus.Models.DB;
using Domus.Models;
using Domus.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Domus.Controllers {
    [Route("Domus/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase {

        #region Property
        public readonly IProject _project;
        public ProjectController(IProject project) => _project = project;

        #endregion

        #region Project
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<Project>> Byid(int id) => await _project.Project_SearchById(id);
        [Authorize]
        [HttpGet("List")]
        public async Task<ActionResult<List<Project>>> List() => await _project.ListProject();
        [Authorize]
        [HttpPut("Update")]
        public async Task<ActionResult<Return>> Update([FromBody] Project update) => await _project.UpdateProject(update);
        [Authorize]
        [HttpPost("New")]
        public async Task<ActionResult<Return>> New([FromBody] Project New) => await _project.NewProject(New);
        [Authorize]
        [HttpDelete("Delete/{id}")]
        public async Task<ActionResult<Return>> Delete(int id) => await _project.DeleteProject(id);

        #endregion


        #region ProjectProducts
        [Authorize]
        [HttpGet("Products/{id}")]
        public async Task<ActionResult<Project_Products>> ProductByid(int id) => await _project.Products_SearchById(id);
        [Authorize]
        [HttpGet("Products/List")]
        public async Task<ActionResult<List<Project_Products>>> ProductList() => await _project.ListProducts();
        [Authorize]
        [HttpPut("Products/Update")]
        public async Task<ActionResult<Return>> ProductUpdate([FromBody] Project_Products update) => await _project.UpdateProducts(update);
        [Authorize]
        [HttpPost("Products/New")]
        public async Task<ActionResult<Return>> NewProduct([FromBody] Project_Products New) => await _project.NewProducts(New);
        [Authorize]
        [HttpDelete("Products/Delete/{id}")]
        public async Task<ActionResult<Return>> ProductDelete(int id) => await _project.DeleteProducts(id);

        #endregion

        #region Tasks
        [Authorize]
        [HttpGet("Tasks/{id}")]
        public async Task<ActionResult<Tasks>> TasksById(int id) => await _project.Task_SearchById(id);
        [Authorize]
        [HttpGet("Tasks/List")]
        public async Task<ActionResult<List<Tasks>>> TasksList() => await _project.ListTasks();
        [Authorize]
        [HttpPut("Tasks/Update")]
        public async Task<ActionResult<Return>> TasksUpdate([FromBody] Tasks update) => await _project.UpdateTask(update);
        [Authorize]
        [HttpPost("Tasks/New")]
        public async Task<ActionResult<Return>> NewTasks([FromBody] Tasks New) => await _project.NewTask(New);
        [Authorize]
        [HttpDelete("Tasks/Delete/{id}")]
        public async Task<ActionResult<Return>> TasksDelete(int id) => await _project.DeleteTask(id);

        #endregion
    }
}
