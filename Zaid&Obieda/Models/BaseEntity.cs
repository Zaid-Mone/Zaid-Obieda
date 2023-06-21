using System;
using System.ComponentModel.DataAnnotations;

namespace Zaid_Obieda.Models
{
    public class BaseEntity
    {
        [Key]
        public Guid Id { get; set; }

        public bool IsDeleted { get; set; } = false;
    }
}
