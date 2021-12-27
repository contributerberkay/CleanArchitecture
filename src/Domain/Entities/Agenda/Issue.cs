using System;
using System.Collections.Generic;

namespace BlazorHero.CleanArchitecture.Domain.Entities.Agenda
{
    public class Issue : Post
    {
        public int TopicId { get; set; }
        public Topic Topic { get; set; }
        public int? StatementId { get; set; }
        public Statement Statement { get; set; }
        public string Headline { get; set; }
        public bool IsActive { get; set; }

        public List<IssueTask> IssueTasks { get; set; }
        public List<IssueEntry> IssueEntries { get; set; }
    }
}
