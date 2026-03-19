namespace NexuSys.DTOs.Products
{
    public class ProductsDTO
    {
        public int ID { get; set; }
        public string Family { get; set; }
        public string SAP_Description { get; set; }
        public string? Packaging { get; set; }
        public decimal Gross_Price { get; set; }
        public string IPI { get; set; }
        public string? Barcode { get; set; }
        public int NCM { get; set; }
        public decimal? Length { get; set; }
        public decimal? Width { get; set; }
        public decimal? Height { get; set; }
        public string? Weight_Unit { get; set; }
        public decimal? Gross_Weight { get; set; }
        public decimal? Net_Weight { get; set; }
        public string? Hierarchy { get; set; }
        public string Billing_CNPJ { get; set; }
        public string Origin_CST { get; set; }
        public int History { get; set; }
        public decimal Stock { get; set; }
    }
}
