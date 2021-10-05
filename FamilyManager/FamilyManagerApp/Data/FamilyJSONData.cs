using System.Collections.Generic;
using Models;

namespace FamilyManagerApp.Data
{
    public class FamilyJSONData : IFamilyData
    {
        
        
        public IList<Family> GetFamilies()
        {
            throw new System.NotImplementedException();
        }

        public IList<Person> GetPeople()
        {
            throw new System.NotImplementedException();
        }

        public void AddFamily(Family family)
        {
            throw new System.NotImplementedException();
        }

        public void AddPerson(Person person)
        {
            throw new System.NotImplementedException();
        }

        public void RemoveFamily(string streetName, int houseNumber)
        {
            throw new System.NotImplementedException();
        }

        public void RemovePerson(int personId)
        {
            throw new System.NotImplementedException();
        }

        public Family GetFamily(string streetName, int houseNumber)
        {
            throw new System.NotImplementedException();
        }

        public Person GetPerson(int personId)
        {
            throw new System.NotImplementedException();
        }

        public void UpdateFamily(Family family)
        {
            throw new System.NotImplementedException();
        }

        public void UpdatePerson(Person person)
        {
            throw new System.NotImplementedException();
        }
    }
}