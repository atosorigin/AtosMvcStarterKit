using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using AtosOrigin.NetLibrary.Components.Core;

namespace Customer.Project.Domain.ValueObjects
{
    [ValueObject]
    public class Address 
    {
        public string Line1 { get; private set; }
        public string Line2 { get; private set; }
        public string City { get; private set; }
        public Country Country { get; private set; }
        public string PoBox { get; private set; }
        public string State { get; private set; }
        public Address(string city, Country country)
        {
            Check.Require(!string.IsNullOrEmpty(city));
            Check.Require(country != Country.Undefined);

            City = city;
            Country = country;
        }
        public Address(string line1, string line2, string city, Country country, string poBox, string state)
            : this(city, country)
        {
            Check.Require(!string.IsNullOrEmpty(city));
            Check.Require(country != Country.Undefined);

            Line1 = line1;
            Line2 = line2;
            PoBox = poBox;
            State = state;

            //City = city;
            //Country = country;
        }

        private Address()
        { }

        public static Address Empty
        {
            get
            {
                return new Address();
            }
        }

        public override int GetHashCode()
        {
            return string.Format(CultureInfo.InvariantCulture, "{0}{1}{2}{3}{4}",
                                 Line1, Line2, City, Country, PoBox).GetHashCode();
        }
        public override bool Equals(object obj)
        {
            return Equals(obj as Address);
        }
        public bool Equals(Address other)
        {
            return other != null && this.GetHashCode() == other.GetHashCode();
        }


        //add this code to class ThreeDPoint as defined previously
        //
        public static bool operator ==(Address a, Address b)
        {
            // If both are null, or both are same instance, return true.
            if (System.Object.ReferenceEquals(a, b))
            {
                return true;
            }

            // If one is null, but not both, return false.
            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            // Return true if the fields match:
            return a.Equals(b);
        }

        public static bool operator !=(Address a, Address b)
        {
            return !(a == b);
        }
    }
}
