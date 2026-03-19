using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using NexuSys.Data;
using NexuSys.DTOs.ServiceEquipment;
using NexuSys.DTOs.ServiceItems;
using NexuSys.DTOs.ServiceOrder;
using NexuSys.DTOs.SItuation;
using NexuSys.DTOs.Type_Service;
using NexuSys.Entities;
using NexuSys.Interfaces;

namespace NexuSys.Services
{
    public class OSService : IService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public OSService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        #region ServiceEquipment

        public async Task<Return> NewServiceEquipment(NewServiceEquipmentDTO dto)
        {
            var result = new Return();
            try
            {
                var entity = _mapper.Map<Service_Equipment>(dto);
                await _context.Service_Equipment.AddAsync(entity);
                await _context.SaveChangesAsync();

                result.Result = true;
                result.Message = "Equipamento de serviço criado com sucesso";
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = ex.Message;
                result.Error = ex.InnerException?.Message;
            }
            return result;
        }

        public async Task<Return> UpdateServiceEquipment(EditServiceEquipmentDTO dto)
        {
            var result = new Return();
            try
            {
                var entity = _mapper.Map<Service_Equipment>(dto);
                _context.Service_Equipment.Update(entity);
                await _context.SaveChangesAsync();

                result.Result = true;
                result.Message = "Equipamento de serviço atualizado com sucesso";
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = ex.Message;
                result.Error = ex.InnerException?.Message;
            }
            return result;
        }

        public async Task<Return> DeleteServiceEquipment(int id)
        {
            var result = new Return();
            try
            {
                var entity = await _context.Service_Equipment.FirstOrDefaultAsync(e => e.ID == id);
                _context.Service_Equipment.Remove(entity);
                await _context.SaveChangesAsync();

                result.Result = true;
                result.Message = $"Equipamento ID:{id} removido";
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = ex.Message;
                result.Error = ex.InnerException?.Message;
            }
            return result;
        }

        public async Task<ByServiceEquipmentDTO> ServiceEquipmentByID(int id) =>
            await _context.Service_Equipment
                .AsNoTracking()
                .ProjectTo<ByServiceEquipmentDTO>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(e => e.OS == id);

        public async Task<List<ServiceEquipmentDTO>> ServiceEquipmentList() =>
            await _context.Service_Equipment
                .AsNoTracking()
                .ProjectTo<ServiceEquipmentDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();

        #endregion

        #region ServiceItems

        public async Task<Return> NewServiceItems(List<ByServiceItemsDTO> dto)
        {
            var result = new Return();
            try
            {

                var search = await _context.Service_Items.AsNoTracking().Where(e => e.OS == dto[0].OS).ToListAsync();

                foreach(var del in search)
                {
                    var x = dto.FirstOrDefault(e => e.Product == del.Product);

                    if(x == null)
                    {
                         _context.Service_Items.Remove(del);
                        await _context.SaveChangesAsync();
                    }
                }

                foreach (var item in dto)
                {

                    var entity = _mapper.Map<Service_Items>(item);

                    if (entity.ID == 0)
                    {
                        await _context.Service_Items.AddAsync(entity);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        _context.Service_Items.Update(entity);
                        await _context.SaveChangesAsync();
                    }
                }

                result.Result = true;
                result.Message = "Item de serviço criado com sucesso";
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = ex.Message;
                result.Error = ex.InnerException?.Message;
            }
            return result;
        }
        public async Task<Return> NewServiceItemsList(List<NewServiceItemsDTO> dto)
        {
            var result = new Return();
            try
            {

                foreach(var item in dto)
                {
                var entity = _mapper.Map<Service_Items>(item);
                await _context.Service_Items.AddAsync(entity);
                }

                await _context.SaveChangesAsync();
                result.Result = true;
                result.Message = "Item de serviço criado com sucesso";
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = ex.Message;
                result.Error = ex.InnerException?.Message;
            }
            return result;
        }

        public async Task<Return> UpdateServiceItems(EditServiceItemsDTO dto)
        {
            var result = new Return();
            try
            {
                var entity = _mapper.Map<Service_Items>(dto);
                _context.Service_Items.Update(entity);
                await _context.SaveChangesAsync();

                result.Result = true;
                result.Message = "Item de serviço atualizado";
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = ex.Message;
                result.Error = ex.InnerException?.Message;
            }
            return result;
        }

        public async Task<Return> DeleteServiceItems(int id)
        {
            var result = new Return();
            try
            {
                var entity = await _context.Service_Items.FirstOrDefaultAsync(e => e.ID == id);
                _context.Service_Items.Remove(entity);
                await _context.SaveChangesAsync();

                result.Result = true;
                result.Message = $"Item ID:{id} removido";
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = ex.Message;
                result.Error = ex.InnerException?.Message;
            }
            return result;
        }

        public async Task<ByServiceItemsDTO> ServiceItemsByID(int id) =>
            await _context.Service_Items
                .AsNoTracking()
                .ProjectTo<ByServiceItemsDTO>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(e => e.ID == id);

        public async Task<List<ServiceItemsDTO>> ServiceItemsList() =>
            await _context.Service_Items
                .AsNoTracking()
                .ProjectTo<ServiceItemsDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();

        #endregion

        #region ServiceOrder

        public async Task<Return> CatchOS(int id, int user)
        {
            var result = new Return();
            try
            {
                await _context.Service_Order.Where(x => x.ID == id).ExecuteUpdateAsync(s => s.SetProperty(x => x.Technical, user));
                await _context.SaveChangesAsync();

                result.Result = true;
                result.Message = "Ordem de serviço Pega!";
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = ex.Message;
                result.Error = ex.InnerException?.Message;
            }
            return result;
        }

        public async Task<Return> NewServiceOrder(NewServiceOrderDTO dto)
        {
            var result = new Return();
            try
            {
                var entity = _mapper.Map<Service_Order>(dto);
                await _context.Service_Order.AddAsync(entity);
                await _context.SaveChangesAsync();
                _context.Entry(entity).State = EntityState.Detached;
                result.Result = true;
                result.Message = "Ordem de serviço criada";
                result.Data = entity;
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = ex.Message;
                result.Error = ex.InnerException?.Message;
            }
            return result;
        }

        public async Task<Return> UpdateServiceOrder(ByServiceOrderDTO dto)
        {
            var result = new Return();
            try
            {
                var entity = _mapper.Map<Service_Order>(dto);
                _context.Service_Order.Update(entity);
                await _context.SaveChangesAsync();
                _context.Entry(entity).State = EntityState.Detached;
                result.Result = true;
                result.Message = "Ordem de serviço atualizada";
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = ex.Message;
                result.Error = ex.InnerException?.Message;
            }
            return result;
        }

        public async Task<Return> DeleteServiceOrder(int id)
        {
            var result = new Return();
            try
            {
                var entity = await _context.Service_Order.FirstOrDefaultAsync(e => e.ID == id);
                _context.Service_Order.Remove(entity);
                await _context.SaveChangesAsync();

                result.Result = true;
                result.Message = $"OS ID:{id} removida";
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = ex.Message;
                result.Error = ex.InnerException?.Message;
            }
            return result;
        }

        public async Task<List<ServiceOrderDTO>> ServiceOrderList() =>
            await _context.Service_Order
                .AsNoTracking()
                .ProjectTo<ServiceOrderDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();

        public async Task<ByServiceOrderDTO> ServiceOrderByID(int id) =>
            await _context.Service_Order
                .AsNoTracking()
                .ProjectTo<ByServiceOrderDTO>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(e => e.ID == id);
        public async Task<OSDetails> ServiceOrderByIDDetails(int id) =>
            await _context.Service_Order
                .AsNoTracking()
            .Include(e => e.BudgetFK)
            .ThenInclude(e => e.ReviewFK)
            .ThenInclude(e => e.DefectsFK)
            .Include(e => e.BudgetFK.ReviewFK.ActiviesFK)
            .Include(e => e.service_equipmentFK)
            .Include(e => e.ItemsFK)
            .ProjectTo<OSDetails>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(e => e.ID == id);

        #endregion

        #region Situation
        public async Task<Return> NewSituation(NewSituationDTO dto)
        {
            var result = new Return();
            try
            {
                var entity = _mapper.Map<Situation>(dto);
                await _context.Situation.AddAsync(entity);
                await _context.SaveChangesAsync();
                _context.Entry(entity).State = EntityState.Detached;
                result.Result = true;
                result.Message = "Situação criada";
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = ex.Message;
                result.Error = ex.InnerException?.Message;
            }
            return result;
        }

        public async Task<Return> UpdateSituation(BySituationDTO dto)
        {
            var result = new Return();
            try
            {
                var entity = _mapper.Map<Situation>(dto);

                _context.Situation.Update(entity);
                await _context.SaveChangesAsync();
                _context.Entry(entity).State = EntityState.Detached;


                result.Result = true;
                result.Message = "Situação atualizada";
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = ex.Message;
                result.Error = ex.InnerException?.Message;
            }
            return result;
        }

        public async Task<Return> DeleteSituation(int id)
        {
            var result = new Return();
            try
            {
                var entity = await _context.Situation.FirstOrDefaultAsync(e => e.ID == id);
                _context.Situation.Remove(entity);
                await _context.SaveChangesAsync();

                result.Result = true;
                result.Message = $"Situação ID:{id} removida";
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = ex.Message;
                result.Error = ex.InnerException?.Message;
            }
            return result;
        }

        public async Task<List<SituationDTO>> SituationList() =>
            await _context.Situation
                .AsNoTracking()
                .ProjectTo<SituationDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();

        public async Task<BySituationDTO> SituationByID(int id) =>
            await _context.Situation
                .AsNoTracking()
                .ProjectTo<BySituationDTO>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(e => e.ID == id);

        #endregion

        #region Type_Service

        public async Task<Return> NewType_Service(NewType_ServiceDTO dto)
        {
            var result = new Return();
            try
            {
                var entity = _mapper.Map<Type_Service>(dto);
                await _context.Type_Service.AddAsync(entity);
                await _context.SaveChangesAsync();
                _context.Entry(entity).State = EntityState.Detached;
                result.Result = true;
                result.Message = "Tipo de serviço criado";
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = ex.Message;
                result.Error = ex.InnerException?.Message;
            }
            return result;
        }

        public async Task<Return> UpdateType_Service(ByType_ServiceDTO dto)
        {
            var result = new Return();
            try
            {
                var entity = _mapper.Map<Type_Service>(dto);
                _context.Type_Service.Update(entity);
                await _context.SaveChangesAsync();
                _context.Entry(entity).State = EntityState.Detached;

                result.Result = true;
                result.Message = "Tipo de serviço atualizado";
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = ex.Message;
                result.Error = ex.InnerException?.Message;
            }
            return result;
        }

        public async Task<Return> DeleteType_Service(int id)
        {
            var result = new Return();
            try
            {
                var entity = await _context.Type_Service.FirstOrDefaultAsync(e => e.ID == id);
                _context.Type_Service.Remove(entity);
                await _context.SaveChangesAsync();

                result.Result = true;
                result.Message = $"Tipo de serviço ID:{id} removido";
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = ex.Message;
                result.Error = ex.InnerException?.Message;
            }
            return result;
        }

        public async Task<ByType_ServiceDTO> Type_ServiceByID(int id) =>
            await _context.Type_Service
                .AsNoTracking()
                .ProjectTo<ByType_ServiceDTO>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(e => e.ID == id);

        public async Task<List<Type_ServiceDTO>> Type_ServiceList() =>
            await _context.Type_Service
                .AsNoTracking()
                .ProjectTo<Type_ServiceDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();

        #endregion
    }
}
