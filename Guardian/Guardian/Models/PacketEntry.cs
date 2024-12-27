
using System.ComponentModel.DataAnnotations;

namespace Guardian.Models
{
    public partial class PacketEntry
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Source (Src) is required.")]
        [StringLength(50, ErrorMessage = "Source (Src) cannot exceed 50 characters.")]
        public string Src { get; set; } = null!;

        [Required(ErrorMessage = "Destination (Dst) is required.")]
        [StringLength(50, ErrorMessage = "Destination (Dst) cannot exceed 50 characters.")]
        public string Dst { get; set; } = null!;

        [Range(1, 65535, ErrorMessage = "Port must be between 1 and 65535.")]
        public int Port { get; set; }

        [Required(ErrorMessage = "ServiceId is required.")]
        public int ServiceId { get; set; }

        [Required(ErrorMessage = "Info is required.")]
        [StringLength(200, ErrorMessage = "Info cannot exceed 200 characters.")]
        public string Info { get; set; } = null!;

        [Required(ErrorMessage = "Attack description is required.")]
        [StringLength(100, ErrorMessage = "Attack description cannot exceed 100 characters.")]
        public string Attack { get; set; } = null!;

        [Required(ErrorMessage = "ThreatLevel is required.")]
        [StringLength(50, ErrorMessage = "ThreatLevel cannot exceed 50 characters.")]
        public string ThreatLevel { get; set; } = null!;

        [Required(ErrorMessage = "Timestamp is required.")]
        public DateTime Timestamp { get; set; }

        public virtual ICollection<Incident> Incidents { get; set; } = new List<Incident>();

        [Required(ErrorMessage = "Service is required.")]
        public virtual Service Service { get; set; } = null!;
    }
}