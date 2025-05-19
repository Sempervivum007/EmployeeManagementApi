namespace EmployeeManagement.Models
{
    public class EmployeeViewModel
    {
        public string Id { get; set; }
        public PersonalInfo PersonalInfo { get; set; }
        public Department Department { get; set; }
        public Position Position { get; set; }
        public DateTime HireDate { get; set; }
        public decimal Salary { get; set; }
    }
}
