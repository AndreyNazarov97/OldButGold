﻿using AutoMapper;
using OldButGold.Forums.Storage.Entities;

namespace OldButGold.Forums.Storage.Mapping
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
