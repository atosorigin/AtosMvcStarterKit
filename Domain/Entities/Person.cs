using System;
using System.ComponentModel.DataAnnotations;
using Customer.Project.Domain.ValueObjects;
using DataAnnotationsExtensions;

namespace Customer.Project.Domain.Entities
{
    /// <summary>
    /// Example business object class
    /// </summary>
    public class Person : IEntityId
    {
        // DataAnnotationsExtensions - Add Client side and server side validation, next to the browser's HTML5 validations 
        // http://weblogs.asp.net/srkirkland/archive/2011/02/23/introducing-data-annotations-extensions.aspx
        // http://dataannotationsextensions.org/
        /*
         * 
        [Email]     -   [DataType(DataType.EmailAddress)]
        [Url]       -   [DataType(DataType.Url)]
        [Digits]    -   [DataType("Number")] //On Opera number uses a spinbox, so this will only work with integers
        
        [Numeric]   -   numbers, decimals
        [Integer]
        [Year]

        [FileExtensions("png,jpg,jpeg,gif")]
        [EqualTo("otherProperty")] 
        [CreditCard]        
        [Cuit]      -   (national identification number)
         
         * Redundant:
        [Date]      -   [DataType(DataType.Date)] // only use this attribute for non-DateTime properties otherwise a redundant validation is added. For DateTime properties use [DataType(DataType.Date)] as the unobtrusive validation is performed already without DataAnnotationsExtensions
        [Min(0.5)]
        [Max(1.5)]         
        */

        // Supported HTML5 types
        // Note: [DataType("TemplateName")] and Mvc's [UIHint("TemplateName")] are equivalent for our purposes 
        /*
        [DataType(DataType.PhoneNumber)]
        [DataType("Search")]
        [DataType("DateTime-Local")]
        [DataType(DataType.Time)]
        [DataType("Month")]
        [DataType("Week")]        
        [DataType("Color")]
        */
        
        [Key]
        public virtual long IdOfEntity { get; set; }
        [Required]
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        [Email]
        [StringLength(256)]
        public string Email { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }        
        
        [DataType(DataType.Date)]
        [Display(Name = "Birthday")]
        public virtual DateTime? DateOfBirth { get; set; }
        public virtual Address Address { get; set; }

        [Display(Name = "Member of team")]
        public int TeamId { get; set; }

        // virtual for lazy loading
        public virtual Team Team { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Url]
        public string WebSite { get; set; }

        [Numeric]
        [Range(0, 50)]
        public int NumberOfCats { get; set; }

        public Person()
        {
        }

        // Always override ToString to display the name of the instance 
        public override string ToString()
        {
            return string.Format("{0} {1}", FirstName, LastName);
        }
    }
}
