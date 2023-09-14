namespace AdvancedC_TopicsGroupAssignment.Models
{
    public class BusinessAddress
    {
        public int Id { get; set; }
        public int BusinessId { get; set; }
        public Business Business { get; set; }

        public int AddressId { get; set; }
        public Address Address { get; set; }
    }
}
