using PetShop.Models;

namespace PetShop.Repositories
{
    public interface IShopRepository
    {
        Task<List<Animal>> AnimalsGet();
        Task<List<Animal>> AnimalsMostCommented(int count);
        Task<List<Animal>> AnimalsByCategory(int categoryId);
        Task<List<Category>> Categories();
        Task<Animal> AnimalGet(int animalId);
        Task AnimalAdd(Animal animal);
        Task AnimalUpdate(Animal animal);
        Task AnimalDelete(int id);
        Task CommentAdd(Comment comment);
    }
}
