using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorHero.CleanArchitecture.Domain.Entities.Blog
{
    public class Article: Post
    {
        public int View { get; set; }
        public ArticleLike Like { get; set; } 
        public string Title { get; set; }
        public string Description { get; set; }
        public List<ArticleComment> Comments { get; set; }
        public int? TopicId { get; set; }
        public Topic Topic { get; set; }
    }
}