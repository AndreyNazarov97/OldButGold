using System.ComponentModel.DataAnnotations;

namespace OldButGold.Storage
{
    public class Topic
    {
        [Key]
        public Guid TopicId { get; set; }

        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }

        public Guid ForumId { get; set; }

        public Guid UserId { get; set; }
        public string Title { get; set; }
    }
}