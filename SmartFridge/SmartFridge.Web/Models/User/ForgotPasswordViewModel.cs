using System.ComponentModel.DataAnnotations;

namespace SmartFridge.Web.Models.User;
public class ForgotPasswordViewModel
{
    [Required]
    public string Email { get; set; }
    
}
