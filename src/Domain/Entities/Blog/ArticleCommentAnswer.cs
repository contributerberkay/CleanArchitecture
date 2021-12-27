using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorHero.CleanArchitecture.Domain.Entities.Blog
{
    public class ArticleCommentAnswer: Post 
    {
        public int ArticleCommentId { get; set; }
        public ArticleComment ArticleComment { get; set; }

    }
}
