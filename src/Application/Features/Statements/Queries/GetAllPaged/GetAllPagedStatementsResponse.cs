namespace BlazorHero.CleanArchitecture.Application.Features.Statements.Queries.GetAllPaged
{
    public class GetAllPagedStatementsResponse
    {
        public int Id { get; set; }
        public string Body { get; set; }
        public string Topic { get; set; }
        public int TopicId { get; set; }
    }
}