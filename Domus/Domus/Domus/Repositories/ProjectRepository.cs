using Domus.DataBase;
using Domus.Models;
using Domus.Models.DB;
using Domus.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Domus.Repositories {
    public class ProjectRepository : IProject {


        #region Property
        public ApplicationDbContext db { get; set; }
        public ProjectRepository(ApplicationDbContext context) => db = context;

        #endregion

        #region Project
        public async Task<Project> Project_SearchById(int id) => await db.Project.FirstOrDefaultAsync(e => e.ID == id);
        public async Task<List<Project>> ListProject() => await db.Project.ToListAsync();
        
        public async Task<Return> NewProject(Project New) {

            var result = new Return();

            try {
                
                await db.Project.AddAsync(New);
                await db.SaveChangesAsync();
                result.Result = true;
                result.Message = "Projeto Cadastrado!"; 

            } catch (Exception ex) {

                result.Result = false;
                result.Message = "Ocorreu um Erro Interno!";
                result.Error = ex.Message;
            }
            return result;
        }
        public async Task<Return> UpdateProject(Project Update) {

            var result = new Return();

            try {

                db.Project.Update(Update);
                await db.SaveChangesAsync();
                result.Result = true;
                result.Message = "Projeto Atualizado!";

            } catch (Exception ex) {

                result.Result = false;
                result.Message = "Ocorreu um Erro Interno!";
                result.Error = ex.Message;
            }
            return result;
        }
        public async Task<Return> DeleteProject(int Delete) {
            
            var result = new Return();

            try {

                var search = await db.Project.FirstOrDefaultAsync(e => e.ID == Delete);
                db.Project.Remove(search);
                await db.SaveChangesAsync();
                result.Result = true;
                result.Message = "Projeto Deletado!";

            } catch (Exception ex) {

                result.Result = false;
                result.Message = "Ocorreu um Erro Interno!";
                result.Error = ex.Message;
            }
            return result;
        }

        #endregion

        #region Products
        public async Task<Project_Products> Products_SearchById(int id) => await db.Project_Products.FirstOrDefaultAsync(e => e.ID == id);
        public async Task<List<Project_Products>> ListProducts() => await db.Project_Products.ToListAsync();

        public async Task<Return> NewProducts(Project_Products New) {

            var result = new Return();

            try {

                await db.Project_Products.AddAsync(New);
                await db.SaveChangesAsync();
                result.Result = true;
                result.Message = "Projeto Cadastrado!";

            } catch (Exception ex) {

                result.Result = false;
                result.Message = "Ocorreu um Erro Interno!";
                result.Error = ex.Message;
            }
            return result;
        }
        public async Task<Return> UpdateProducts(Project_Products Update) {

            var result = new Return();

            try {

                db.Project_Products.Update(Update);
                await db.SaveChangesAsync();
                result.Result = true;
                result.Message = "Projeto Atualizado!";

            } catch (Exception ex) {

                result.Result = false;
                result.Message = "Ocorreu um Erro Interno!";
                result.Error = ex.Message;
            }
            return result;
        }

        public async Task<Return> DeleteProducts(int Delete) {

            var result = new Return();

            try {

                var search = await db.Project_Products.FirstOrDefaultAsync(e => e.ID == Delete);
                db.Project_Products.Remove(search);
                await db.SaveChangesAsync();
                result.Result = true;
                result.Message = "Projeto Deletado!";

            } catch (Exception ex) {

                result.Result = false;
                result.Message = "Ocorreu um Erro Interno!";
                result.Error = ex.Message;
            }
            return result;
        }

        #endregion

        #region Tasks
        public async Task<Tasks> Task_SearchById(int id) => await db.Tasks.FirstOrDefaultAsync(e => e.ID == id);
        public async Task<List<Tasks>> ListTasks() => await db.Tasks.ToListAsync(); 

        public async Task<Return> NewTask(Tasks New) {

            var result = new Return();

            try {

                await db.Tasks.AddAsync(New);
                await db.SaveChangesAsync();
                result.Result = true;
                result.Message = "Tarefa Cadastrado!";

            } catch (Exception ex) {

                result.Result = false;
                result.Message = "Ocorreu um Erro Interno!";
                result.Error = ex.Message;
            }
            return result;
        }

        public async Task<Return> UpdateTask(Tasks Update) {
            
            var result = new Return();

            try {

                db.Tasks.Update(Update);
                await db.SaveChangesAsync();
                result.Result = true;
                result.Message = "Tarefa Atualizado!";

            } catch (Exception ex) {

                result.Result = false;
                result.Message = "Ocorreu um Erro Interno!";
                result.Error = ex.Message;
            }
            return result;
        }
        public async Task<Return> DeleteTask(int Delete) {

            var result = new Return();

            try {

                var search = await db.Tasks.FirstOrDefaultAsync(e => e.ID == Delete);
                db.Tasks.Remove(search);
                await db.SaveChangesAsync();
                result.Result = true;
                result.Message = "Tarefa Deletada!";

            } catch (Exception ex) {

                result.Result = false;
                result.Message = "Ocorreu um Erro Interno!";
                result.Error = ex.Message;
            }
            return result;
        }

        #endregion

    }
}
