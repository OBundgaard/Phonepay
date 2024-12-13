using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Phonepay.Core.Models;

public class FriendRequest
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ID { get; set; }

    [Required]
    public int SenderID { get; set; }

    [Required]
    public int ReceiverID { get; set; }

    [Required]
    public DateTime SentAt { get; set; }
}
