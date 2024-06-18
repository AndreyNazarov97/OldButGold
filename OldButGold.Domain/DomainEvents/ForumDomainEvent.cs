using OldButGold.Forums.Domain.Models;

namespace OldButGold.Forums.Domain.DomainEvents
{
    public enum ForumDomainEventType
    {
        TopicCreated = 100,
        TopicUpdated = 101,
        TopicDeleted = 102,

        CommentCreated = 200,
        CommentUpdated = 201,
        CommentDeleted = 202,
    }

    public class ForumDomainEvent
    {
        private ForumDomainEvent()
        {
        }

        public ForumDomainEventType EventType { get; init; }

        public Guid TopicId { get; init; }

        public string Title { get; init; } = null;

        public ForumComment? Comment { get; init; }

        public static ForumDomainEvent TopicCreated(Topic topic)
        {
            return new ForumDomainEvent
            {
                EventType = ForumDomainEventType.TopicCreated,
                TopicId = topic.Id,
                Title = topic.Title,
                Comment = null
            };
        }
        public static ForumDomainEvent CommentCreated(Topic topic, Comment comment)
        {
            return new ForumDomainEvent
            {
                EventType = ForumDomainEventType.CommentCreated,
                TopicId = topic.Id,
                Title = topic.Title,
                Comment = new ForumComment()
                {
                    CommentId = comment.Id,
                    Text = comment.Text
                }
            };
        }



    }
    public class ForumComment
    {
        public Guid CommentId { get; init; }

        public string Text { get; init; }
    }
}
