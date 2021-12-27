using BlazorHero.CleanArchitecture.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorHero.CleanArchitecture.Domain.Entities.Blog 
{
    public class ArticleLike:AuditableEntity<int>
    {
        public int Like { get; set; }
        public int BiproArticleId { get; set; }//bu isim "articleid"ye kısaltılabilir
        public Article BiproArticle { get; set; }

    }
}
