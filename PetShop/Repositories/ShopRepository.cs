using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.EntityFrameworkCore;
using PetShop.Data;
using PetShop.Models;

namespace PetShop.Repositories
{
    // Repository for managing pet shop data
    public class ShopRepository : IShopRepository
    {
        private readonly MyDbContext _dbContext;

        // Constructor for creating a new instance of ShopRepository
        public ShopRepository(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Add a new animal to the database
        public async Task AnimalAdd(Animal animal)
        {
            await _dbContext.Animals.AddAsync(animal);
            await _dbContext.SaveChangesAsync();
        }

        // Delete an animal from the database by ID
        public async Task AnimalDelete(int id)
        {
            Animal animalToRemove = await _dbContext.Animals
                                            .FirstOrDefaultAsync(animal => animal.Id == id);
            if (animalToRemove != null)
            {
                _dbContext.Animals.Remove(animalToRemove);
                await _dbContext.SaveChangesAsync();
            }
        }

        // Get an animal from the database by ID
        public async Task<Animal> AnimalGet(int animalId)
        {
            var animal = await _dbContext?.Animals.FirstOrDefaultAsync(Animal => Animal.Id == animalId);
            return animal;
        }

        // Get a list of animals from the database by category ID
        public async Task<List<Animal>> AnimalsByCategory(int categoryId)
        {
            var animals = await _dbContext.Animals
                        .Where(animal => animal.CategoryId == categoryId)
                        .ToListAsync();
            return animals;
        }

        // Get a list of all animals from the database
        public async Task<List<Animal>> AnimalsGet()
        {
            var animals = await _dbContext.Animals.ToListAsync();
            return animals;
        }

        // Get a list of the most commented animals from the database
        public async Task<List<Animal>> AnimalsMostCommented(int count)
        {
            var animals = await _dbContext.Animals
           .Where(a => a.Comments != null)
           .OrderByDescending(a => a.Comments.Count)
           .Take(count)
           .ToListAsync();
            return animals;
        }

        // Update an animal in the database
        public async Task AnimalUpdate(Animal animal)
        {
            var animalToUpdate = await _dbContext.Animals.FindAsync(animal.Id);
            if (animalToUpdate != null)
            {
                _dbContext.Entry(animalToUpdate).CurrentValues.SetValues(animal);
                await _dbContext.SaveChangesAsync();
            }
        }

        // Get a list of all categories from the database
        public async Task<List<Category>> Categories()
        {
            return await _dbContext.Categories.ToListAsync();
        }

        // Add a new comment to the database
        public async Task CommentAdd(Comment comment)
        {
            _dbContext.Comments.AddAsync(comment);
            await _dbContext.SaveChangesAsync();
        }
    }
}
