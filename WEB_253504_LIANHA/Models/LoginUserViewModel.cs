﻿using System.ComponentModel.DataAnnotations;

namespace WEB_253504_LIANHA.Models
{
    public class LoginUserViewModel
    {
        [Required]
        public string Email { get; set; } = default!;
        [Required]
        public string Password { get; set; } = default!;
    }
}
