using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace EmployeeManagement.Models
{
    public class Employee
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; } = null!;

        [BsonRepresentation(BsonType.ObjectId)]
        public string? PersonalInfoId { get; set; } = null!;

        [BsonRepresentation(BsonType.ObjectId)]
        public string? DepartmentId { get; set; } = null!;

        [BsonRepresentation(BsonType.ObjectId)]
        public string? PositionId { get; set; } = null!;

        public DateTime HireDate { get; set; }
        public decimal Salary { get; set; }
    }
}