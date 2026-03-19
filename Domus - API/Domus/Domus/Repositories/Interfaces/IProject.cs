using Domus.Models;
using Domus.Models.DB;

namespace Domus.Repositories.Interfaces {
    public interface IProject {

        #region Project
        Task<Return> NewProject(Project New);
        Task<Return> UpdateProject(Project Update);
        Task<Return> DeleteProject(int Delete);
        Task<List<Project>> ListProject();
        Task<Project> Project_SearchById(int id);

        #endregion

        #region ProjectProducts
        Task<Return> NewProducts(Project_Products New);
        Task<Return> UpdateProducts(Project_Products Update);
        Task<Return> DeleteProducts(int Delete);
        Task<List<Project_Products>> ListProducts();
        Task<Project_Products> Products_SearchById(int id);

        #endregion

        #region Tasks
        Task<Return> NewTask(Tasks New);
        Task<Return> UpdateTask(Tasks Update);
        Task<Return> DeleteTask(int Delete);
        Task<List<Tasks>> ListTasks();
        Task<Tasks> Task_SearchById(int id);

        #endregion
    }
}
