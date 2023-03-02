using System.ComponentModel.DataAnnotations;

namespace PetShop.Models
{
	enum ECategories
	{
        mammals, fish, birds, reptiles,amphibians
    }
	public class Category
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public virtual ICollection<Animal>? Animals { get; set; }

		public Category(int id,string name)
		{
			Id = id;
			Name = name;
		}

	}
}
