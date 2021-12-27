using System.Collections.Generic;
using BlazorHero.CleanArchitecture.Domain.Contracts;

namespace BlazorHero.CleanArchitecture.Domain.Entities
{
    public class Topic : AuditableEntity<int>
    {
        public string Name { get; set; }

        public int? ParentId { get; set; }
        public Topic Parent { get; set; }
        public List<Topic> Chilren { get; set; }
        public List<Statement> Statements { get; set; }
        //public List<Issue> Issues { get; set; }
        //public List<Article> Articles { get; set; }
    }
}
