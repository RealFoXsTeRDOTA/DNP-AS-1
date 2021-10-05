using System.Collections;
using System.Collections.Generic;
using Models;

namespace FamilyManagerApp.Data
{
    public interface IFamilyData
    {
        IList<Family> GetFamilies();
        IList<Person> GetPeople();
        IList<Pet> GetPets();
        void AddFamily(Family family);
        void AddPerson(Person person, Family family);
        void AddPet(Pet pet, Family family);
        void AddPet(Pet pet, Child child);

        void RemoveFamily(string streetName, int houseNumber);
        void RemovePerson(int personId);
        void RemovePet(int petId);
        Family GetFamily(string streetName, int houseNumber);
        Person GetPerson(int personId);
        Pet GetPet(int petId);
        void UpdateFamily(Family family);
        void UpdatePerson(Person person);
        void UpdatePet(Pet pet);
        ISet<string> GetEyeColors();
        ISet<string> GetHairColors();
    }
}