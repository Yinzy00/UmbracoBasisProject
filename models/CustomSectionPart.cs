using NPoco;
using System;
using Umbraco.Cms.Infrastructure.Persistence.DatabaseAnnotations;

namespace BasisProjectUmbraco.models
{
    [TableName("MyCustomSectionParts")]
    [PrimaryKey("Id", AutoIncrement = true)]
    [ExplicitColumns]
    public class CustomSectionPart
    {
        [PrimaryKeyColumn(AutoIncrement = true, IdentitySeed = 1)]
        [Column("Id")]
        public int Id { get; set; }

        [Column("Title")]
        public string Title { get; set; }

        [Column("Description")]
        public string Description { get; set; }

        [Column("Created")]
        public DateTime Created { get; set; }

        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(Title) && !string.IsNullOrEmpty(Title);
        }
    }
}
