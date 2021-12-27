using AutoMapper;
using BlazorHero.CleanArchitecture.Application.Features.Topics.Commands.AddEdit;
using BlazorHero.CleanArchitecture.Application.Features.Topics.Queries.GetAll;
using BlazorHero.CleanArchitecture.Application.Features.Topics.Queries.GetById;
using BlazorHero.CleanArchitecture.Domain.Entities;

namespace BlazorHero.CleanArchitecture.Application.Mappings
{
    public class TopicProfile : Profile
    {
        public TopicProfile()
        {
            CreateMap<AddEditTopicCommand, Topic>().ReverseMap();
            CreateMap<GetTopicByIdResponse, Topic>().ReverseMap();
            CreateMap<GetAllTopicsResponse, Topic>().ReverseMap();
        }
    }
}