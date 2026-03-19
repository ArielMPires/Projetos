using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domus.DataBase;
using Domus.DTO.Computer;
using Domus.DTO.Patrimony;
using Domus.Models;
using Domus.Models.DB;
using Domus.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Domus.Repositories
{
    public class PatrimonyRepository : IPatrimony
    {

        #region Property
        public readonly ApplicationDbContext db;
        public readonly IMapper _mapper;

        public PatrimonyRepository(ApplicationDbContext context, IMapper mapper)
        {
            db = context;
            _mapper = mapper;
        }
        #endregion

        #region Patrimony
        public async Task<Patrimony> Patrimony_SearchBy(int id) => await db.Patrimony
            .Include(e => e.Current_OwnerFK)
            .Include(e => e.DepartmentFK)
            .Include(e => e.CategoryFK)
            .Include(e => e.ComputerFK)
            .FirstOrDefaultAsync(e => e.ID == id);
        public async Task<IEnumerable<PatrimonyDTO>> PatrimonyList() => await db.Patrimony
            .AsNoTracking()
            .Include(e => e.Current_OwnerFK)
            .Include(e => e.CategoryFK)
            .Include(e => e.DepartmentFK)
            .ProjectTo<PatrimonyDTO>(_mapper.ConfigurationProvider)
            .ToListAsync();
        public async Task<IEnumerable<PatrimonyDTO>> PCRegister() => await db.Patrimony.Where(e => e.CategoryFK.Name == "PC" && e.ComputerFK == null || e.CategoryFK.Name == "IMPRESSORA" && e.ComputerFK == null)
            .AsNoTracking()
            .Include(e => e.Current_OwnerFK)
            .Include(e => e.CategoryFK)
            .Include(e => e.DepartmentFK)
            .ProjectTo<PatrimonyDTO>(_mapper.ConfigurationProvider)
            .ToListAsync();

        public async Task<Return> NewPatrimony(Patrimony New)
        {

            var result = new Return();

            try
            {
                await db.Patrimony.AddAsync(New);
                await db.SaveChangesAsync();
                result.Result = true;
                result.Message = "Patrimonio Cadastrado!";

            }
            catch (Exception ex)
            {

                result.Result = false;
                result.Message = "Ocorreu um Erro Interno!";
                result.Error = ex.InnerException.Message;
            }

            return result;
        }
        public async Task<Return> UpdatePatrimony(Patrimony Update)
        {

            var result = new Return();

            try
            {
                db.Patrimony.Update(Update);
                await db.SaveChangesAsync();
                result.Result = true;
                result.Message = "Patrimonio Atualizado!";
            }
            catch (Exception ex)
            {

                result.Result = false;
                result.Message = "Ocorreu um Erro Interno!";
                result.Error = ex.Message;
            }

            return result;
        }
        public async Task<Return> DeletePatrimony(int Delete)
        {

            var result = new Return();

            try
            {

                var search = await db.Patrimony_Category.FirstOrDefaultAsync(e => e.ID == Delete);
                db.Patrimony_Category.Remove(search);
                await db.SaveChangesAsync();
                result.Result = true;
                result.Message = "Patrimonio Deletado!";

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

        #region Category
        public async Task<Patrimony_Category> Category_SearchBy(int id) => await db.Patrimony_Category.FirstOrDefaultAsync(e => e.ID == id);
        public async Task<List<Patrimony_Category>> CategoryList() => await db.Patrimony_Category.ToListAsync();
        public async Task<Return> NewCategory(Patrimony_Category New)
        {

            var result = new Return();

            try
            {
                await db.Patrimony_Category.AddAsync(New);
                await db.SaveChangesAsync();
                result.Result = true;
                result.Message = "Patrimonio de Categoria Cadastrado!";
            }
            catch (Exception ex)
            {

                result.Result = false;
                result.Message = "Ocorreu um Erro Interno!";
                result.Error = ex.InnerException.Message;

            }

            return result;
        }

        public async Task<Return> DeleteCategory(int Delete)
        {

            var result = new Return();

            try
            {

                var search = await db.Patrimony_Category.FirstOrDefaultAsync(e => e.ID == Delete);
                db.Patrimony_Category.Remove(search);
                await db.SaveChangesAsync();
                result.Result = true;
                result.Message = "Patrimony de Categoria Deletada";
            }
            catch (Exception ex)
            {

                result.Result = false;
                result.Message = "Ocorreu um Erro Interno!";
                result.Error = ex.Message;
            }
            return result;
        }

        public async Task<Return> UpdateCategory(Patrimony_Category Update)
        {

            var result = new Return();

            try
            {

                db.Patrimony_Category.Update(Update);
                await db.SaveChangesAsync();
                result.Result = true;
                result.Message = "Patrimonio de Categoria Atualizado!";
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

        #region Computer 
        public async Task<Computer> Computer_SearchByPc(int id) => await db.Computer.FirstOrDefaultAsync(e => e.ID == id);
        public async Task<List<Computer>> ComputerList() => await db.Computer
            .AsNoTracking()
            .ToListAsync();
        public async Task<IEnumerable<PCDTO>> ComputerListByOwner(int owner) => await db.Computer.Where(e => e.PatrimonyFK.Current_Owner == owner)   
            .AsNoTracking()
            .Include(e => e.PatrimonyFK)
            .ProjectTo<PCDTO>(_mapper.ConfigurationProvider)
            .ToListAsync();

        public async Task<Return> NewComputer(Computer New)
        {

            var result = new Return();

            try
            {

                await db.Computer.AddAsync(New);
                await db.SaveChangesAsync();
                result.Result = true;
                result.Message = "Computador Cadastrado!";
            }
            catch (Exception ex)
            {

                result.Result = false;
                result.Message = "Ocorreu um Erro Interno!";
                result.Error = ex.Message;
            }
            return result;
        }
        public async Task<Return> UpdateComputer(Computer Update)
        {

            var result = new Return();

            try
            {

                db.Computer.Update(Update);
                await db.SaveChangesAsync();
                result.Result = true;
                result.Message = "Computador Atualizado!";
            }
            catch (Exception ex)
            {

                result.Result = false;
                result.Message = "Ocorreu um Erro Interno!";
                result.Error = ex.Message;

            }
            return result;
        }

        public async Task<Return> DeleteComputer(int Delete)
        {

            var result = new Return();

            try
            {
                var search = await db.Computer.FirstOrDefaultAsync(e => e.ID == Delete);
                db.Computer.Remove(search);
                await db.SaveChangesAsync();
                result.Result = true;
                result.Message = "Computador Deletado!";

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
