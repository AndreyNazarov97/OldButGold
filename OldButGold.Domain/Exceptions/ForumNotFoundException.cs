namespace OldButGold.Domain.Exceptions
{
    public partial class ForumNotFoundException : DomainException
    {
        public ForumNotFoundException(Guid forumId) : base(ErrorCode.Gone, $"Forum with id {forumId} was not found")
        {
            
        }

    }
}
