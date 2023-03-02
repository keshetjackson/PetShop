using System.ComponentModel.DataAnnotations;

namespace PetShop.Models
{
	public class Animal
	{
		public int Id { get;private  set; }
		[MaxLength(35)]
		[MinLength(1)]
		[DataType(DataType.Text)]
        [RegularExpression("[A-Za-z\\s]{2,30}$", ErrorMessage = "Enter valid name contain 2-30 letters only")]
        [Required(ErrorMessage = "Enter name")]
		public string Name { get; set; }
		[Range(0,2500)]

        [Required(ErrorMessage = "age must be numbers between 0 - 2500(deep sea sponge)")]
        public int Age { get; set; }
		//[RegularExpression("@(https?:)?//?[^'\"<>]+?\\.(jpg|jpeg|gif|png)@", ErrorMessage = "please enter valid url")]
		public string? Picture { get; set; }
        [RegularExpression(".{3,200}$", ErrorMessage = "Please enter between 3-200 charecters")]
        public string? Description { get; set; }
		[Required(ErrorMessage = "enter category id")]
		public int CategoryId { get; set; }

		public virtual ICollection<Comment>? Comments { get; set; }

		public Animal(int id, string name, int age, string picture, string description, int categoryId)
		{
			Id = id;
			Name = name;
			Age = age;
			Picture = picture;
			Description = description;
			CategoryId = categoryId;
		}

		public Animal() { }

        public Animal( string name, int age, string picture, string description, int categoryId)
        {
            Name = name;
            Age = age;
            Picture = picture;
            Description = description;
            CategoryId = categoryId;
        }
    }
}
