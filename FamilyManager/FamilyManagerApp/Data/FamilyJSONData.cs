using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using Models;

namespace FamilyManagerApp.Data
{
    public class FamilyJSONData : IFamilyData {
        private string FamilyFile = "Data/families.json";
        private IList<Family> Families;
        private IList<Person> People;
        private IList<Pet> Pets;

        public FamilyJSONData() {
            if (!File.Exists(FamilyFile)) {
                Seed();
                WriteFamiliesToFile();
            }
            else {
                string content = File.ReadAllText(FamilyFile);
                Families = JsonSerializer.Deserialize<List<Family>>(content);
                People = new List<Person>();
                Pets = new List<Pet>();
                foreach (Family f in Families)
                {
                    foreach (Adult adult in f.Adults)
                        People.Add(adult);
                    foreach (Child child in f.Children)
                        People.Add(child);
                    foreach (Pet pet in f.Pets) 
                        Pets.Add(pet);
                }
            }
        }

        public IList<Family> GetFamilies() {
            List<Family> fam = new List<Family>(Families);
            return fam;
        }

        public IList<Person> GetPeople() {
            List<Person> people = new List<Person>(People);
            return people;
        }

        public IList<Pet> GetPets() {
            List<Pet> pets = new List<Pet>(Pets);
            return pets;
        }

        public void AddFamily(Family family) {
            Family fam = Families.FirstOrDefault(f => f.HouseNumber == family.HouseNumber
                                                      && f.StreetName.Equals(family.StreetName));
            if (fam == null)
                Families.Add(family);
            else 
                throw new Exception("The family already exists");
            WriteFamiliesToFile();
        }

        public void AddPerson(Person person) {
            int max = People.Max(p => p.Id);
            person.Id = (++max);
            People.Add(person);
            WriteFamiliesToFile();
        }

        public void AddPet(Pet pet) {
            int max = Pets.Max(p => p.Id);
            pet.Id = (++max);
            Pets.Add(pet);
            WriteFamiliesToFile();
        }

        public void RemoveFamily(string streetName, int houseNumber) {
            Family family = Families.First(f => f.StreetName.Equals(streetName)
                                                && f.HouseNumber == houseNumber);
            Families.Remove(family);
            WriteFamiliesToFile();
        }

        public void RemovePerson(int personId) {
            Person person = People.First(p => p.Id == personId);
            People.Remove(person);
            WriteFamiliesToFile();
        }

        public void RemovePet(int petId) {
            Pet pet = Pets.First(p => p.Id == petId);
            Pets.Remove(pet);
            WriteFamiliesToFile();
        }

        public Family GetFamily(string streetName, int houseNumber) {
            return Families.FirstOrDefault(f => f.StreetName.Equals(streetName)
                                                && f.HouseNumber == houseNumber);
        }

        public Person GetPerson(int personId) {
            return People.FirstOrDefault(p => p.Id == personId);
        }

        public Pet GetPet(int petId) {
            return Pets.FirstOrDefault(p => p.Id == petId);
        }

        public void UpdateFamily(Family family) {
            Family fam = Families.First(f => f.HouseNumber == family.HouseNumber
                                             && f.StreetName.Equals(family.StreetName));
            //TODO Update stuff
            WriteFamiliesToFile();
        }

        public void UpdatePerson(Person person) {
            Person per = People.First(p => p.Id == person.Id);
            // TODO update stuff
            WriteFamiliesToFile();
        }

        public void UpdatePet(Pet pet) {
            Pet p = Pets.First(p => p.Id == pet.Id);
            // TODO update stuff
            WriteFamiliesToFile();
        }

        public ISet<string> GetEyeColors() {
            ISet<string> output = new HashSet<string>();
            foreach (Person person in People) {
                output.Add(person.EyeColor);
            }

            return output;
        }

        public ISet<string> GetHairColors() {
            ISet<string> output = new HashSet<string>();
            foreach (Person person in People) {
                output.Add(person.HairColor);
            }

            return output;
        }

        private void WriteFamiliesToFile() {
            string familiesAsJson = JsonSerializer.Serialize(Families);
            File.WriteAllText(FamilyFile, familiesAsJson);
        }

        private void Seed() {
            Family[] f = {
                new Family() {
                    Adults = {
                        new Adult() {
                            Age = 35,
                            EyeColor = "brown",
                            FirstName = "Adriana",
                            HairColor = "red",
                            Height = 175,
                            Id = 1,
                            JobTitle = new Job() {
                                JobTitle = "Bartender",
                                Salary = 12000
                            },
                            LastName = "Grecea",
                            Sex = "Feminine",
                            Weight = 70
                        }
                    },
                    Children = {
                        new Child() {
                            Age = 12,
                            EyeColor = "blue",
                            FirstName = "Morten",
                            HairColor = "Green",
                            Height = 180,
                            Id = 2,
                            Interests = {
                                new Interest() {
                                    Type = "Jogging",
                                    Description = "Running in the morning"
                                }
                            },
                            LastName = "Hansen",
                            Sex = "Masculine"
                        }
                    },
                    HouseNumber = 23,
                    StreetName = "Sundvej"
                }
            };
            Families = f.ToList();
        }
    }
}