namespace HirePressCore.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class MasterSkills
    {
        [Key]
        public int SkillID { get; set; }

        [StringLength(20)]
        public string SkillType { get; set; }

        public string SkillTypeData { get; set; }
    }
}
