using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BFuel.Domain.Models
{
    public class Supply
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "LiterPrice", ResourceType = typeof(BFuel.BFDomain.Utility.Fields))]
        [Required(ErrorMessageResourceType = typeof(BFuel.BFDomain.Utility.Messages), ErrorMessageResourceName = "MSG_E001")]
        public double LiterPrice { get; set; }
        [Display(Name = "TotalPaid", ResourceType = typeof(BFuel.BFDomain.Utility.Fields))]
        [Required(ErrorMessageResourceType = typeof(BFuel.BFDomain.Utility.Messages), ErrorMessageResourceName = "MSG_E001")]
        public double TotalPaid { get; set; }

        [Display(Name = "TotalLiters", ResourceType = typeof(BFuel.BFDomain.Utility.Fields))]
        [Required(ErrorMessageResourceType = typeof(BFuel.BFDomain.Utility.Messages), ErrorMessageResourceName = "MSG_E001")]
        public double TotalLiters { get; set; }

        [Display(Name = "LocalName", ResourceType = typeof(BFuel.BFDomain.Utility.Fields))]
        [Required(ErrorMessageResourceType = typeof(BFuel.BFDomain.Utility.Messages), ErrorMessageResourceName = "MSG_E001")]
        public string LocalName { get; set; }

        [Display(Name = "FuelName", ResourceType = typeof(BFuel.BFDomain.Utility.Fields))]
        [Required(ErrorMessageResourceType = typeof(BFuel.BFDomain.Utility.Messages), ErrorMessageResourceName = "MSG_E001")]
        public string FuelName { get; set; }

        public DateTime InsertedDate { get; set; }

        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}
