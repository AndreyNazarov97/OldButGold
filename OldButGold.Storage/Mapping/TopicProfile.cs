using AutoMapper;
using OldButGold.Storage.Entities;

namespace OldButGold.Storage.Mapping
{
    internal class TopicProfile : Profile
    {
        public TopicProfile()
        {
            CreateMap<Topic, Domain.Models.Topic>()
                .ForMember(d => d.Id, s => s.MapFrom(t => t.TopicId));
        }
    }
}
