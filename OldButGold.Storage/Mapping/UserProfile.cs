using AutoMapper;
using OldButGold.Domain.UseCases.SignIn;

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
