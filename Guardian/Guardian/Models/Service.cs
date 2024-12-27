using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Guardian.Models
{
    public partial class Service
    {
        public int ServiceId { get; set; }

        [Required(ErrorMessage = "ServiceName is required.")]
        [StringLength(100, ErrorMessage = "ServiceName cannot exceed 100 characters.")]
        public string ServiceName { get; set; } = null!;

        public virtual ICollection<PacketEntry> PacketEntries { get; set; } = new List<PacketEntry>();
    }
}