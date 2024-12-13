using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Phonepay.Core.Models;

public class TransactionRequest
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ID { get; set; }

    [Required]
    public int SenderID { get; set; }

    [Required]
    public int ReceiverID { get; set; }

    [Required]
    public string? Currency { get; set; }

    [Required]
    public float Amount { get; set; }

    [Required]
    public DateTime SentAt { get; set; }
}
