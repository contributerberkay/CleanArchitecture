using BlazorHero.CleanArchitecture.Domain.Entities.Agenda;

namespace BlazorHero.CleanArchitecture.Domain.Entities
{
    public class Statement: Post
    {
        //public Issue Issue { get; set; }
        public int TopicId { get; set; }
        public Topic Topic { get; set; }
    }
}