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
        //Required -> informa ao banco que o campo não pode ser nulo
        public string Name { get; set; }

        [Display(Name = "Email", ResourceType = typeof(BFuel.Domain.Utility.Language.Fields))]
        [Required(ErrorMessageResourceType = typeof(BFuel.Domain.Utility.Language.Messages), ErrorMessageResourceName = "MSG_E001")]
        [EmailAddress(ErrorMessageResourceType = typeof(BFuel.Domain.Utility.Language.Messages), ErrorMessageResourceName = "MSG_E002")]
        public string Email { get; set; }

        [Display(Name = "Password", ResourceType = typeof(BFuel.Domain.Utility.Language.Fields))]
        [Required(ErrorMessageResourceType = typeof(BFuel.Domain.Utility.Language.Messages), ErrorMessageResourceName = "MSG_E001")]
        public string Password { get; set; }
    }
}
