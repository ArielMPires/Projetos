using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using NexuSys.Data;
using NexuSys.DTOs.Budget;
using NexuSys.DTOs.Possible_Defects;
using NexuSys.DTOs.Repair_Activities;
using NexuSys.DTOs.Review;
using NexuSys.DTOs.Review_Activies;
using NexuSys.DTOs.Review_Defects;
using NexuSys.Entities;
using NexuSys.Interfaces;
using System.Data;

namespace NexuSys.Services
{
    public class BudgetService : IBudget
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public BudgetService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        #region Budget

        public async Task<Return> NewBudget(ByBudgetDTO dto)
        {
            var result = new Return();
            try
            {
                var entity = _mapper.Map<Budget>(dto);

                if (entity.ID == 0)
                {
                    await _context.Budgets.AddAsync(entity);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    _context.Budgets.Update(entity);
                    await _context.SaveChangesAsync();
                }

                result.Result = true;
                result.Message = "Orçamento criado/Atualizado com sucesso";
                result.Data = await _context.Budgets.AsNoTracking().FirstOrDefaultAsync(e => e.ID == entity.ID);
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = ex.Message;
                result.Error = ex.InnerException?.Message;
            }
            return result;
        }

        public async Task<Return> UpdateBudget(ByBudgetDTO dto)
        {
            var result = new Return();
            try
            {
                var entity = _mapper.Map<Budget>(dto);
                _context.Budgets.Update(entity);
                await _context.SaveChangesAsync();

                result.Result = true;
                result.Message = "Orçamento atualizado com sucesso";
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = ex.Message;
                result.Error = ex.InnerException?.Message;
            }
            return result;
        }

        public async Task<Return> DeleteBudget(int id)
        {
            var result = new Return();
            try
            {
                var entity = await _context.Budgets.FirstOrDefaultAsync(e => e.ID == id);
                _context.Budgets.Remove(entity);
                await _context.SaveChangesAsync();

                result.Result = true;
                result.Message = $"Orçamento ID:{id} removido";
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = ex.Message;
                result.Error = ex.InnerException?.Message;
            }
            return result;
        }

        public async Task<ByBudgetDTO> BudgetByID(int id) =>
            await _context.Budgets
                .AsNoTracking()
                .ProjectTo<ByBudgetDTO>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(e => e.ID == id);

        public async Task<List<BudgetDTO>> BudgetList() =>
            await _context.Budgets
                .AsNoTracking()
                .ProjectTo<BudgetDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();

        #endregion

        #region Possible_Defects

        public async Task<ByPossible_DefectsDTO> Possible_DefectsByID(int id) =>
            await _context.Possible_Defects
                .AsNoTracking()
                .ProjectTo<ByPossible_DefectsDTO>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(e => e.ID == id);

        public async Task<List<Possible_DefectsDTO>> Possible_DefectsList() =>
            await _context.Possible_Defects
                .AsNoTracking()
                .ProjectTo<Possible_DefectsDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();

        public async Task<Return> NewPossible_Defects(NewPossible_DefectsDTO dto)
        {
            var result = new Return();

            try
            {
                var entity = _mapper.Map<Possible_Defects>(dto);
                var history = entity.historyFK;


                await _context.History.AddAsync(history);
                await _context.SaveChangesAsync();
                _context.Entry(history).State = EntityState.Detached;


                entity.History = history.ID;
                entity.historyFK = null;

                await _context.Possible_Defects.AddAsync(entity);
                await _context.SaveChangesAsync();
                _context.Entry(entity).State = EntityState.Detached;
                result.Result = true;
                result.Message = "Defeito possível criado com sucesso";
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = ex.InnerException?.Message ?? ex.Message;
            }


            return result;
        }

        public async Task<Return> UpdatePossible_Defects(EditPossible_DefectsDTO dto)
        {
            var result = new Return();

            try
            {
                var entity = _mapper.Map<Possible_Defects>(dto);
                var history = entity.historyFK;


                _context.History.Update(history);
                await _context.SaveChangesAsync();

                entity.historyFK = null;
                _context.Possible_Defects.Update(entity);
                await _context.SaveChangesAsync();
                _context.Entry(entity).State = EntityState.Detached;
                result.Result = true;
                result.Message = "Defeito possível atualizado com sucesso";
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = ex.InnerException?.Message ?? ex.Message;

            }


            return result;
        }

        public async Task<Return> DeletePossible_Defects(int id)
        {
            var result = new Return();

            try
            {
                var entity = await _context.Possible_Defects
                    .FirstOrDefaultAsync(e => e.ID == id);

                if (entity != null)
                {
                    _context.Possible_Defects.Remove(entity);
                    await _context.SaveChangesAsync();
                }

                result.Result = true;
                result.Message = $"Defeito possível ID:{id} removido";
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = ex.Message;
                result.Error = ex.InnerException?.Message;
            }

            return result;
        }

        #endregion


        #region Repair_Activities

        public async Task<ByRepair_ActivitiesDTO> Repair_ActivitiesByID(int id) =>
            await _context.Repair_Activities
                .AsNoTracking()
                .ProjectTo<ByRepair_ActivitiesDTO>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(e => e.ID == id);


        public async Task<List<Repair_ActivitiesDTO>> Repair_ActivitiesList() =>
            await _context.Repair_Activities
                .AsNoTracking()
                .ProjectTo<Repair_ActivitiesDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();


        public async Task<Return> NewRepair_Activities(NewRepair_ActivitiesDTO dto)
        {
            var result = new Return();

            try
            {
                var repair = _mapper.Map<Repair_Activities>(dto);

                var history = repair.historyFK;

                if (history != null)
                {
                    repair.historyFK = null;

                    await _context.History.AddAsync(history);
                    await _context.SaveChangesAsync();

                    _context.Entry(history).State = EntityState.Detached;

                    repair.History = history.ID;
                }

                await _context.Repair_Activities.AddAsync(repair);
                await _context.SaveChangesAsync();

                _context.Entry(repair).State = EntityState.Detached;

                result.Result = true;
                result.Message = "Atividade de reparo criada com sucesso";
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = ex.Message;
                result.Error = ex.InnerException?.Message;
            }

            return result;
        }


        public async Task<Return> UpdateRepair_Activities(EditRepair_ActivitiesDTO dto)
        {
            var result = new Return();

            try
            {
                var repair = await _context.Repair_Activities
                    .FirstOrDefaultAsync(x => x.ID == dto.ID);

                if (repair == null)
                {
                    result.Result = false;
                    result.Message = "Atividade não encontrada";
                    return result;
                }

                repair.Name = dto.Name;

                await _context.SaveChangesAsync();

                result.Result = true;
                result.Message = "Atividade atualizada com sucesso";
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = ex.Message;
            }

            return result;
        }

        public async Task<Return> DeleteRepair_Activities(int id)
        {
            var result = new Return();

            try
            {
                var repair = await _context.Repair_Activities
                    .FirstOrDefaultAsync(e => e.ID == id);

                if (repair != null)
                {
                    _context.Repair_Activities.Remove(repair);
                    await _context.SaveChangesAsync();
                }

                result.Result = true;
                result.Message = $"Atividade de reparo ID:{id} removida";
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = ex.Message;
                result.Error = ex.InnerException?.Message;
            }

            return result;
        }

        #endregion

        #region Review_Activities

        public async Task<Return> NewReview_Activities(List<ByReview_ActivitiesDTO> dto)
        {
            var result = new Return();
            try
            {
                var search = await _context.Review_Activities.AsNoTracking().Where(e => e.Review == dto[0].Review).ToListAsync();

                foreach (var del in search)
                {
                    var x = dto.FirstOrDefault(e => e.Activities == del.Activities);

                    if (x == null)
                    {
                        _context.Review_Activities.Remove(del);
                        await _context.SaveChangesAsync();
                    }
                }
                foreach (var activie in dto)
                {

                    var entity = _mapper.Map<Review_Activities>(activie);

                    if (entity.ID == 0)
                    {
                        await _context.Review_Activities.AddAsync(entity);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        _context.Review_Activities.Update(entity);
                        await _context.SaveChangesAsync();
                    }
                }
                
                result.Result = true;
                result.Message = "Atividade de revisão criada";
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = ex.Message;
                result.Error = ex.InnerException?.Message;
            }
            return result;
        }

        public async Task<Return> UpdateReview_Activities(EditReview_ActivitiesDTO dto)
        {
            var result = new Return();
            try
            {
                var entity = _mapper.Map<Review_Activities>(dto);
                _context.Review_Activities.Update(entity);
                await _context.SaveChangesAsync();

                result.Result = true;
                result.Message = "Atividade de revisão atualizada";
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = ex.Message;
                result.Error = ex.InnerException?.Message;
            }
            return result;
        }

        public async Task<Return> DeleteReview_Activities(int id)
        {
            var result = new Return();
            try
            {
                var entity = await _context.Review_Activities.FirstOrDefaultAsync(e => e.ID == id);
                _context.Review_Activities.Remove(entity);
                await _context.SaveChangesAsync();

                result.Result = true;
                result.Message = $"Atividade de revisão ID:{id} removida";
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = ex.Message;
                result.Error = ex.InnerException?.Message;
            }
            return result;
        }

        public async Task<List<Review_ActivitiesDTO>> Review_ActivitiesList() =>
            await _context.Review_Activities
                .AsNoTracking()
                .ProjectTo<Review_ActivitiesDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();

        public async Task<ByReview_ActivitiesDTO> Review_ActivitiesByID(int id) =>
            await _context.Review_Activities
                .AsNoTracking()
                .ProjectTo<ByReview_ActivitiesDTO>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(e => e.ID == id);

        #endregion

        #region Review_Defects

        public async Task<Return> NewReview_Defects(List<ByReview_DefectsDTO> dto)
        {
            var result = new Return();
            try
            {
                var search = await _context.Review_Defects.AsNoTracking().Where(e => e.Review == dto[0].Review).ToListAsync();

                foreach (var del in search)
                {
                    var x = dto.FirstOrDefault(e => e.Defects == del.Defects);

                    if (x == null)
                    {
                        _context.Review_Defects.Remove(del);
                        await _context.SaveChangesAsync();
                    }
                }
                foreach (var defect in dto)
                {

                    var entity = _mapper.Map<Review_Defects>(defect);

                    if (entity.ID == 0)
                    {
                        await _context.Review_Defects.AddAsync(entity);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        _context.Review_Defects.Update(entity);
                        await _context.SaveChangesAsync();
                    }
                }

                result.Result = true;
                result.Message = "Defeito de revisão criado";
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = ex.Message;
                result.Error = ex.InnerException?.Message;
            }
            return result;
        }

        public async Task<Return> UpdateReview_Defects(EditReview_DefectsDTO dto)
        {
            var result = new Return();
            try
            {
                var entity = _mapper.Map<Review_Defects>(dto);
                _context.Review_Defects.Update(entity);
                await _context.SaveChangesAsync();

                result.Result = true;
                result.Message = "Defeito de revisão atualizado";
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = ex.Message;
                result.Error = ex.InnerException?.Message;
            }
            return result;
        }

        public async Task<Return> DeleteReview_Defects(int id)
        {
            var result = new Return();
            try
            {
                var entity = await _context.Review_Defects.FirstOrDefaultAsync(e => e.ID == id);
                _context.Review_Defects.Remove(entity);
                await _context.SaveChangesAsync();

                result.Result = true;
                result.Message = $"Defeito de revisão ID:{id} removido";
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = ex.Message;
                result.Error = ex.InnerException?.Message;
            }
            return result;
        }

        public async Task<List<Review_DefectsDTO>> Review_DefectsList() =>
            await _context.Review_Defects
                .AsNoTracking()
                .ProjectTo<Review_DefectsDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();

        public async Task<ByReview_DefectsDTO> Review_DefectsByID(int id) =>
            await _context.Review_Defects
                .AsNoTracking()
                .ProjectTo<ByReview_DefectsDTO>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(e => e.ID == id);

        #endregion

        #region Review

        public async Task<Return> NewReview(ByReviewDTO dto)
        {
            var result = new Return();
            try
            {
                var entity = _mapper.Map<Review>(dto);

                if (entity.ID == 0)
                {
                    await _context.Review.AddAsync(entity);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    _context.Review.Update(entity);
                    await _context.SaveChangesAsync();
                }

                result.Result = true;
                result.Message = "Revisão criada/Atualizado com sucesso";
                result.Data = await _context.Review.AsNoTracking().FirstOrDefaultAsync(e => e.ID == entity.ID);
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = ex.Message;
                result.Error = ex.InnerException?.Message;
            }
            return result;
        }

        public async Task<Return> UpdateReview(EditReviewDTO dto)
        {
            var result = new Return();
            try
            {
                var entity = _mapper.Map<Review>(dto);
                _context.Review.Update(entity);
                await _context.SaveChangesAsync();

                result.Result = true;
                result.Message = "Revisão atualizada com sucesso";
               
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = ex.Message;
                result.Error = ex.InnerException?.Message;
            }
            return result;
        }

        public async Task<Return> DeleteReview(int id)
        {
            var result = new Return();
            try
            {
                var entity = await _context.Review.FirstOrDefaultAsync(e => e.ID == id);
                _context.Review.Remove(entity);
                await _context.SaveChangesAsync();

                result.Result = true;
                result.Message = $"Revisão ID:{id} removida";
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Message = ex.Message;
                result.Error = ex.InnerException?.Message;
            }
            return result;
        }

        public async Task<List<ReviewDTO>> ReviewList() =>
            await _context.Review
                .AsNoTracking()
                .ProjectTo<ReviewDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();

        public async Task<ByReviewDTO> ReviewByID(int id) =>
            await _context.Review
                .AsNoTracking()
                .ProjectTo<ByReviewDTO>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(e => e.ID == id);

        #endregion
    }
}
