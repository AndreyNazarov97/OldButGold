using AutoMapper;
using OldButGold.Forums.Domain.DomainEvents;

namespace OldButGold.Forums.Storage.Mapping
{
    public class DomainEventsProfile : Profile
    {
        public DomainEventsProfile()
        {
            CreateMap<ForumDomainEvent, Models.ForumDomainEvent>();
            CreateMap<ForumComment, Models.ForumComment>();
        }
    }
}
