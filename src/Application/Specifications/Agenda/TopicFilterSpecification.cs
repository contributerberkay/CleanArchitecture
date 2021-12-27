using BlazorHero.CleanArchitecture.Application.Specifications.Base;
using BlazorHero.CleanArchitecture.Domain.Entities;

namespace BlazorHero.CleanArchitecture.Application.Specifications.Agenda
{
    public class TopicFilterSpecification : HeroSpecification<Topic>
    {
        public TopicFilterSpecification(string searchString)
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                Criteria = p => p.Name.Contains(searchString);
            }
            else
            {
                Criteria = p => true;
            }
        }
    }
}
