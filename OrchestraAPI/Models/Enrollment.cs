﻿using System.ComponentModel.DataAnnotations.Schema;

namespace OrchestraAPI.Models
{
    [Table ("Enrollment")]
    public class Enrollment
    {
        public int PlayerId { get; set; }
        public int OrchestraId { get; set; }
        public int SectionId { get; set; }
        public int InstrumentId { get; set; }
        public int Experience { get; set; }
        public int isApproved { get; set; }
    }
}
