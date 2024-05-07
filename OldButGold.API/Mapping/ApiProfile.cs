using AutoMapper;
using OldButGold.Domain.Models;

namespace OldButGold.API.Mapping
{
    public class ApiProfile : Profile
    {
        public ApiProfile()
        {
            CreateMap<Forum, Models.Forum>();
            CreateMap<Topic, Models.Topic.Topic>();
        }
    }
}
