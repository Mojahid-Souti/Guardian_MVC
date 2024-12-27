using System;
using System.ComponentModel.DataAnnotations;

namespace Guardian.Models
{
    public partial class Incident
    {
        public int IncidentId { get; set; }

        [Required(ErrorMessage = "PacketEntryId is required.")]
        public int PacketEntryId { get; set; }

        [Required(ErrorMessage = "Status is required.")]
        [StringLength(50, ErrorMessage = "Status cannot exceed 50 characters.")]
        public string Status { get; set; } = null!;

        [Required(ErrorMessage = "AssignedTo is required.")]
        [StringLength(100, ErrorMessage = "AssignedTo cannot exceed 100 characters.")]
        public string AssignedTo { get; set; } = null!;

        [StringLength(500, ErrorMessage = "Comment cannot exceed 500 characters.")]
        public string? Comment { get; set; }

        public DateTime? ResolvedAt { get; set; }

        [Required]
        public virtual PacketEntry PacketEntry { get; set; } = null!;
    }
}