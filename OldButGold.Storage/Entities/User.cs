using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OldButGold.Storage.Entities
{
    public class User
    {
        [Key]
        public Guid UserId { get; set; }

        [MaxLength(30)]
        public string Login { get; set; }

        [MaxLength(100)]
        public byte[] Salt { get; set; }

        [MaxLength(32)]
        public byte[] PasswordHash { get; set; }

        [InverseProperty(nameof(Topic.Author))]
        ICollection<Topic> Topics { get; set; }

        [InverseProperty(nameof(Comment.Author))]
        ICollection<Comment> Comments { get; set; }

        [InverseProperty(nameof(Session.User))]
        ICollection<Session> Sessions {  get; set; }

    }
}
