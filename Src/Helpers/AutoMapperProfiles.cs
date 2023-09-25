using api.DTOs.Comment;
using api.DTOs.Post;
using api.DTOs.Role;
using api.DTOs.Skill;
using api.DTOs.User;
using api.Extensions;
using api.Models;
using AutoMapper;

namespace api.Helpers;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<User, UserDto>();
        CreateMap<User, UserWithSkillsDto>()
            .ForMember(
                dest => dest.Age,
                opt => opt.MapFrom(src => src.Birthdate.CalculateAge())
            );
        CreateMap<Skill, SkillDto>();
        CreateMap<Post, PostDto>()
            .ForMember(
                dest => dest.Username,
                opt => opt.MapFrom(src => src.User.Username)
            )
            .ForMember(
                dest => dest.PhotoUrl,
                opt => opt.MapFrom(src => src.Url)
            )
            .ForMember(
                dest => dest.TotalLikes,
                opt => opt.MapFrom(src => src.Likes.Count())
            )
            .ForMember(
                dest => dest.TotalComments,
                opt => opt.MapFrom(src => src.Comments.Count())
            );
        CreateMap<Comment, CommentDto>()
            .ForMember(
                dest => dest.Username,
                opt => opt.MapFrom(src => src.User.Username)
            );
        CreateMap<Role, RoleDto>();
        CreateMap<User, UserWithStatusDto>()
            .ForMember(
                dest => dest.Age,
                opt => opt.MapFrom(src => src.Birthdate.CalculateAge())
            )
            .ForMember(
                dest => dest.IsEnabled,
                opt => opt.MapFrom(src => src.DisabledAt == DateTime.MinValue ? true : false)
            );
    }
}