using AutoMapper;
using OldButGold.Forums.Domain.Models;

namespace OldButGold.Forums.API.Mapping
{
    public class ApiProfile : Profile
    {
        public ApiProfile()
        {
            CreateMap<Forum, Models.Forum>();  
            CreateMap<Topic, Models.Topics.Topic>();
            CreateMap<Comment, Models.Comments.Comment>();
        }
    }
}
