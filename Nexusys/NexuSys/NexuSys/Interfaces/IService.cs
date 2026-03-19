using NexuSys.DTOs.ServiceEquipment;
using NexuSys.DTOs.ServiceItems;
using NexuSys.DTOs.ServiceOrder;
using NexuSys.DTOs.SItuation;
using NexuSys.DTOs.Type_Service;
using NexuSys.Entities;

namespace NexuSys.Interfaces
{
    public interface IService
    {
        #region ServiceOrder 
        Task<Return> NewServiceOrder(NewServiceOrderDTO ServiceOrder);
        Task<Return> UpdateServiceOrder(ByServiceOrderDTO ServiceOrder);
        Task<Return> DeleteServiceOrder(int id);
        Task<List<ServiceOrderDTO>> ServiceOrderList();
        Task<ByServiceOrderDTO> ServiceOrderByID(int id);
        Task<OSDetails> ServiceOrderByIDDetails(int id);
        Task<Return> CatchOS(int id, int user);

        #endregion

        #region Situation 
        Task<Return> NewSituation(NewSituationDTO Situation);
        Task<Return> UpdateSituation(BySituationDTO Situation);
        Task<Return> DeleteSituation(int id);
        Task<List<SituationDTO>> SituationList();
        Task<BySituationDTO> SituationByID(int id);

        #endregion

        #region Type_Service
        Task<Return> NewType_Service(NewType_ServiceDTO Type_Service);
        Task<Return> UpdateType_Service(ByType_ServiceDTO Type_Service);
        Task<Return> DeleteType_Service(int id);
        Task<List<Type_ServiceDTO>> Type_ServiceList();
        Task<ByType_ServiceDTO> Type_ServiceByID(int id);
        #endregion

        #region ServiceEquipment
        Task<Return> NewServiceEquipment(NewServiceEquipmentDTO ServiceEquipment);
        Task<Return> UpdateServiceEquipment(EditServiceEquipmentDTO ServiceEquipment);
        Task<Return> DeleteServiceEquipment(int id);
        Task<List<ServiceEquipmentDTO>> ServiceEquipmentList();
        Task<ByServiceEquipmentDTO> ServiceEquipmentByID(int id);
        #endregion

        #region ServiceItems
        Task<Return> NewServiceItems(List<ByServiceItemsDTO> ServiceItems);
        Task<Return> NewServiceItemsList(List<NewServiceItemsDTO> ServiceItems);
        Task<Return> UpdateServiceItems(EditServiceItemsDTO ServiceItems);
        Task<Return> DeleteServiceItems(int id);
        Task<List<ServiceItemsDTO>> ServiceItemsList();
        Task<ByServiceItemsDTO> ServiceItemsByID(int id);
        #endregion
    }
}
