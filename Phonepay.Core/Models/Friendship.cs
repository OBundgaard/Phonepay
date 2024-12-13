using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Phonepay.Core.Models;

public class Friendship
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ID { get; set; }

    [Required]
    public int UserID { get; set; }

    [Required]
    public int FriendID { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; }
}
