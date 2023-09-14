namespace AdvancedC_TopicsGroupAssignment.Models
{
    public class BusinessPerson
    {
        public int Id { get; set; }
        public int BusinessId { get; set; }
        public Business Business { get; set; }
        public int PersonId { get; set; }
        public Person Person { get; set; }
    }
}
