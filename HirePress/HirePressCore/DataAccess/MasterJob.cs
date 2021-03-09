namespace HirePressCore.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MasterJob")]
    public partial class MasterJob
    {
        [Key]
        public int ID { get; set; }
        public string JobID { get; set; }
        public string ApplicationEmail { get; set; }
        public string Category { get; set; }
        public string ClosingDate { get; set; }
        public string Company { get; set; }
        public string Description { get; set; }
        public string Education { get; set; }
        public string JobTags { get; set; }
        public string JobTitle { get; set; }
        public string Location { get; set; }
        public string Tagline { get; set; }
        public string Website { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string JobURL { get; set; }
        public bool IsFake { get; set; }
        public bool IsClosed { get; set; }
    }
}
