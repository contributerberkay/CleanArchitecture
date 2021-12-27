
namespace BlazorHero.CleanArchitecture.Domain.Entities.Agenda
{
    public class IssueEntry : Post
    {
        public int IssueId { get; set; }
        public Issue Issue { get; set; } 
    }
}