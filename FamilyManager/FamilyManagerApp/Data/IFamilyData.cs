using System.Collections;
using System.Collections.Generic;
using Models;

namespace FamilyManagerApp.Data
{
    public interface IFamilyData
    {
        IList<Family> GetFamilies();
        IList<Person> GetPeople();
        IList<Adult> GetAdults();
        IList<Child> GetChildren();
        IList<Pet> GetPets();
        void AddFamily(Family family);
        void AddAdult(Adult adult, Family family);
        void AddChild(Child child, Family family);
        void AddPet(Pet pet, Family family);
        void AddPet(Pet pet, Family family, Child child);

        void RemoveFamily(string streetName, int houseNumber);
        void RemovePerson(int personId);
        void RemovePet(int petId);
        Family GetFamily(string streetName, int houseNumber);
        Person GetPerson(int id);
        Person GetPersonByIdFirstLastName(int id, string firstname, string lastname);
        Pet GetPet(int petId);
        void UpdateFamily(Family family);
        void UpdatePerson(Person person);
        void UpdatePet(Pet pet);
        IList<string> GetEyeColors();
        IList<string> GetHairColors();
    }
}