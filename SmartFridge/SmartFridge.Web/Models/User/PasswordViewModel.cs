﻿using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace SmartFridge.Web.Models.User;
public class PasswordViewModel
{
    public int Id { get; set; }

    [Required]
    [Remote(action: "VerifyPassword", controller: "User")]
    public string OldPassword { get; set; }
    
    [Required]
    public string Password { get; set; }

    [Compare("Password", ErrorMessage = "Confirm password doesn't match, Type again !")]
    public string PasswordConfirm  { get; set; }

}
