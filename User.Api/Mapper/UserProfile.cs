using AutoMapper;
using UserApp.Infrastructure.Entities;
using UserApp.Api.Models;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Text;

namespace UserApp.Api.Mapper
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, User>();
            CreateMap<UserInsertRequestModel, UserInsertRequestModel>();
            CreateMap<UserUpdateRequestModel, UserUpdateRequestModel>();
            CreateMap<UserResponseModel, UserResponseModel>();

            CreateMap<UserInsertRequestModel, User>();

            CreateMap<UserUpdateRequestModel, User>()
                .ForMember(
                    dest => dest.Id,
                    options => options.Ignore()
                );
            //.ForMember(
            //    dest => dest.Id,
            //    opt => opt.MapFrom( src => new Guid( protector.Unprotect( src.Id ) ) )
            //);

            CreateMap<User, UserResponseModel>();
            //.ForMember(
            //    dest => dest.Id,
            //    opt => opt.MapFrom( src => protector.Protect( src.Id.ToString() ) )
            //);
        }
    }
}
