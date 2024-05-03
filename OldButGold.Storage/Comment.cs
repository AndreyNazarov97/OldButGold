using System.ComponentModel.DataAnnotations;

namespace OldButGold.Storage
{
    public class Comment
    {
        [Key] 
        public Guid CommentId { get; set; }

        public Guid UserId { get; set; }

        public Guid TopicId { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset? UpdatedAt { get; set; }

        public string Text { get; set; }
        
    }
}