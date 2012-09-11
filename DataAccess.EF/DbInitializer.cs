using System;
using System.Collections.Generic;
using System.Data.Entity;
using Customer.Project.Domain.Entities;
using Customer.Project.Domain.ValueObjects;

namespace Customer.Project.DataAccessEF
{
    public class DbInitializer : DropCreateDatabaseIfModelChanges<MvcContext>
    {
        /// <summary>
        /// Fill your Entity with data
        /// </summary>
        /// <param name="context"></param>
        protected override void Seed(MvcContext context)
        {
            base.Seed(context);

            //             context.Database.ExecuteSqlCommand("ALTER TABLE [User] ADD CONSTRAINT uc_Email UNIQUE(Email)");

            //SeedPeople(context);
        }
        //protected void SeedPeople(MvcContext context)
        //{
        //    var people = PeopleTestData();
        //    people.ForEach(p => context.People.Add(p));
        //    context.SaveChanges();
        //}

        //public List<Person> PeopleTestData()
        //{
        //    var people = new List<Person>()
        //                     {
        //                         new Person() { IdOfEntity  = 1, FirstName = "Alexander", LastName = "van Trijffel", Address = new Address("Utrecht", new Country("NL", "The Netherlands")), DateOfBirth = new DateTime(2012, 07, 18), Email = "alexander.vantrijffel@atos.net", NumberOfCats = 0, PhoneNumber = "0101001000", WebSite = "http://www.atos.net" },
        //                         new Person() { IdOfEntity  = 2, FirstName = "Desiderius", LastName = "Erasmus", Address = new Address("Rotterdam", new Country("NL", "The Netherlands")), DateOfBirth = new DateTime(2012, 07, 18), Email = "desiderius.erasmus@atos.net", NumberOfCats = 3, PhoneNumber = "0101005000", WebSite = "http://www.atos.net" },
        //                     };
        //    return people;
        //}

    }
}
