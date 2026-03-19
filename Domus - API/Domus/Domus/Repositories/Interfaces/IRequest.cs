using Domus.Models.DB;
using Domus.Models;
using Domus.DTO.Request;
using Domus.DTO.Purchase_Order;

namespace Domus.Repositories.Interfaces {
    public interface IRequest {

        #region Request
        Task<Return> NewRequest(Request New);
        Task<Return> UpdateRequest(Request Update);
        Task<Return> DeleteRequest(int Delete); 
        Task<IEnumerable<RequestDTO>> RequestList();
        Task<IEnumerable<RequestDTO>> RequestListPending();
        Task<Request> Request_SearchByPc(int id);
        #endregion

        #region Aprroval
        Task<Return> NewAprroval(Request New);
        Task<Return> UpdateAprroval (Request_Approval Update);
        Task<Return> DeleteAprroval (int Delete);
        Task<List<Request_Approval>> AprrovalList();
        Task<Request_Approval> Approval_SearchByPc (int id);
        #endregion

        #region Items
        Task<Return> New_Items(Request_Items New);
        Task<Return> UpdateItems(Request_Items Update);
        Task<Return> DeleteItems(int Delete);
        Task<List<Request_Items>> ItemsList();
        Task<Request_Items> Items_SearchByPc(int id);
        #endregion

        #region Usage
        Task<Return> New_Usage(Request_Usage New);
        Task<Return> UpdateUsage(Request_Usage Update);
        Task<Return> DeleteUsage(int Delete);
        Task <List<Request_Usage>> UsageList();
        Task<Request_Usage> Usage_SearchByPc(int id);
        #endregion

        #region Order
        Task<Return> New_Order(Purchase_Order New);
        Task<Return> UpdateOrder(Purchase_Order Update);
        Task<Return> DeleteOrder(int Delete);
        Task<IEnumerable<Purchase_OrderDTO>> PurchaseList();
        Task<IEnumerable<Purchase_OrderDTO>> PurchaseListDelivered();
        Task<Purchase_Order> Purchase_SearchByPc(int id);
        #endregion
    }
}
