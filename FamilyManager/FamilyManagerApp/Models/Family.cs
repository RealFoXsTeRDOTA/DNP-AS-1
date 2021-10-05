using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Models {
public class Family {
    
    //public int Id { get; set; }
    public string Name { get; set; }
    public string StreetName { get; set; }
    public int HouseNumber{ get; set; }
    public List<Adult> Adults { get; set; }
    public List<Child> Children{ get; set; }
    public List<Pet> Pets{ get; set; }

    public Family() {
        Adults = new List<Adult>();     
    }

    public int GetNumberOfMembers() {
        return Adults.Capacity + Children.Capacity;
    }

    public bool HasPets() {
        return Pets.Any();
    }

    public string GetUniqueKey() {
        return $"{StreetName} {HouseNumber}";
    }
}


}