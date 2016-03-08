/*FarmMetadata.cs
 * a class to apply validation/edits and apply the resource files
 * 
 *      Revision History:
 *          Cody Lefebvre:04.05.2015:Created
 *          Cody Lefebvre:04.06.2015:Added annotations for translations
 *          Cody Lefebvre:04.06.2015:Added annotations for partial class errors
 */
using OEC06.App_GlobalResources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OEC06.Models
{
    [MetadataType(typeof(farmMetadata))]
    public partial class farm 
    {
        OECContext db = new OECContext();
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (town == null) town = ""; else town = town.Trim();
            if (county == null) county = ""; else county = county.Trim();
            if (town == "" && county == "")
                yield return new ValidationResult(string.Format(Translations.TownCountyError), new [] { "town", "county" });
            if (provinceCode != null)
            {
                provinceCode = provinceCode.ToUpper();
                var province = db.provinces.Find(provinceCode);
                if (province == null)
                    yield return new ValidationResult(string.Format(string.Format(Translations.ProvinceCodeError), provinceCode), new[] { "provinceCode" });
            }
            if (postalCode != null)
            {
                postalCode = postalCode.ToUpper();
                if (postalCode.Length == 6) postalCode = postalCode.Insert(3, " ");
            }
            if (homePhone != null)
            {
                string newPhone ="";
                foreach (char item in homePhone)
                    if (item >= '0' && item <= '9') newPhone += item;
                if (newPhone.Length != 10)
                    yield return new ValidationResult(string.Format(Translations.PhoneError), new[] { "homePhone" });
                else
                    homePhone = newPhone.Insert(3, "-").Insert(7, "-");
            }
            if (cellPhone != null)
            {
                string newPhone = "";
                foreach (char item in cellPhone)
                    if (item >= '0' && item <= '9') newPhone += item;
                if (newPhone.Length != 10)
                    yield return new ValidationResult(string.Format(Translations.PhoneError), new[] { "cellPhone" });
                else
                    cellPhone = newPhone.Insert(3, "-").Insert(7, "-");
            }

            if (dateJoined != null && (dateJoined > DateTime.Now))
                    yield return new ValidationResult(string.Format(Translations.DateJoinedError), new[] { "dateJoined" });
            if (lastContactDate != null)
            {
                if (lastContactDate > DateTime.Now)
                    yield return new ValidationResult(string.Format(Translations.DateOfLastContactError), new[] { "lastContactDate" });
                if (dateJoined == null)
                    yield return new ValidationResult(string.Format(Translations.JoinedLastContactError), new[] { "dateJoined, lastContactDate" });
                else if (lastContactDate < dateJoined)
                    yield return new ValidationResult("farm cannot be contacted about plots before they have joined the program", new[] { "lastContactDate" });
            }
        }
    }
    public class farmMetadata
    {
        [Display(Name = "Farm ID")]
        [DisplayFormat(DataFormatString = "{0:d5}")]
        public int farmId { get; set; }

        [Required(ErrorMessageResourceName = "RequiredError",ErrorMessageResourceType = typeof(Translations))]
        [Display(ResourceType=typeof(Translations),Name="Name")]
        public string name { get; set; }

        [Display(ResourceType=typeof(Translations),Name = "Address")]
        public string address { get; set; }

        [Display(ResourceType=typeof(Translations),Name = "Town")]
        public string town { get; set; }

        [Display(ResourceType=typeof(Translations),Name = "County")]
        public string county { get; set; }
        
        [RegularExpression(@"[a-zA-Z]{2}")]
        [Display(ResourceType=typeof(Translations),Name = "Province")]
        public string provinceCode { get; set; }

        [Display(ResourceType=typeof(Translations),Name = "PostalCode")]
        [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(Translations))]
        [RegularExpression(@"^[a-zA-Z]\d[a-zA-Z] ?\d[a-zA-Z]\d$",ErrorMessageResourceName = "PostalCodeError",ErrorMessageResourceType = typeof(Translations))]
        public string postalCode { get; set; }

        [Display(ResourceType=typeof(Translations),Name = "HomePhone")]
        public string homePhone { get; set; }

        [Display(ResourceType=typeof(Translations),Name = "CellPhone")]
        public string cellPhone { get; set; }

        [Display(ResourceType=typeof(Translations),Name = "Directions")]
        [DataType(DataType.MultilineText)]
        public string directions { get; set; }

        [Display(ResourceType=typeof(Translations),Name = "DateJoined")]
        [DisplayFormat(DataFormatString="{0:dd MMM yyyy}")]
        public Nullable<System.DateTime> dateJoined { get; set; }

        [Display(ResourceType=typeof(Translations),Name = "DateOfLastContact")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public Nullable<System.DateTime> lastContactDate { get; set; }
    }
}