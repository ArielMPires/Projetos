using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NexuSys.Entities
{
    public class Service_Order
    {

        #region Property
        public int ID { get; set; }
        public int Customer { get; set; }
        public DateTime Date_Receipt { get; set; }
        public int Situation { get; set; }
        public int Department { get; set; }
        public string Problem { get; set; }
        public DateTime? Departure_date { get; set; }
        public int? Service_Note { get; set; }
        public int? Delivery_Note { get; set; }
        public int? Departure_Note { get; set; }
        public string? Note { get; set; }
        public int FileFolder { get; set; }
        public int History { get; set; }
        public int Unit { get; set; }
        public int? Technical { get; set; }
        public int Type_Service { get; set; }
        public DateTime? Estimated_Date { get; set; }
        public int Priority { get; set; }
        #endregion

        #region Navegation
        public History? historyFK { get; set; }          
        public Budget? BudgetFK { get; set; }
        public Customers? CustomersFK { get; set; }
        public Department? departmentFK { get; set; }
        public NFs? DeliveryNoteFK { get; set; }
        public NFs? DepartureNoteFK { get; set; }
        public NFs? ServiceNoteFK { get; set; }
        public Unit? unitFK { get; set; }
        public Users? TechnicalFK { get; set; }
        public Type_Service? Type_ServiceFK { get; set; }
        public Situation? SituationFK { get; set; }
        public Service_Equipment? service_equipmentFK { get; set; }
        public ObservableCollection<Service_Items>? ItemsFK {  get; set; }
        public FileFolder? filefolderFK { get; set; }

        #endregion

    }
}
