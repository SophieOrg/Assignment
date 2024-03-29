﻿using System;
using System.ComponentModel.DataAnnotations;

namespace FMS.Data.Models
{
    
    public enum Role { volunteer, manager, guest }

    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public Role Role { get; set; }
    }
}
