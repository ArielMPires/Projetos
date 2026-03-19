using Domus.DataBase;
using Domus.Models;
using Domus.Models.DB;
using Domus.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Domus.Repositories {
    public class Provider_OrderRepository : IProvider_Order {

        #region Property
        public ApplicationDbContext db { get; set; }
        public Provider_OrderRepository(ApplicationDbContext context) => db = context;

        #endregion

        #region ProviderOrder
        public async Task<Provider_Order> Provider_SearchById(int id) => await db.Provider_Order.FirstOrDefaultAsync(e => e.ID == id);
        public async Task<List<Provider_Order>> ListProvider() => await db.Provider_Order.ToListAsync();

        public async Task<Return> NewProvider(Provider_Order New) {
            
            var result = new Return();

            try {

                await db.Provider_Order.AddAsync(New);
                await db.SaveChangesAsync();
                result.Result = true;
                result.Message = "Pedido do Provedor Cadastrado!";

            } catch (Exception ex) {

                result.Result = false;
                result.Message = "Ocorreu um Erro Interno!";
                result.Error = ex.Message;
            }
            return result;
        }
        public async Task<Return> UpdateProvider(Provider_Order Update) {

            var result = new Return();

            try {

                db.Provider_Order.Update(Update);
                await db.SaveChangesAsync();
                result.Result = true;
                result.Message = "Pedido Atualizada!";

            } catch (Exception ex) {

                result.Result = false;
                result.Message = "Ocorreu um Erro Interno!";
                result.Error = ex.Message;
            }
            return result;
        }

        public async Task<Return> DeleteProvider(int Delete) {

            var result = new Return();

            try {

                var search = await db.Provider_Order.FirstOrDefaultAsync(e => e.ID == Delete);
                db.Provider_Order.Remove(search);
                await db.SaveChangesAsync();
                result.Result = true;
                result.Message = "Pedido Deletado!";

            } catch (Exception ex) {

                result.Result = false;
                result.Message = "Ocorreu um Erro Interno!";
                result.Error = ex.Message;
            }
            return result;
        }

        #endregion

        #region ServiceProvider
        public async Task<Service_Providers> Service_SearchById(int id) => await db.Service_Providers.FirstOrDefaultAsync(e => e.ID == id);
        public async Task<List<Service_Providers>> ListServices() => await db.Service_Providers.ToListAsync();

        public async Task<Return> NewService(Service_Providers New) {
            
            var result = new Return();

            try {

                await db.Service_Providers.AddAsync(New);
                await db.SaveChangesAsync();
                result.Result = true;
                result.Message = "Provedores de Serviços Cadastrado!";

            } catch (Exception ex) {

                result.Result = false;
                result.Message = "Ocorreu um Erro Interno!";
                result.Error = ex.Message;
            }
            return result;
        }
        public async Task<Return> UpdateService(Service_Providers Update) {

            var result = new Return();

            try {

                db.Service_Providers.Update(Update);
                await db.SaveChangesAsync();
                result.Result = true;
                result.Message = "Provedores Atualizada!";

            } catch (Exception ex) {

                result.Result = false;
                result.Message = "Ocorreu um Erro Interno!";
                result.Error = ex.Message;
            }
            return result;
        }
        public async Task<Return> DeleteService(int Delete) {

            var result = new Return();

            try {

                var search = await db.Service_Providers.FirstOrDefaultAsync(e => e.ID == Delete);
                db.Service_Providers.Remove(search);
                await db.SaveChangesAsync();
                result.Result = true;
                result.Message = "Provedores Deletado!";

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
