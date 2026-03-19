using NexuSys.DTOs.Budget;
using NexuSys.DTOs.Review_Activies;
using NexuSys.DTOs.Review_Defects;
using NexuSys.DTOs.Repair_Activities;
using NexuSys.DTOs.Possible_Defects;
using NexuSys.DTOs.Review;
using NexuSys.Entities;

namespace NexuSys.Interfaces
{
    public interface IBudget
    {
        #region Budget
        Task<Return> NewBudget(ByBudgetDTO Budget);
        Task<Return> UpdateBudget(ByBudgetDTO Budget);
        Task<Return> DeleteBudget(int id);
        Task<List<BudgetDTO>> BudgetList();
        Task<ByBudgetDTO> BudgetByID(int id);
        #endregion

        #region Review Activies
        Task<Return> NewReview_Activities(List<ByReview_ActivitiesDTO> Review_Activities);
        Task<Return> UpdateReview_Activities(EditReview_ActivitiesDTO Review_Activities);
        Task<Return> DeleteReview_Activities(int id);
        Task<List<Review_ActivitiesDTO>> Review_ActivitiesList();
        Task<ByReview_ActivitiesDTO> Review_ActivitiesByID(int id);
        #endregion

        #region Review Defects
        Task<Return> NewReview_Defects(List<ByReview_DefectsDTO> Review_Defects);
        Task<Return> UpdateReview_Defects(EditReview_DefectsDTO Review_Defects);
        Task<Return> DeleteReview_Defects(int id);
        Task<List<Review_DefectsDTO>> Review_DefectsList();
        Task<ByReview_DefectsDTO> Review_DefectsByID(int id);
        #endregion

        #region Repair_Activities

        Task<Return> NewRepair_Activities(NewRepair_ActivitiesDTO repairActivities);

        Task<Return> UpdateRepair_Activities(EditRepair_ActivitiesDTO repairActivities);

        Task<Return> DeleteRepair_Activities(int id);

        Task<List<Repair_ActivitiesDTO>> Repair_ActivitiesList();

        Task<ByRepair_ActivitiesDTO> Repair_ActivitiesByID(int id);

        #endregion


        #region Possible Defects
        Task<Return> NewPossible_Defects(NewPossible_DefectsDTO Possible_Defects);
        Task<Return> UpdatePossible_Defects(EditPossible_DefectsDTO Possible_Defects);
        Task<Return> DeletePossible_Defects(int id);
        Task<List<Possible_DefectsDTO>> Possible_DefectsList();
        Task<ByPossible_DefectsDTO> Possible_DefectsByID(int id);
        #endregion

        #region Review
        Task<Return> NewReview(ByReviewDTO Review);
        Task<Return> UpdateReview(EditReviewDTO Review);
        Task<Return> DeleteReview(int id);
        Task<List<ReviewDTO>> ReviewList();
        Task<ByReviewDTO> ReviewByID(int id);
        #endregion

    }
}
