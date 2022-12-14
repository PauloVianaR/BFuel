using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BFuel.Domain.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Display(Name="Name", ResourceType = typeof(BFuel.BFDomain.Utility.Fields))]
        [Required(ErrorMessageResourceType = typeof(BFuel.BFDomain.Utility.Messages), ErrorMessageResourceName = "MSG_E001")]
        //Required -> informa ao banco que o campo não pode ser nulo
        public string Name { get; set; }

        [Display(Name = "Email", ResourceType = typeof(BFuel.BFDomain.Utility.Fields))]
        [Required(ErrorMessageResourceType = typeof(BFuel.BFDomain.Utility.Messages), ErrorMessageResourceName = "MSG_E001")]
        [EmailAddress(ErrorMessageResourceType = typeof(BFuel.BFDomain.Utility.Messages), ErrorMessageResourceName = "MSG_E002")]
        public string Email { get; set; }

        [Display(Name = "Password", ResourceType = typeof(BFuel.BFDomain.Utility.Fields))]
        [Required(ErrorMessageResourceType = typeof(BFuel.BFDomain.Utility.Messages), ErrorMessageResourceName = "MSG_E001")]
        public string Password { get; set; }

        [Display(Name = "TotalExpenses", ResourceType = typeof(BFuel.BFDomain.Utility.Fields))]
        public int TotalExpenses_Config { get; set; } = -1;

        [Display(Name = "TotalSupplied", ResourceType = typeof(BFuel.BFDomain.Utility.Fields))]
        public int TotalSupplied_Config { get; set; } = -1;

        [Display(Name = "UserType", ResourceType = typeof(BFuel.BFDomain.Utility.Fields))]
        public int UserType { get; set; } = 0;
    }
}
