using AutoMapper;
using BlazorHero.CleanArchitecture.Application.Features.Statements.Commands.AddEdit;
using BlazorHero.CleanArchitecture.Domain.Entities;

namespace BlazorHero.CleanArchitecture.Application.Mappings
{
    public class StatementProfile : Profile
    {
        public StatementProfile()
        {
            CreateMap<AddEditStatementCommand, Statement>().ReverseMap();
        }
    }
}