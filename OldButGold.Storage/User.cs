using System.ComponentModel.DataAnnotations;

namespace OldButGold.Storage
{
    public class User
    {
        [Key]
        public Guid UserId { get; set; }
        [MaxLength(30)]
        public string Login {  get; set; }

    }
}
