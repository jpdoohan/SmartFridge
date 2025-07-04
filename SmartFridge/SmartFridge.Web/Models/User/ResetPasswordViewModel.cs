﻿using System.ComponentModel.DataAnnotations;

namespace SmartFridge.Web.Models.User;
public class ResetPasswordViewModel
{
    [Required]
    public string Email { get; set; }
    
    [Required]
    public string Token { get; set; }

    [Required]
    public string Password { get; set; }

    [Compare("Password", ErrorMessage = "Confirm password doesn't match, Type again !")]
    public string PasswordConfirm  { get; set; }
    
}
