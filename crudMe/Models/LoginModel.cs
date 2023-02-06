using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace crudMe.Models
{
    public class LoginModel
    {
        public LoginModel()
        {
        }
        [BindProperty]
        public Credential Credential { get; set; }
    }

    public class Credential
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}

