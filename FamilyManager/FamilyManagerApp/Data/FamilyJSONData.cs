using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Models;

namespace FamilyManagerApp.Data
{
    public class FamilyJSONData : IFamilyData
    {
        private string FamilyFile = "family.json";
        private IList<Family> Families;
        private IList<Person> People;
        public FamilyJSONData()
        {
            if (!File.Exists(FamilyFile))
            {
                Seed();
                WriteFamiliesToFile();
            }
            else
            {
                string content = File.ReadAllText(FamilyFile);
                Families = JsonSerializer.Deserialize<List<Family>>(content);
                foreach (Family f in Families)
                {
                    foreach (Adult adult in f.Adults)
                        People.Add(adult);
                    foreach (Child child in f.Children)
                        People.Add(child);
                }
            }
        }
        
        public IList<Family> GetFamilies()
        {
            List<Family> fam = new List<Family>(Families);
            return fam;
        }

        public IList<Person> GetPeople()
        {
            List<Person> people = new List<Person>(People);
            return people;
        }

        public void AddFamily(Family family)
        {
            Family fam = Families.FirstOrDefault(f => f.HouseNumber == family.HouseNumber
                                                      && f.StreetName.Equals(family.StreetName));
            if (fam == null)
                Families.Add(family);
            WriteFamiliesToFile();
        }

        public void AddPerson(Person person)
        {
            int max = People.Max(p => p.Id);
            person.Id = (++max);
            People.Add(person);
            WriteFamiliesToFile();
        }

        public void RemoveFamily(string streetName, int houseNumber)
        {
            Family family = Families.First(f => f.StreetName.Equals(streetName) 
                                                && f.HouseNumber == houseNumber);
            Families.Remove(family);
            WriteFamiliesToFile();
        }

        public void RemovePerson(int personId)
        {
            Person person = People.First(p => p.Id == personId);
            People.Remove(person);
            WriteFamiliesToFile();
        }

        public Family GetFamily(string streetName, int houseNumber)
        {
            return Families.FirstOrDefault(f => f.StreetName.Equals(streetName) 
                                                && f.HouseNumber == houseNumber);
        }

        public Person GetPerson(int personId)
        {
            return People.FirstOrDefault(p => p.Id == personId);
        }

        public void UpdateFamily(Family family)
        {
            Family fam = Families.First(f => f.HouseNumber == family.HouseNumber
                                             && f.StreetName.Equals(family.StreetName));
            //TODO Update stuff
            WriteFamiliesToFile();
        }

        public void UpdatePerson(Person person)
        {
            Person per = People.First(p => p.Id == person.Id);
            // TODO update stuff
            WriteFamiliesToFile();
        }

        public void WriteFamiliesToFile()
        {
            string familiesAsJson = JsonSerializer.Serialize(Families);
            File.WriteAllText(FamilyFile, familiesAsJson);
        }

        public void Seed()
        {
            
        }
    }
}