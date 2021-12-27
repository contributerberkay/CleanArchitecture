namespace BlazorHero.CleanArchitecture.Application.Requests.Agenda
{
    public class GetAllPagedStatementsRequest : PagedRequest
    {
        public string SearchString { get; set; }
    }
}