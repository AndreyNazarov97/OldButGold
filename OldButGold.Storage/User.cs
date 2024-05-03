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

        [InverseProperty(nameof(Topic.Author))]
        ICollection<Topic> Topics { get; set; }

        [InverseProperty(nameof(Comment.Author))]
        ICollection<Comment> Comments { get; set; }

    }
}
