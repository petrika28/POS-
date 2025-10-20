namespace WebApplication5.Models
{
    public class Invoice
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public DateTime Date { get; set; }
        public double Value { get; set; }
        public int ClientId {  get; set; }
        public Client Client {  get; set; }
        public List<Product> Products { get; set; } = new List<Product>();

        }
    }

