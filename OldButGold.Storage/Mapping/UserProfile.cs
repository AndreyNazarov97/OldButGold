using AutoMapper;
using OldButGold.Domain.UseCases.SignIn;
using OldButGold.Storage.Entities;

namespace OldButGold.Storage.Mapping
{
    internal class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, RecognisedUser>();
        }
    }
}
