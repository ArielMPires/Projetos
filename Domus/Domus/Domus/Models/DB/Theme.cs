namespace Domus.Models.DB
{
    public class Theme
    {
        #region Property
        public int ID { get; set; }
        public int User {  get; set; }
        public string Primary {  get; set; }
        public string Secondary { get; set; }
        public string Tertiary { get; set; }
        public string PrimaryDark { get; set; }
        public string PrimaryDarkText { get; set; }
        public string SecondaryDarkText { get; set; }
        #endregion

        #region Navigation
        public Users? UserFK { get; set; }
        #endregion
    }
}
