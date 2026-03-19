namespace NexuSys.Helper
{
    public class DefectsType
    {
        public int ID { get; set; }

        public string Name { get; set; }


        public static List<DefectsType> Defects { get; } = new List<DefectsType>
        {
            new DefectsType { ID = 0, Name = "Constante" },
            new DefectsType { ID = 1, Name = "Intermitente" },
            new DefectsType { ID = 2, Name = "Sem Defeito" }
        };
    }
}
