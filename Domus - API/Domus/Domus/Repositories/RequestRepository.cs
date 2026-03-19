using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domus.DataBase;
using Domus.DTO.Purchase_Order;
using Domus.DTO.Request;
using Domus.Models;
using Domus.Models.DB;
using Domus.Repositories.Interfaces;
using Domus.Services;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;

namespace Domus.Repositories
{
    public class RequestRepository : IRequest
    {

        #region Property
        public ApplicationDbContext db { get; set; }
        private readonly TenantProvider _tenantProvider;
        private readonly IMapper _mapper;
        public RequestRepository(ApplicationDbContext context, TenantProvider tenantProvider, IMapper mapper)
        {
            db = context;
            _tenantProvider = tenantProvider;
            _mapper = mapper;
        }
        #endregion

        #region Request
        public async Task<Request> Request_SearchByPc(int id) => await db.Request
            .Include(e => e.ItemsFK)
            .Include(e => e.RequesterFK)
            .Include(e => e.DepartmentFK)
            .Include(e => e.UseFK)
            .FirstOrDefaultAsync(e => e.ID == id);
        public async Task<IEnumerable<RequestDTO>> RequestList() => await db.Request
            .AsNoTracking()
            .Include(e => e.AuthorizationFK)
            .Include(e => e.RequesterFK)
            .Include(e => e.DepartmentFK)
            .Include(e => e.UseFK)
            .ProjectTo<RequestDTO>(_mapper.ConfigurationProvider)
            .ToListAsync();
        public async Task<IEnumerable<RequestDTO>> RequestListPending() => await db.Request.Where(e => e.AuthorizationFK == null)
            .AsNoTracking()
            .Include(e => e.AuthorizationFK)
            .Include(e => e.RequesterFK)
            .Include(e => e.DepartmentFK)
            .Include(e => e.UseFK)
            .ProjectTo<RequestDTO>(_mapper.ConfigurationProvider)
            .ToListAsync();
        public async Task<Return> NewRequest(Request New)
        {

            var result = new Return();

            try
            {
                var items = New.ItemsFK;
                New.ItemsFK = null;
                await db.Request.AddAsync(New);
                await db.SaveChangesAsync();
                foreach (var item in items)
                {
                    item.ProductFK = null;
                    item.Request = New.ID;
                    await db.Request_Items.AddAsync(item);
                }
                await db.SaveChangesAsync();
                result.Result = true;
                result.Message = "Nova Solicitação Cadastrada!";
            }
            catch (Exception ex)
            {

                result.Result = false;
                result.Message = "Ocorreu um Erro Interno!";
                result.Error = ex.Message;
            }
            return result;
        }

        public async Task<Return> DeleteRequest(int Delete)
        {

            var result = new Return();

            try
            {

                var search = await db.Request.FirstOrDefaultAsync(e => e.ID == Delete);
                db.Request.Remove(search);
                await db.SaveChangesAsync();
                result.Result = true;
                result.Message = "Solicitação Excluída";

            }
            catch (Exception ex)
            {

                result.Result = false;
                result.Message = "Ocorreu um Erro Interno!";
                result.Error = ex.Message;
            }
            return result;
        }

        public async Task<Return> UpdateRequest(Request Update)
        {

            var result = new Return();

            try
            {
                db.Request.Update(Update);
                await db.SaveChangesAsync();
                result.Result = true;
                result.Message = "Solicitação Atualizada!";


            }
            catch (Exception ex)
            {

                result.Result = false;
                result.Message = "Ocorreu um Erro Interno!";
                result.Error = ex.Message;
            }
            return result;
        }
        #endregion

        #region Approval
        public async Task<Request_Approval> Approval_SearchByPc(int id) => await db.Request_Approval.FirstOrDefaultAsync(e => e.ID == id);
        public Task<List<Request_Approval>> AprrovalList() => db.Request_Approval.ToListAsync();

        public async Task<Return> NewAprroval(Request New)
        {

            var result = new Return();

            try
            {

                var approval = New.AuthorizationFK;
                if (approval.Situation == true)
                {
                    var order = New.RequestFK;
                    await db.Request_Approval.AddAsync(approval);
                    await db.SaveChangesAsync();

                    await db.Purchase_Order.AddAsync(order);
                    await db.SaveChangesAsync();

                    var os = await db.Purchase_Order
                        .AsNoTracking()
                        .Include(e => e.SupplierFK)
                        .FirstOrDefaultAsync(e => e.ID == order.ID);

                    os.RequestFK = await db.Request
                        .AsNoTracking()
                        .Include(e => e.RequesterFK)
                        .Include(e => e.UseFK)
                        .Include(e => e.ItemsFK)
                        .Include(e => e.DepartmentFK)
                        .FirstOrDefaultAsync(e => e.ID == os.Request);

                    foreach (var item in os.RequestFK.ItemsFK)
                    {
                        item.ProductFK = await db.Products.AsNoTracking().FirstOrDefaultAsync(e => e.ID == item.Product);
                    }
                    var tenantid = _tenantProvider.GetTenantId();

                    PDF.pedidocompra(os, tenantid);
                    var send = new Sender_Email(new SmtpClient(), new MailMessage());

                    send.SendPurchase(os, tenantid);
                    result.Message = "Solicitação Aprovada com Sucesso!";
                }
                else
                {
                    await db.Request_Approval.AddAsync(approval);
                    await db.SaveChangesAsync();
                    result.Message = "Solicitação Rejeitada com Sucesso!";
                }
                New.AuthorizationFK = null;
                New.RequestFK = null;
                New.Authorization = approval.ID;
                db.Request.Update(New);
                await db.SaveChangesAsync();
                result.Result = true;


            }
            catch (Exception ex)
            {

                result.Result = false;
                result.Message = "Ocorreu um Erro Interno!";
                result.Error = ex.Message;
            }
            return result;
        }

        public async Task<Return> DeleteAprroval(int Delete)
        {

            var result = new Return();
            try
            {

                var search = await db.Request_Approval.FirstOrDefaultAsync(e => e.ID == Delete);
                db.Request_Approval.Remove(search);
                await db.SaveChangesAsync();
                result.Result = true;
                result.Message = "Solicitação Aprovada Deletada!";

            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = "Ocorreu um Erro Interno!";
                result.Error = ex.Message;
            }
            return result;
        }
        public async Task<Return> UpdateAprroval(Request_Approval Update)
        {

            var result = new Return();
            try
            {

                db.Request_Approval.Update(Update);
                await db.SaveChangesAsync();
                result.Result = true;
                result.Message = "Solicitação Aprovada Deletada!";

            }
            catch (Exception ex)
            {

                result.Result = false;
                result.Message = "Ocorreu um Erro Interno!";
                result.Error = ex.Message;
            }
            return result;
        }
        #endregion

        #region Items
        public async Task<Request_Items> Items_SearchByPc(int id) => await db.Request_Items.FirstOrDefaultAsync(e => e.ID == id);
        public Task<List<Request_Items>> ItemsList() => db.Request_Items
            .Include(e => e.ProductFK)
            .ToListAsync();
        public async Task<Return> New_Items(Request_Items New)
        {

            var result = new Return();
            try
            {

                await db.Request_Items.AddAsync(New);
                await db.SaveChangesAsync();
                result.Result = true;
                result.Message = "Solicitação dos Itens Cadastrado!";

            }
            catch (Exception ex)
            {

                result.Result = false;
                result.Message = "Ocorreu um Erro Interno!";
                result.Error = ex.Message;
            }

            return result;
        }
        public async Task<Return> DeleteItems(int Delete)
        {

            var result = new Return();
            try
            {
                var search = db.Request_Items.FirstOrDefault(e => e.ID == Delete);
                db.Request_Items.Remove(search);
                await db.SaveChangesAsync();
                result.Result = true;
                result.Message = "Solicitação de Itens Deletado!";

            }
            catch (Exception ex)
            {

                result.Result = false;
                result.Message = "Ocorreu um Erro Interno!";
                result.Error = ex.Message;
            }
            return result;
        }
        public async Task<Return> UpdateItems(Request_Items Update)
        {

            var result = new Return();
            try
            {

                db.Request_Items.Update(Update);
                await db.SaveChangesAsync();
                result.Result = true;
                result.Message = "Items Atualizado!";
            }
            catch (Exception ex)
            {

                result.Result = false;
                result.Message = "Ocorreu um Erro Interno!";
                result.Error = ex.Message;
            }
            return result;
        }

        #endregion

        #region Usage
        public async Task<Request_Usage> Usage_SearchByPc(int id) => await db.Request_Usage.FirstOrDefaultAsync(e => e.ID == id);
        public Task<List<Request_Usage>> UsageList() => db.Request_Usage.ToListAsync();
        public async Task<Return> New_Usage(Request_Usage New)
        {

            var result = new Return();
            try
            {

                await db.Request_Usage.AddAsync(New);
                await db.SaveChangesAsync();
                result.Result = true;
                result.Message = "Solicitação de Uso Cadastrada!";
            }
            catch (Exception ex)
            {

                result.Result = false;
                result.Message = "Ocorreu um Erro Interno!";
                result.Error = ex.Message;
            }
            return result;
        }
        public async Task<Return> UpdateUsage(Request_Usage Update)
        {

            var result = new Return();

            try
            {

                db.Request_Usage.Update(Update);
                await db.SaveChangesAsync();
                result.Result = true;
                result.Message = "Solicitação de Uso Atualizada";
            }
            catch (Exception ex)
            {

                result.Result = false;
                result.Message = "Ocorreu um Erro Interno!";
                result.Error = ex.Message;
            }
            return result;
        }
        public async Task<Return> DeleteUsage(int Delete)
        {

            var result = new Return();

            try
            {
                var search = db.Request_Items.FirstOrDefault(e => e.ID == Delete);
                db.Request_Items.Remove(search);
                await db.SaveChangesAsync();
                result.Result = true;
                result.Message = "Solicitação de Uso Deletada!";
            }
            catch (Exception ex)
            {

                result.Result = false;
                result.Message = "Ocorreu um Erro Interno!";
                result.Error = ex.Message;
            }
            return result;
        }
        #endregion

        #region Order
        public Task<Purchase_Order> Purchase_SearchByPc(int id) => db.Purchase_Order
            .Include(e => e.RequestFK)
            .ThenInclude(e => e.ItemsFK)
            .Include(e => e.SupplierFK)
            .FirstOrDefaultAsync(e => e.ID == id);
        public async Task<IEnumerable<Purchase_OrderDTO>> PurchaseList() => await db.Purchase_Order
            .AsNoTracking()
            .Include(e => e.SupplierFK)
            .Include(e => e.RequestFK)
            .ThenInclude(e => e.RequesterFK)
            .ProjectTo<Purchase_OrderDTO>(_mapper.ConfigurationProvider)
            .ToListAsync();
        public async Task<IEnumerable<Purchase_OrderDTO>> PurchaseListDelivered() => await db.Purchase_Order.Where(e => e.Delivered == false)
            .AsNoTracking()
            .Include(e => e.SupplierFK)
            .Include(e => e.RequestFK)
            .ThenInclude(e => e.RequesterFK)
            .ProjectTo<Purchase_OrderDTO>(_mapper.ConfigurationProvider)
            .ToListAsync();
        public async Task<Return> DeleteOrder(int Delete)
        {

            var result = new Return();

            try
            {
                var search = db.Request_Items.FirstOrDefault(e => e.ID == Delete);
                db.Request_Items.Remove(search);
                await db.SaveChangesAsync();
                result.Result = true;
                result.Message = "Compra Deletada!";
            }
            catch (Exception ex)
            {

                result.Result = false;
                result.Message = "Ocorreu um Erro Interno!";
                result.Error = ex.Message;
            }
            return result;
        }

        public async Task<Return> New_Order(Purchase_Order New)
        {

            var result = new Return();
            try
            {

                await db.Purchase_Order.AddAsync(New);
                await db.SaveChangesAsync();
                result.Result = true;
                result.Message = "Compra Cadastrada!";


            }
            catch (Exception ex)
            {

                result.Result = false;
                result.Message = "Ocorreu um Erro Interno!";
                result.Error = ex.Message;

            }
            return result;
        }

        public async Task<Return> UpdateOrder(Purchase_Order Update)
        {

            var result = new Return();
            try
            {
                db.Purchase_Order.Update(Update);
                await db.SaveChangesAsync();
                result.Result = true;
                result.Message = "Compra Atualizada! ";
            }
            catch (Exception ex)
            {

                result.Result = false;
                result.Message = "Ocorreu um Erro Interno!";
                result.Error = ex.Message;
            }
            return result;
        }
        #endregion
    }
}
