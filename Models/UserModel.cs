using System.ComponentModel.DataAnnotations;

namespace ElasticAPI.Models
{
    public class UserModel : IIdentifiable
    {
        [Key]
        public string? Id { get; set; }
        public string nome { get; set; }
        public string email { get; set; }
        public string senha { get; set; }
        public string role { get; set; }
        public string data_cadastro { get; set; }
    }
}
