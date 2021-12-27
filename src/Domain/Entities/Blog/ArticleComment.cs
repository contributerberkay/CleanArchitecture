using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorHero.CleanArchitecture.Domain.Entities.Blog
{
    public class ArticleComment: Post
    {
        public int BiproArticleId { get; set; }
        public Article BiproArticle { get; set; }
        public List<ArticleCommentAnswer> Answers { get; set; } 

    }
}
