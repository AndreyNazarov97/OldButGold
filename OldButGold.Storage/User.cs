using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OldButGold.Storage
{
    public class User
    {
        [Key]
        public Guid UserId { get; set; }

        [MaxLength(30)]
        public string Login {  get; set; }

        [MaxLength(120)]
        public string Salt {  get; set; }

        [MaxLength(300)]
        public string PasswordHash {  get; set; }

        [InverseProperty(nameof(Topic.Author))]
        ICollection<Topic> Topics { get; set; }

        [InverseProperty(nameof(Comment.Author))]
        ICollection<Comment> Comments { get; set; }

    }
}
