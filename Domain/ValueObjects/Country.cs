using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AtosOrigin.NetLibrary.Components.Core;

namespace Customer.Project.Domain.ValueObjects
{
    /// <summary>
    /// Country value object
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2224:OverrideEqualsOnOverloadingOperatorEquals"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1046:DoNotOverloadOperatorEqualsOnReferenceTypes"), ValueObject]
    public class Country : ValueObjectBase<Country>
    {
        private static Dictionary<string, Country> _allCountries = null;

        public static Dictionary<string, Country> AllCountries
        {
            get
            {
                if (_allCountries == null)
                {
                    _allCountries = new Dictionary<string, Country>
                                        {
                                              {"NL", new Country("NL", "Netherlands")}
                                            , {"US", new Country("US", "United States")}
                                        };
                }
                return _allCountries;
            }
        }

        public override string ToString()
        {
            return Name;
        }
        public static Country Undefined
        {
            get { return new Country("--", ""); }
        }
        public string Code { get; private set; }
        public string Name { get; private set; }

        public Country(string code, string name)
        {
            Check.Require(!string.IsNullOrEmpty(code));
            Check.Require(!string.IsNullOrEmpty(name));
            Check.Require(code == "--" || !string.IsNullOrEmpty(name));
            Check.Require(name.Length < 30);
            Check.Require(code.Length == 2);
            Name = name;
            Code = code;
        }

        // For NHibernate
        private Country()
        { }


        public override int GetHashCode()
        {
            return Code.GetHashCode();
        }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates")]
        public static bool operator ==(Country a, Country b)
        {
            return OperatorEquals(a, b);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates")]
        public static bool operator !=(Country a, Country b)
        {
            return !OperatorEquals(a, b);
        }
    }
}
