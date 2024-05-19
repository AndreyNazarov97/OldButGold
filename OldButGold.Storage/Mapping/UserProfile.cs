using AutoMapper;
using OldButGold.Forums.Domain.UseCases.SignIn;
using OldButGold.Forums.Storage.Entities;

namespace OldButGold.Forums.Storage.Mapping
{
    internal class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, RecognisedUser>();
            CreateMap<Session, Domain.Authentication.Session>();
        }
    }
}
