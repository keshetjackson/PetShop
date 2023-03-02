using System.ComponentModel.DataAnnotations;

namespace PetShop.Models
{
	public class Comment
	{
		public int Id { get; set; }
		public int AnimalId { get; set; }
		[MaxLength(300)]
		[MinLength(1)]
		[Required(ErrorMessage = "please enter comment content")]
		public string Content { get;private set; }

		public Comment(int id,int animalId, string content)
		{
			Id= id;
			AnimalId= animalId;
			Content = content;
		}
		public Comment(int animalId, string content)
		{
			AnimalId = animalId;
			Content = content;
		}

		public Comment()
		{

		}
	}
}
