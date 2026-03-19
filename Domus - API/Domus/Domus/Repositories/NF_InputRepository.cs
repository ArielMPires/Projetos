using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domus.DataBase;
using Domus.DTO.NF;
using Domus.Models;
using Domus.Models.DB;
using Domus.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace Domus.Repositories {
    public class NF_InputRepository : INF_Input {


        #region Property
        public ApplicationDbContext db { get; set; }
        private readonly IMapper _mapper;

        public NF_InputRepository(ApplicationDbContext context, IMapper mapper)
        {
            db = context;
            _mapper = mapper;
        }
        #endregion

        #region NF_Input
        public async Task<NF_Input> Input_SearchByNf(int id) => await db.NF_Input.FirstOrDefaultAsync(e => e.ID == id);

        public async Task<IEnumerable<InputDTO>> InputList() => await db.NF_Input
            .AsNoTracking()
            .Include(e => e.SupplierFK)
            .ProjectTo<InputDTO>(_mapper.ConfigurationProvider)
            .ToListAsync();

        public async Task<Return> InputNew(NF_Input New) {

            var result = new Return();

            try {
                var items = New.NF_Input_ItemsFK;
                New.NF_Input_ItemsFK = null;
                await db.NF_Input.AddAsync(New);
                await db.SaveChangesAsync();
                
                foreach (var item in items) {
                    item.ProductFK = null;
                    item.NF = New.ID;
                    await db.NF_Input_Items.AddAsync(item);
                    var estoque = await db.Stock.FirstOrDefaultAsync(e => e.Product == item.Product);
                    estoque.Amount = estoque.Amount + item.Amount;
                    db.Stock.Update(estoque);
                    await db.SaveChangesAsync();

                }

                result.Result = true;
                result.Message = "NF Entrada Cadastrada!";


            } catch (Exception ex) {

                result.Result = false;
                result.Message = "Ocorreu um Erro Interno!";
                result.Error = ex.Message;
            }
            return result;
        }


        #endregion

        #region NF_Output
        public async Task<NF_Output> OutputSearchByNf(int id) => await db.NF_Output.FirstOrDefaultAsync(e => e.ID == id);
        public async Task<IEnumerable<OutputDTO>> OutputList() =>await  db.NF_Output
            .AsNoTracking()
            .Include(e => e.UnitFK)
            .ProjectTo<OutputDTO>(_mapper.ConfigurationProvider)
            .ToListAsync();
        

        public async Task<Return> OutputNew(NF_Output New) {
            
            var result = new Return();

            try {

                var saida = New.NF_Output_ItemsFK;
                New.NF_Output_ItemsFK = null;
                var unit = await db.Unit.FirstOrDefaultAsync();
                New.Unit = unit.ID;
                await db.NF_Output.AddAsync(New);
                await db.SaveChangesAsync();

                foreach (var item in saida) { 
                    item.NF = New.ID;
                    await db.NF_Output_Items.AddAsync(item);
                    if(String.IsNullOrEmpty(New.Label))
                    {
                        var estoque = await db.Stock.FirstOrDefaultAsync(e => e.Product == item.Product);
                        estoque.Amount = estoque.Amount - item.Amount;
                        db.Stock.Update(estoque);
                    await db.SaveChangesAsync();
                    }
                    else
                    {
                    var EstoqueV = await db.Virtual_Stock.FirstOrDefaultAsync(e => e.Product == item.Product && e.Order == Convert.ToInt32(New.Label));
                    db.Virtual_Stock.Remove(EstoqueV);
                    await db.SaveChangesAsync();
                    }
                }

                result.Result = true;
                result.Message = "NF Saída Cadastrada!";

            } catch (Exception ex) {

                result.Result = false;
                result.Message = "Ocorreu um Erro Interno!";
                result.Error = ex.Message + ex.InnerException.Message;
            }

        return result;
        }


        #endregion

    }
}
