﻿using System.ComponentModel.DataAnnotations;
using JobPortal.Enums;

namespace JobPortal.Models.Dto
{
    public class UserLoginResponseDto
    {
        [Required]
        public int UserID { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(255, ErrorMessage = "Name cannot exceed 255 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        [StringLength(255, ErrorMessage = "Email cannot exceed 255 characters.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Role is required.")]
        public UserRole Role { get; set; }

        public string Token { get; set; }
    }
}