namespace OldButGold.Domain.Exceptions
{
    public partial class ForumNotFoundException : DomainException
    {
        public ForumNotFoundException(Guid forumId) : base(DomainErrorCode.Gone, $"Forum with id {forumId} was not found")
        {
            
        }

    }
}
