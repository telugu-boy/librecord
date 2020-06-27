using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using lcapis.Entities;
using lcapis.Models.LCServers;
using lcapis.Models.LCUsers;

namespace lcapis.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            //users
            CreateMap<LCUser, UserModel>();
            CreateMap<RegisterUserModel, LCUser>();
            CreateMap<UpdateUserModel, LCUser>();

            //server
            CreateMap<CreateServerModel, LCServer>();
            CreateMap<JoinServerModel, LCInvite>();
        }
    }
}
