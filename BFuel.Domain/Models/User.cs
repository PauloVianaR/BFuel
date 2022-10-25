using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BFuel.Domain.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Display(Name="Name", ResourceType = typeof(BFuel.Domain.Utility.Language.Fields))]
        [Required(ErrorMessageResourceType = typeof(BFuel.Domain.Utility.Language.Messages), ErrorMessageResourceName = "MSG_E001")]
        public string Name { get; set; }

        [Display(Name = "Email", ResourceType = typeof(BFuel.Domain.Utility.Language.Fields))]
        [Required(ErrorMessageResourceType = typeof(BFuel.Domain.Utility.Language.Messages), ErrorMessageResourceName = "MSG_E001")]
        [EmailAddress(ErrorMessageResourceType = typeof(BFuel.Domain.Utility.Language.Messages), ErrorMessageResourceName = "MSG_E002")]
        public string Email { get; set; }

        [Display(Name = "Password", ResourceType = typeof(BFuel.Domain.Utility.Language.Fields))]
        [Required(ErrorMessageResourceType = typeof(BFuel.Domain.Utility.Language.Messages), ErrorMessageResourceName = "MSG_E001")]
        public string Password { get; set; }

        [Display(Name = "TotalExpenses", ResourceType = typeof(BFuel.Domain.Utility.Language.Fields))]
        public int TotalExpenses_Config { get; set; }

        [Display(Name = "TotalSupplied", ResourceType = typeof(BFuel.Domain.Utility.Language.Fields))]
        public int TotalSupplied_Config { get; set; }

        [Display(Name = "UserType", ResourceType = typeof(BFuel.Domain.Utility.Language.Fields))]
        public int UserType { get; set; }
    }
}
