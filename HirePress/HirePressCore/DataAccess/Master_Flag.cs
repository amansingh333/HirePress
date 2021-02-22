namespace HirePressCore.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Master_Flag
    {
        [Key]
        public int FlagID { get; set; }

        [StringLength(20)]
        public string FlagName { get; set; }

        public bool IsFlag { get; set; }
    }
}
