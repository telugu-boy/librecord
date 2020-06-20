using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using lcapis.Entities;
using lcapis.Models.LCUsers;

namespace lcapis.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<LCUser, UserModel>();
            CreateMap<RegisterModel, LCUser>();
            CreateMap<UpdateModel, LCUser>();
        }
    }
}
