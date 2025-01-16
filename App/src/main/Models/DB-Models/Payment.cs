using System.ComponentModel.DataAnnotations;

namespace Webshop.App.src.main.Models;

public class Payment
{
    [Key]
    public int PaymentId { get; set; }
    public int OrderId { get; set; }
    public DateTime PaymentDate { get; set; }
    public string PaymentMethod { get; set; }
    public decimal Amount { get; set; }
}