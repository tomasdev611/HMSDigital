﻿using System;

namespace HMS2SDK.Data.Models
{
    public partial class PatientMap
    {
        public int Id { get; set; }
        public string Source { get; set; }
        public int SourceIdNumber { get; set; }
        public int HmsIdNumber { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
