namespace OldButGold.Forums.Storage.Models
{
    internal class TopicListItemReadModel
    {
        public Guid TopicId { get; set; }   
        
        public string Title { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public int TotalCommentsCount { get; set; }

        public DateTimeOffset? LastCommentCreatedAt { get; set; }

        public string CommentText { get; set; }
    }
}
