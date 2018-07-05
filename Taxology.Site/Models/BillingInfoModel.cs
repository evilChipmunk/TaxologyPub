using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Shared.Domain;

namespace Taxology.Site.Models
{
    [Serializable]
    public class BillingInfoModel
    {
        public BillingInfoModel()
        {
            StateList = States.ToList();
            CountryList = Countries.ToList();
        }
        [Required]
        public Guid CustomerId { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        public string Address1 { get; set; }

        public string Address2 { get; set; }

        [Required]
        public string ZipCode { get; set; }

        [Required]
        public string StateAbbreviation { get; set; }

        [Required]
        public string Country { get; set; } 

        public IEnumerable<Country> CountryList { get; set; }
        public IEnumerable<State> StateList { get; set; }
    }
}