using BlazorHero.CleanArchitecture.Application.Specifications.Base;
using BlazorHero.CleanArchitecture.Domain.Entities;

namespace BlazorHero.CleanArchitecture.Application.Specifications.Agenda
{
    public class StatementFilterSpecification : HeroSpecification<Statement>
    {
        public StatementFilterSpecification(string searchString)
        {
            Includes.Add(a => a.Topic);
            if (!string.IsNullOrEmpty(searchString))
            {
                Criteria = p => p.Body.Contains(searchString) || p.Topic.Name.Contains(searchString);
            }
            else
            {
                Criteria = p =>true;
            }
        }
    }
}