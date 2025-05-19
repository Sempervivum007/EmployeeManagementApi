using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.Models
{
    public class Department
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; } = null!;

        [Required(ErrorMessage = "Назва обов'язкова")]
        public string Name { get; set; } = null!;
        
        [Required(ErrorMessage = "Опис обов'язковий")]
        public string Description { get; set; } = null!;
    }
}