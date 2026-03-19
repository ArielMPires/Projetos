using Agnus.Models;
using Agnus.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agnus.Interfaces
{
    public interface IProject
    {
        public void SetHeader(string tenantId, string token);
        public void SetTenantHeader(string tenantId);

        #region Project
        Task<Return> NewProject(Project New);
        Task<Return> DeleteProject(int Delete);
        Task<Return> UpdateProject(Project Update);
        Task<List<Project>> ListProject();
        Task<Project> Project_SearchById(int id);
        #endregion

        #region Project Products
        Task<Return> NewProducts(Project_Products New);
        Task<Return> DeleteProducts(int Delete);
        Task<Return> UpdateProducts(Project_Products Update);
        Task<List<Project_Products>> ListProducts();
        Task<Project_Products> Products_SearchById(int id);
        #endregion

        #region Tasks
        Task<Return> NewTask(Tasks New);
        Task<Return> DeleteTask(int Delete);
        Task<Return> UpdateTask(Tasks Update);
        Task<List<Tasks>> ListTasks();
        Task<Tasks> Task_SearchById(int id);
        #endregion
    }
}
