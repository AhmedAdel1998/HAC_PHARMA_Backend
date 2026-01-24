using System.ComponentModel.DataAnnotations;

namespace HAC_Pharma.Domain.Entities;

public class Notification : BaseEntity
{
    [Required]
    public string Title { get; set; } = string.Empty;
    
    [Required]
    public string Message { get; set; } = string.Empty;
    
    [Required]
    public string Type { get; set; } = "system"; // rfq, application, system, alert
    
    public bool IsRead { get; set; } = false;
    
    public string? Link { get; set; }
    
    public int? RelatedEntityId { get; set; }
    
    public string? RelatedEntityType { get; set; }
    
    public string? UserId { get; set; } // If null, it's a broadcast/system-wide notification
}
