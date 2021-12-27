namespace BlazorHero.CleanArchitecture.Domain.Entities.Agenda
{
    public class IssueTask : Post
    {
        public string Name { get; set; }
        public Status Status { get; set; }
        public int IssueId { get; set; }
        public Issue Issue { get; set; }
    }

    public enum Status
    {
        ToDo=1,Inprogress=2,Done=3
    }
}