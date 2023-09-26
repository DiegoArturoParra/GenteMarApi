using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GenteMarCore.Entities.Models
{
    [Table("GENTEMAR_LOGS", Schema = "DBA")]
    public partial class GENTEMAR_LOGS
    {
        [Key]
        public long ID_LOG { get; set; }
        public int STATUS_CODE { get; set; }
        public string MESSAGE_WARNING { get; set; }
        public string MESSAGE_EXCEPTION { get; set; }
        public DateTime DATE_CREATED { get; set; }
        public string USER_SESSION { get; set; }
        public string SEVERITY_LEVEL { get; set; }
    }
}
