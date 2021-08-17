using AppGlobal.Constants;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AppGlobal.Entities
{
    public class TrackEntity
    {   
        [Required]
        public DateTime DateCreated { get; set; }

        [Required]
        public DateTime DateUpdated { get; set; }
    }
    public class AuditEntity
    {
        [Required]
        [DefaultValue((int)StatusType.Active)]
        public int StatusID { get; set; }

        [Required]
        public DateTime DateCreated { get; set; }

        [Required]
        public DateTime DateUpdated { get; set; }
    }

    public class AuthEntity
    {
        public string UserID { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
    }

    public class LoginResult
    {
        public string Token { get; set; }
        public AuthEntity User { get; set; }
    }

    public class LoginEntity
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
