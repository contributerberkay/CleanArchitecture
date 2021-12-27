namespace BlazorHero.CleanArchitecture.Client.Infrastructure.Routes
{
    public static class TopicsEndpoints
    {
        public static string ExportFiltered(string searchString)
        {
            return $"{Export}?searchString={searchString}";
        }

        public static string Export = "api/v1/topics/export";

        public static string GetAll = "api/v1/topics";
        public static string Delete = "api/v1/topics";
        public static string Save = "api/v1/topics";
        public static string GetCount = "api/v1/topics/count";
        public static string Import = "api/v1/topics/import";
  }
}