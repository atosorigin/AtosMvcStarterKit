using System;

namespace Customer.Project.Domain.Entities
{
    public class Product :IEntityId
    {
        // All the properties are marked virtual; this is because NHibernate creates "proxies" of 
        // your entities at run time to allow for lazy loading, and for it to do that it needs 
        // to be able to override the properties.
        public virtual long IdOfEntity { get; set; }
        public virtual String Name { get; set; }
        public virtual decimal RawPrice { get; set; }
        public virtual decimal GetPriceWithTax(ITaxCalculator calculator)
        {
            decimal price = 0;
            if (calculator != null)
            {
             price = calculator.GetTax(RawPrice) + RawPrice;
            }
            return price;
        }
    }

    public interface ITaxCalculator
    {
        decimal GetTax(decimal rawPrice);
    }
}
