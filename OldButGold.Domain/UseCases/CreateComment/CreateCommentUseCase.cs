using MediatR;
using OldButGold.Forums.Domain.Authentication;
using OldButGold.Forums.Domain.Authorization;
using OldButGold.Forums.Domain.DomainEvents;
using OldButGold.Forums.Domain.Exceptions;
using OldButGold.Forums.Domain.Models;
using OldButGold.Forums.Domain.UseCases.CreateForum;
using OldButGold.Forums.Domain.UseCases.CreateTopic;
using OldButGold.Forums.Domain.UseCases.GetForums;
using OldButGold.Forums.Domain.UseCases.GetTopics;

namespace OldButGold.Forums.Domain.UseCases.CreateComment
{
    public class CreateCommentUseCase(
        IIntentionManager intentionManager,
        IIdentityProvider identityProvider,
        IUnitOfWork unitOfWork) : IRequestHandler<CreateCommentCommand, Models.Comment>
    {
        public async Task<Comment> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
        {

            await using var scope = await unitOfWork.CreateScope(cancellationToken);
            var createCommentStorage = scope.GetStorage<ICreateCommentStorage>();


            var topic = await createCommentStorage.FindTopic(request.TopicId, cancellationToken);
            if (topic is null)
            {
                throw new TopicNotFoundException(request.TopicId);
            }

            intentionManager.ThrowIfForbidden(TopicIntention.CreateComment, topic);

            var domainEventsStorage = scope.GetStorage<IDomainEventStorage>();
            var comment = await createCommentStorage.CreateComment(
                request.TopicId, identityProvider.Current.UserId, request.Text, cancellationToken);
            await domainEventsStorage.AddEvent(ForumDomainEvent.CommentCreated(topic, comment), cancellationToken);

            await scope.Commit(cancellationToken);

            return comment;
        }
    }
}
