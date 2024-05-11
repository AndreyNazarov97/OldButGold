using AutoMapper;
using OldButGold.Storage.Entities;

namespace OldButGold.Storage.Mapping
{
    internal class ForumProfile : Profile
    {
        public ForumProfile()
        {
            CreateMap<Forum, Domain.Models.Forum>()
                .ForMember(d => d.Id, s => s.MapFrom(f => f.ForumId));
                
        }

    }
}
