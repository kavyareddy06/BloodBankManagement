using System.Globalization;

namespace BloodBankManagement.Models
{
    public class BloodBank
    {
        public int Id { get; set; }
        public string DonorName { get; set; }
        public int Age {  get; set; }
        public string BloodType { get; set; }
        public string PhoneNo {  get; set; }
        public int Quantity {  get; set; }
        public DateTime CollectionDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string Status {  get; set; }
    }
}
