using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Phonepay.Core.Models;

public class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ID { get; set; }

    [Required]
    public string? PhoneNumber { get; set; }

    [Required]
    public string? Name { get; set; }

    [Required]
    public DateTime CreatedDate { get; set; }
}
