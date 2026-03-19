using Agnus.Models.DB;
using Agnus.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domus.DTO.Service_Order;
using Agnus.DTO.Service_Order;
using Domus.DTO.Service_Type;

namespace Agnus.Interfaces
{
    public interface IService_Order
    {
        public void SetHeader(string tenantId, string token);
        #region Service Order
        public Task<Return> NewOrder(Service_Order order);
        public Task<IEnumerable<ServiceOrderDTO>> ListAllOrder();
        public Task<IEnumerable<ServiceOrderDTO>> ListTechnicalOrder(int id);
        public Task<IEnumerable<ServiceOrderDTO>> ListPendingOrder();
        public Task<Return> UpdateOrder(Service_Order order);
        public Task<Return> EndOrder(int id,EndOrderDTO order);
        public Task<Service_Order> Order(int id);
        public Task<Return> CatchOrder(int id, CatchOrderDTO order);
        public Task<Return> ContactOrder(int id);
        #endregion

        #region Service Items
        public Task<Return> NewItemOrder(Service_Items item);
        public Task<Return> UpdateItem(Service_Items item);
        public Task<List<Service_Items>> ListAllItemsByOrder(int order);
        public Task<Service_Items> ItemById(int id);
        public Task<Return> DeleteItem(int id);
        #endregion

        #region Service Type
        public Task<Return> NewType(Service_Type type);
        public Task<IEnumerable<TypeDTO>> ListAllType();
        public Task<IEnumerable<TypeDTO>> ListAllTypeByCategory(int id);
        public Task<Return> UpdateType(Service_Type type);
        public Task<Return> DeleteType(int typyId);
        public Task<Service_Type> TypeById(int typyId);
        #endregion

        #region Service Category
        public Task<Return> NewCategory(Service_Category category);
        public Task<List<Service_Category>> ListAllCategory();
        public Task<Return> UpdateCategory(Service_Category category);
        public Task<Return> DeleteCategory(int categoryId);
        public Task<Service_Category> CategoryById(int categoryId);
        #endregion

        #region Service Execute
        public Task<Return> NewExecute(Service_Execute execute);
        public Task<List<Service_Execute>> ListAllExecute();
        public Task<Return> UpdateExecute(Service_Execute execute);
        public Task<Return> DeleteExecute(int executeId);
        public Task<Service_Execute> ExecuteById(int executeId);
        public Task<List<Service_Execute>> ListAllServiceByOrder(int order); 
        #endregion

        #region Services
        public Task<Return> NewService(Agnus.Models.DB.Services service);
        public Task<List<Agnus.Models.DB.Services>> ListAllService();
        public Task<Return> UpdateService(Agnus.Models.DB.Services service);
        public Task<Return> DeleteService(int serviceId);
        public Task<Agnus.Models.DB.Services> ServiceById(int serviceId);
        #endregion

        #region Service Checklist
        public Task<Return> NewChecklist(Service_CheckList checkList);
        public Task<List<Service_CheckList>> ListAllChecklist();
        public Task<Return> UpdateChecklist(Service_CheckList checkList);
        public Task<Return> DeleteChecklist(int checklistId);
        public Task<Service_CheckList> ChecklistById(int checklistId);
        public Task<List<Service_CheckList>> ListAllCheckByOrder(int order);
        #endregion
    }
}
