using BlazorHero.CleanArchitecture.Domain.Contracts;

namespace BlazorHero.CleanArchitecture.Domain.Entities
{
    public abstract class Post : AuditableEntity<int>
    {
        public string Body { get; set; }

    }
}