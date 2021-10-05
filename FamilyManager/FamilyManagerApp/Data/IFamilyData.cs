using System.Collections;
using System.Collections.Generic;
using Models;

namespace FamilyManagerApp.Data
{
    public interface IFamilyData
    {
        IList<Family> GetFamilies();
        IList<Person> GetPeople();
        void AddFamily(Family family);
        void AddPerson(Person person);
        void RemoveFamily(string streetName, int houseNumber);
        void RemovePerson(int personId);
        Family GetFamily(string streetName, int houseNumber);
        Person GetPerson(int personId);
        void UpdateFamily(Family family);
        void UpdatePerson(Person person);
    }
}