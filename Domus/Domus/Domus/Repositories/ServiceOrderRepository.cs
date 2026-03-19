using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domus.DataBase;
using Domus.DTO.Service_Order;
using Domus.DTO.Service_Type;
using Domus.Models;
using Domus.Models.DB;
using Domus.Repositories.Interfaces;
using Domus.Services;
using Microsoft.AspNetCore.Http.Timeouts;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using System.Runtime.InteropServices;

namespace Domus.Repositories
{
    public class ServiceOrderRepository : IService_Order
    {
        #region Property
        private readonly ApplicationDbContext _context;
        private readonly ITenantDbContextFactory _tenantDbContext;
        private readonly TenantProvider _tenantProvider;
        private readonly IMapper _mapper;
        #endregion

        public ServiceOrderRepository(IMapper mapper, ApplicationDbContext context, ITenantDbContextFactory tenantcontext, TenantProvider tenantProvider)
        {
            _context = context;
            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll;
            _tenantDbContext = tenantcontext;
            _tenantProvider = tenantProvider;
            _mapper = mapper;
        }

        #region Service Order
        public async Task<Return> NewOrder(Service_Order order)
        {
            var result = new Return();
            try
            {
                await _context.Service_Order.AddAsync(order);
                await _context.SaveChangesAsync();

                result.Result = true;
                result.Message = $"Chamado Criado com Sucesso.";
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = "Ocorreu um erro Interno";
                result.Error = ex.Message;
            }
            return result;
        }
        public async Task<Return> CatchOrder(int id, CatchOrderDTO Corder)
        {
            var result = new Return();
            try
            {
                var order = await _context.Service_Order.FirstOrDefaultAsync(e => e.ID == id);
                _mapper.Map(Corder,order);
                await _context.SaveChangesAsync();
                result.Result = true;
                result.Message = $"Chamado Pego com Sucesso.";

            }
            catch(Exception ex)
            {
                result.Result = false;
                result.Message = "Ocorreu um erro Interno";
                result.Error = ex.Message;
            }
            return result;
        }
        public async Task<Return> ContactOrder(int id)
        {
            var result = new Return();
            try
            {
                var order = await _context.Service_Order.FirstOrDefaultAsync(e => e.ID == id);
                order.Contact_Date = DateTime.Now;
                _context.Entry(order).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                result.Result = true;
                result.Message = $"Contato Cadastrado com Sucesso.";

            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = "Ocorreu um erro Interno";
                result.Error = ex.Message;
            }
            return result;
        }
        public async Task<IEnumerable<ServiceOrderDTO>> ListAllOrder() => await _context.Service_Order
            .AsNoTracking()
            .Include(e => e.RequestFK)
            .Include(e => e.TechnicalFK)
            .Include(e => e.ConcludeFK)
            .Include(e => e.TypeFK)
            .ThenInclude(e => e.CategoryFK)
            .ProjectTo<ServiceOrderDTO>(_mapper.ConfigurationProvider)
            .ToListAsync();
        public async Task<IEnumerable<ServiceOrderDTO>> ListPendingOrder() => await _context.Service_Order.Where(e => e.Status == false && e.Technical == null)
            .AsNoTracking()
            .Include(e => e.RequestFK)
            .Include(e => e.TechnicalFK)
            .Include(e => e.TypeFK)
            .ThenInclude(e => e.CategoryFK)
            .ProjectTo<ServiceOrderDTO>(_mapper.ConfigurationProvider)
            .ToListAsync();
        public async Task<IEnumerable<ServiceOrderDTO>> ListTechnicalOrder(int id) => await _context.Service_Order.Where(e => e.Technical == id && e.Status == false)
            .AsNoTracking()
            .Include(e => e.RequestFK)
            .Include(e => e.TechnicalFK)
            .Include(e => e.TypeFK)
            .ThenInclude(e => e.CategoryFK)
            .ProjectTo<ServiceOrderDTO>(_mapper.ConfigurationProvider)
            .ToListAsync();
        public async Task<Return> UpdateOrder(Service_Order order)
        {
            var result = new Return();
            try
            {
                _context.Entry(order).State = EntityState.Detached;
                _context.Service_Order.Update(order);
                await _context.SaveChangesAsync();

                result.Result = true;
                result.Message = $"Chamado Editado com Sucesso.";
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = "Ocorreu um erro Interno";
                result.Error = ex.Message;
            }
            return result;
        }
        public async Task<Return> EndOrder(int id,EndOrderDTO order)
        {
            var result = new Return();
            try
            {
                var os = await _context.Service_Order
                    .Include(e => e.ConcludeFK)
                    .FirstOrDefaultAsync(e => e.ID == id);
                _mapper.Map(order,os);
                await _context.SaveChangesAsync();

                var saida = new NF_Output()
                {
                    DateOut = DateTime.Now,
                    DateCreate = DateTime.Now,
                    Label = Convert.ToString(os.ID),
                    CreateBy = order.ChangedBy,
                    Total_Value = 0
                };

                var unit = await _context.Unit.FirstOrDefaultAsync();
                saida.Unit = unit.ID;
                await _context.NF_Output.AddAsync(saida);
                await _context.SaveChangesAsync();
                var items = await _context.Service_Items.Where(e => e.Order == os.ID).ToListAsync();

                foreach (var item in items)
                {
                    var itemsaida = new NF_Output_Items()
                    {
                        NF = saida.ID,
                        Product = item.Product,
                        Amount = item.Amount,
                        Usage_Value = 0
                    };
                    await _context.NF_Output_Items.AddAsync(itemsaida);

                    var EstoqueV = await _context.Virtual_Stock.FirstOrDefaultAsync(e => e.Product == item.Product && e.Order == os.ID);
                    _context.Virtual_Stock.Remove(EstoqueV);
                    await _context.SaveChangesAsync();
                }

                var user = await _context.Users.FirstOrDefaultAsync(e => e.ID == os.Request);

                var tenantid = _tenantProvider.GetTenantId();
                var ordem = await _context.Service_Order
                    .AsNoTracking()
                    .Include(e => e.Service_CheckListFK)
                    .ThenInclude(e => e.CheckListFK)
                    .Include(e => e.Service_ExecuteFK)
                    .ThenInclude(e => e.ServiceFK)
                    .Include(e => e.Service_ItemsFK)
                    .ThenInclude(e => e.ProductFK)
                    .ThenInclude(e => e.ProductsFK)
                    .Include(e => e.RequestFK)
                    .Include(e => e.TechnicalFK)
                    .FirstOrDefaultAsync(e => e.ID == os.ID);

                PDF.chamado(ordem, tenantid);
                var send = new Sender_Email(new SmtpClient(), new MailMessage());

                send.SendRate(user, ordem.ID, tenantid);
                send = new Sender_Email(new SmtpClient(), new MailMessage());
                send.SendReport(os, tenantid);

                result.Result = true;
                result.Message = $"Chamado encerrado com Sucesso.";
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = "Ocorreu um erro Interno";
                result.Error = ex.Message;
            }
            return result;
        }
        public async Task<Service_Order> Order(int id) => await _context.Service_Order
            .Include(e => e.TypeFK)
            .Include(e => e.ComputerFK)
            .Include(e => e.RequestFK)
            .Include(e => e.TechnicalFK)
            .Include(e => e.FileFolderFK)
            .Include(e => e.Service_ItemsFK)
            .Include(e => e.Service_CheckListFK)
            .Include(e => e.Service_ExecuteFK)
            .FirstOrDefaultAsync(e => e.ID == id);
        #endregion

        #region Service Item
        public async Task<Return> NewItemOrder(Service_Items item)
        {
            var result = new Return();
            try
            {
                await _context.Service_Items.AddAsync(item);
                await _context.SaveChangesAsync();

                var search = await _context.Virtual_Stock.FirstOrDefaultAsync(e => e.Product == item.Product && e.Order == item.Order);

                if (search != null)
                {
                    var dif = item.Amount - search.Amount;
                    search.Amount = item.Amount;
                    _context.Virtual_Stock.Update(search);

                    var stk = await _context.Stock.FirstOrDefaultAsync(e => e.Product == search.Product);
                    stk.Amount = stk.Amount - dif;
                    _context.Stock.Update(stk);
                    await _context.SaveChangesAsync();
                }
                else
                {

                    var vstock = new Virtual_Stock() { Order = item.Order, Amount = item.Amount, Product = item.Product, Measure = "Un", label = $"OS" };
                    await _context.Virtual_Stock.AddAsync(vstock);
                    await _context.SaveChangesAsync();
                    var stock = await _context.Stock.FirstOrDefaultAsync(e => e.Product == item.Product);
                    stock.Amount = stock.Amount - vstock.Amount;
                    _context.Stock.Update(stock);
                    await _context.SaveChangesAsync();
                }

                result.Result = true;
                result.Message = $"Item adicionado ao chamado.";
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = "Ocorreu um erro Interno";
                result.Error = ex.Message;
            }
            return result;
        }
        public async Task<Service_Items> ItemById(int id) => await _context.Service_Items.FirstOrDefaultAsync(e => e.ID == id);
        public async Task<List<Service_Items>> ListAllItemsByOrder(int order) => await _context.Service_Items.Where(e => e.Order == order)
            .Include(e => e.ProductFK)
            .ToListAsync();
        public async Task<Return> UpdateItem(Service_Items item)
        {
            var result = new Return();
            try
            {
                _context.Service_Items.Update(item);
                await _context.SaveChangesAsync();

                result.Result = true;
                result.Message = $"Item Atualizado.";
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = "Ocorreu um erro Interno";
                result.Error = ex.Message;
            }
            return result;
        }
        public async Task<Return> DeleteItem(int id)
        {
            var result = new Return();
            try
            {
                var tipo = await _context.Service_Items.FirstOrDefaultAsync(e => e.ID == id);
                _context.Service_Items.Remove(tipo);
                await _context.SaveChangesAsync();

                var vstock = await _context.Virtual_Stock.FirstOrDefaultAsync(e => e.Order == tipo.Order && e.Product == tipo.Product);
                _context.Virtual_Stock.Remove(vstock);

                var stock = await _context.Stock.FirstOrDefaultAsync(e => e.Product == tipo.Product);
                stock.Amount = stock.Amount + vstock.Amount;
                _context.Stock.Update(stock);
                await _context.SaveChangesAsync();

                result.Result = true;
                result.Message = $"tipo deletado.";
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = "Ocorreu um erro Interno";
                result.Error = ex.Message;
            }
            return result;
        }
        #endregion

        #region Service Type
        public async Task<Return> NewType(Service_Type type)
        {
            var result = new Return();
            try
            {
                var tenantConnections = _tenantProvider.GetAllConnectionStrings();

                foreach (var connectionString in tenantConnections)
                {
                    var con = connectionString.Value;
                    await using var context = _tenantDbContext.Create(con);

                    await context.Service_Type.AddAsync(type);
                    await context.SaveChangesAsync();

                }
                result.Result = true;
                result.Message = $"Produto de Categoria Cadastrado.";
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = "Ocorreu um erro Interno";
                result.Error = ex.Message;
            }
            return result;
        }

        public async Task<IEnumerable<TypeDTO>> ListAllType() => await _context.Service_Type
            .AsNoTracking()
            .Include(e => e.CategoryFK)
            .ProjectTo<TypeDTO>(_mapper.ConfigurationProvider)
            .ToListAsync();
        public async Task<IEnumerable<TypeDTO>> ListAllTypeByCategory(int id) => await _context.Service_Type.Where(e => e.Category == id)
            .AsNoTracking()
            .Include(e => e.CategoryFK)
            .ProjectTo<TypeDTO>(_mapper.ConfigurationProvider)
            .ToListAsync();

        public async Task<Return> UpdateType(Service_Type type)
        {
            var result = new Return();
            try
            {
                var tenantConnections = _tenantProvider.GetAllConnectionStrings();

                foreach (var connectionString in tenantConnections)
                {
                    var con = connectionString.Value;
                    await using var context = _tenantDbContext.Create(con);

                    context.Service_Type.Update(type);
                    await context.SaveChangesAsync();

                }
                result.Result = true;
                result.Message = $"Categoria de produto Atualizado.";
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = "Ocorreu um erro Interno";
                result.Error = ex.Message;
            }
            return result;
        }

        public async Task<Return> DeleteType(int typyId)
        {
            var result = new Return();
            try
            {
                var tenantConnections = _tenantProvider.GetAllConnectionStrings();

                foreach (var connectionString in tenantConnections)
                {
                    var con = connectionString.Value;
                    await using var context = _tenantDbContext.Create(con);

                    var tipo = await context.Service_Type.FirstOrDefaultAsync(e => e.ID == typyId);
                    context.Service_Type.Remove(tipo);
                    await context.SaveChangesAsync();
                }
                result.Result = true;
                result.Message = $"Categoria de produto Deletado.";
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = "Ocorreu um erro Interno";
                result.Error = ex.Message;
            }
            return result;
        }

        public async Task<Service_Type> TypeById(int typyId) => await _context.Service_Type
            .Include(e => e.CategoryFK)
            .FirstOrDefaultAsync(e => e.ID == typyId);
        #endregion

        #region Service Category
        public async Task<Return> NewCategory(Service_Category category)
        {
            var result = new Return();
            try
            {
                var tenantConnections = _tenantProvider.GetAllConnectionStrings();

                foreach (var connectionString in tenantConnections)
                {
                    var con = connectionString.Value;
                    await using var context = _tenantDbContext.Create(con);

                    await context.Service_Category.AddAsync(category);
                    await context.SaveChangesAsync();

                }
                result.Result = true;
                result.Message = $"Produto de Categoria Cadastrado.";
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = "Ocorreu um erro Interno";
                result.Error = ex.Message;
            }
            return result;
        }

        public async Task<List<Service_Category>> ListAllCategory() => await _context.Service_Category.ToListAsync();
        public async Task<Return> UpdateCategory(Service_Category category)
        {
            var result = new Return();
            try
            {
                var tenantConnections = _tenantProvider.GetAllConnectionStrings();

                foreach (var connectionString in tenantConnections)
                {
                    var con = connectionString.Value;
                    await using var context = _tenantDbContext.Create(con);

                    context.Service_Category.Update(category);
                    await context.SaveChangesAsync();

                }
                result.Result = true;
                result.Message = $"Categoria de produto Atualizado.";
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = "Ocorreu um erro Interno";
                result.Error = ex.Message;
            }
            return result;
        }

        public async Task<Return> DeleteCategory(int categoryId)
        {
            var result = new Return();
            try
            {
                var tenantConnections = _tenantProvider.GetAllConnectionStrings();

                foreach (var connectionString in tenantConnections)
                {
                    var con = connectionString.Value;
                    await using var context = _tenantDbContext.Create(con);

                    var tipo = await context.Service_Category.FirstOrDefaultAsync(e => e.ID == categoryId);
                    context.Service_Category.Remove(tipo);
                    await context.SaveChangesAsync();
                }
                result.Result = true;
                result.Message = $"Categoria de produto Deletado.";
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = "Ocorreu um erro Interno";
                result.Error = ex.Message;
            }
            return result;
        }

        public async Task<Service_Category> CategoryById(int categoryId) => await _context.Service_Category.FirstOrDefaultAsync(e => e.ID == categoryId);
        #endregion

        #region Service Execute
        public async Task<Return> NewExecute(Service_Execute execute)
        {
            var result = new Return();
            try
            {
                await _context.Service_Execute.AddAsync(execute);
                await _context.SaveChangesAsync();

                result.Result = true;
                result.Message = $"serviço adicionado com Sucesso.";
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = "Ocorreu um erro Interno";
                result.Error = ex.Message;
            }
            return result;
        }
        public async Task<List<Service_Execute>> ListAllExecute() => await _context.Service_Execute.ToListAsync();
        public async Task<List<Service_Execute>> ListAllServiceByOrder(int order) => await _context.Service_Execute.Where(e => e.Order == order)
            .Include(e => e.ServiceFK)
            .ToListAsync();
        public async Task<Return> UpdateExecute(Service_Execute execute)
        {
            var result = new Return();
            try
            {
                _context.Service_Execute.Update(execute);
                await _context.SaveChangesAsync();

                result.Result = true;
                result.Message = $"serviço Criado com Sucesso.";
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = "Ocorreu um erro Interno";
                result.Error = ex.Message;
            }
            return result;
        }
        public async Task<Return> DeleteExecute(int executeId)
        {
            var result = new Return();
            try
            {
                var tipo = await _context.Service_Execute.FirstOrDefaultAsync(e => e.ID == executeId);
                _context.Service_Execute.Remove(tipo);
                await _context.SaveChangesAsync();

                result.Result = true;
                result.Message = $"categoria deletado.";
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = "Ocorreu um erro Interno";
                result.Error = ex.Message;
            }
            return result;
        }
        public async Task<Service_Execute> ExecuteById(int executeId) => await _context.Service_Execute.FirstOrDefaultAsync(e => e.ID == executeId);
        #endregion

        #region Services
        public async Task<Return> NewService(Models.DB.Services service)
        {
            var result = new Return();
            try
            {
                var tenantConnections = _tenantProvider.GetAllConnectionStrings();

                foreach (var connectionString in tenantConnections)
                {
                    var con = connectionString.Value;
                    await using var context = _tenantDbContext.Create(con);

                    await context.Services.AddAsync(service);
                    await context.SaveChangesAsync();

                }
                result.Result = true;
                result.Message = $"Serviço Cadastrado.";
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = "Ocorreu um erro Interno";
                result.Error = ex.Message + ex.InnerException;
            }
            return result;

        }

        public async Task<List<Models.DB.Services>> ListAllService() => await _context.Services.ToListAsync();

        public async Task<Return> UpdateService(Models.DB.Services service)
        {
            var result = new Return();
            try
            {
                var tenantConnections = _tenantProvider.GetAllConnectionStrings();

                foreach (var connectionString in tenantConnections)
                {
                    var con = connectionString.Value;
                    await using var context = _tenantDbContext.Create(con);

                    context.Services.Update(service);
                    await context.SaveChangesAsync();

                }
                result.Result = true;
                result.Message = $"Produto de Categoria Cadastrado.";
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = "Ocorreu um erro Interno";
                result.Error = ex.Message;
            }
            return result;
        }

        public async Task<Return> DeleteService(int serviceId)
        {
            var result = new Return();
            try
            {
                var tenantConnections = _tenantProvider.GetAllConnectionStrings();

                foreach (var connectionString in tenantConnections)
                {
                    var con = connectionString.Value;
                    await using var context = _tenantDbContext.Create(con);

                    var tipo = await _context.Services.FirstOrDefaultAsync(e => e.ID == serviceId);
                    context.Services.Remove(tipo);
                    await context.SaveChangesAsync();

                }
                result.Result = true;
                result.Message = $"Produto de Categoria Cadastrado.";
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = "Ocorreu um erro Interno";
                result.Error = ex.Message;
            }
            return result;
        }

        public async Task<Models.DB.Services> ServiceById(int serviceId) => await _context.Services.FirstOrDefaultAsync(e => e.ID == serviceId);
        #endregion

        #region Service Checklist
        public async Task<Return> NewChecklist(Service_CheckList checkList)
        {
            var result = new Return();
            try
            {
                await _context.Service_CheckList.AddAsync(checkList);
                await _context.SaveChangesAsync();

                result.Result = true;
                result.Message = $"serviço Criado com Sucesso.";
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = "Ocorreu um erro Interno";
                result.Error = ex.Message;
            }
            return result;
        }

        public async Task<List<Service_CheckList>> ListAllChecklist() => await _context.Service_CheckList.ToListAsync();

        public async Task<List<Service_CheckList>> ListAllCheckByOrder(int order) => await _context.Service_CheckList.Where(e => e.Order == order)
            .Include(e => e.CheckListFK)
            .ToListAsync();

        public async Task<Return> UpdateChecklist(Service_CheckList checkList)
        {
            var result = new Return();
            try
            {
                _context.Service_CheckList.Update(checkList);
                await _context.SaveChangesAsync();

                result.Result = true;
                result.Message = $"CheckList Atualizado com Sucesso.";
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = "Ocorreu um erro Interno";
                result.Error = ex.Message;
            }
            return result;
        }

        public async Task<Return> DeleteChecklist(int checklistId)
        {
            var result = new Return();
            try
            {
                var tipo = await _context.Service_CheckList.FirstOrDefaultAsync(e => e.ID == checklistId);
                _context.Service_CheckList.Remove(tipo);
                await _context.SaveChangesAsync();

                result.Result = true;
                result.Message = $"categoria deletado.";
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = "Ocorreu um erro Interno";
                result.Error = ex.Message;
            }
            return result;
        }

        public async Task<Service_CheckList> ChecklistById(int checklistId) => await _context.Service_CheckList.FirstOrDefaultAsync(e => e.ID == checklistId);

        #endregion

        #region Service Rate
        public async Task<bool> NewRate(Service_Rate rate)
        {
            try
            {
                await _context.Service_Rate.AddAsync(rate);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<List<Service_Rate>> ListAllRate() => await _context.Service_Rate.ToListAsync();

        public Task<List<Service_Rate>> ListByTechnical(int id) => _context.Service_Rate.Where(e => e.OrderFK.Technical == id).ToListAsync();
        #endregion
    }
}
