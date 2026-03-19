using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domus.DataBase;
using Domus.DTO.Passwords;
using Domus.Models;
using Domus.Models.DB;
using Domus.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Domus.Repositories
{
    public class PasswordRepository : IPassword
    {

        #region Property
        public ApplicationDbContext db { get; set; }
        private readonly IMapper _mapper;
        public PasswordRepository(ApplicationDbContext context, IMapper mapper)
        {
            db = context;
            _mapper = mapper;
        }

        #endregion

        #region Passoword
        public async Task<Passwords> Password_SearchBy(int id) => await db.Passwords.FirstOrDefaultAsync(e => e.ID == id);
        public async Task<IEnumerable<PasswordDTO>> PasswordsList() => await db.Passwords
                .Include(e => e.TypeFK)
                .Include(e => e.OwnerFK)
                .ProjectTo<PasswordDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();

        public async Task<Return> NewPassoword(PasswordDTO New)
        {

            var result = new Return();

            try
            {
                var newpass = _mapper.Map<Passwords>(New);
                await db.Passwords.AddAsync(newpass);
                await db.SaveChangesAsync();
                result.Result = true;
                result.Message = "Senha Cadastrada!";

            }
            catch (Exception ex)
            {

                result.Result = false;
                result.Message = "Ocorreu um Erro Interno!";
                result.Error = ex.Message;
            }
            return result;
        }

        public async Task<Return> UpdatePassoword(Passwords Update)
        {

            var result = new Return();

            try
            {

                db.Passwords.Update(Update);
                await db.SaveChangesAsync();
                result.Result = true;
                result.Message = "Senha Atualizada!";

            }
            catch (Exception ex)
            {

                result.Result = false;
                result.Message = "Ocorreu um Erro Interno!";
                result.Error = ex.Message;
            }
            return result;
        }

        public async Task<Return> DeletePassoword(int Delete)
        {

            var result = new Return();

            try
            {

                var search = await db.Passwords.FirstOrDefaultAsync(e => e.ID == Delete);
                db.Passwords.Remove(search);
                await db.SaveChangesAsync();
                result.Result = true;
                result.Message = "Senha Deletada!";
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

        #region Type
        public async Task<Type_Passwords> Type_SearchBy(int id) => await db.Type_Passwords.FirstOrDefaultAsync(e => e.ID == id);
        public async Task<List<Type_Passwords>> TypeList() => await db.Type_Passwords.ToListAsync();
        public async Task<Return> NewType(Type_Passwords New)
        {

            var result = new Return();

            try
            {

                await db.Type_Passwords.AddAsync(New);
                await db.SaveChangesAsync();
                result.Result = true;
                result.Message = "Senha Cadastrada!";

            }
            catch (Exception ex)
            {

                result.Result = false;
                result.Message = "Ocorreu um Erro Interno!";
                result.Error = ex.Message;
            }
            return result;
        }

        public async Task<Return> UpdateType(Type_Passwords Update)
        {

            var result = new Return();

            try
            {

                db.Type_Passwords.Update(Update);
                await db.SaveChangesAsync();
                result.Result = true;
                result.Message = "Senha Atualizada!";

            }
            catch (Exception ex)
            {

                result.Result = false;
                result.Message = "Ocorreu um Erro Interno!";
                result.Error = ex.Message;
            }
            return result;
        }
        public async Task<Return> DeleteType(int Delete)
        {

            var result = new Return();

            try
            {

                var search = await db.Type_Passwords.FirstOrDefaultAsync(e => e.ID == Delete);
                db.Type_Passwords.Remove(search);
                await db.SaveChangesAsync();
                result.Result = true;
                result.Message = "Senha Deletada!";
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
