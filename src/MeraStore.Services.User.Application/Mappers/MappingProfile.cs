using AutoMapper;

using MeraStore.Services.User.Application.Dtos;

namespace MeraStore.Services.User.Application.Mappers;

public class MappingProfile : Profile
{
  public MappingProfile()
  {
    CreateMap<Domain.Entities.User, UserDto>().ReverseMap();
  }
}