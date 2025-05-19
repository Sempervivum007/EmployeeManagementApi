using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace EmployeeManagement.Models
{
    public class PersonalInfo
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string FullName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public DateTime BirthDate { get; set; }
    }
}