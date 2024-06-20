namespace OldButGold.Forums.Domain.Models
{
    public class Comment
    {
        public Guid Id { get; set; }    
        
        public DateTimeOffset CreatedAt { get; set; }

        public Guid TopicId { get; set; }

        public Guid UserId { get; set; }

        public string Text { get; set; }

        public string AuthorLogin { get; set; }
    }
}
