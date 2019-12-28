using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoeShop.Models
{
    [Table("Account")]
    public partial class Account
    {
        public Account()
        {
            Invoice = new HashSet<Invoice>();
            RoleAccount = new HashSet<RoleAccount>();
        }
        [Key]
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Fullname { get; set; }
        public string Email { get; set; }
        public bool Status { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }

        public virtual ICollection<Invoice> Invoice { get; set; }
        public virtual ICollection<RoleAccount> RoleAccount { get; set; }
    }
}
