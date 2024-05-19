namespace OldButGold.Forums.Domain.Exceptions
{
    public partial class ForumNotFoundException(Guid forumId) : DomainException(DomainErrorCode.Gone, $"Forum with id {forumId} was not found")
    {
    }
}
