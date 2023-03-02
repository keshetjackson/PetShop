using Castle.Components.DictionaryAdapter.Xml;
using DocumentFormat.OpenXml.Presentation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PetShop.Data;
using PetShop.Models;
using PetShop.Repositories;
using System.Diagnostics;
using System.Drawing.Printing;
using Comment = PetShop.Models.Comment;

namespace PetShop.Controllers
{
    public class HomeController : Controller
    {
        // Dependencies required by the controller
        private readonly IShopRepository _shopRepository;
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<HomeController> _logger;

        // Constructor to inject dependencies
        public HomeController(IShopRepository repository, IWebHostEnvironment environment, ILogger<HomeController> logger)
        {
            _shopRepository = repository;
            _environment = environment;
            _logger = logger;
        }

        // Action to display the most commented animals
        public async Task<IActionResult> Index()
        {
            try
            {
                // Get the most commented animals from the repository
                var animals = await _shopRepository.AnimalsMostCommented(2);

                // Return the view with the animals
                return View(animals);
            }
            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError(ex, $"exception accured in index {ex.GetBaseException().Message}" );

                // Return the error view
                return Error();
            }
        }

        // Action to display the catalog of animals
        public async Task<IActionResult> Catalog(int categoryId = 0)
        {
            IEnumerable<Animal> animals;

            try
            {
                if (categoryId == 0)
                {
                    // If no category ID was specified, get all animals
                    animals = await _shopRepository.AnimalsGet();

                    // Set the category name to "Select Category"
                    ViewBag.CategoryName = "Select Category";
                }
                else
                {
                    // If a category ID was specified, get animals in that category
                    animals = await _shopRepository.AnimalsByCategory(categoryId);

                    // Set the category name to the name of the category
                    ViewBag.CategoryName = _shopRepository.Categories().Result.First(c => c.Id == categoryId).Name;
                }

                // Set the category ID and list of categories
                ViewBag.CategoryId = categoryId;
                ViewBag.Categories = await _shopRepository.Categories();

                // Return the view with the animals
                return View(animals);
            }
            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError(ex.ToString());

                // Return the error view
                return Error();
            }
        }

        // Action to add a comment to an animal
        public async Task<IActionResult> AddCommentAsync(int animalId, string content)
        {
            // Get the URL of the previous page
            var previousUrl = Request.Headers["Referer"].ToString();
            if (content == null) return Redirect(previousUrl);
            try
            {
                if(content.Length <= 300 && content.Length >0)
                {
                    // Create a new comment object
                    var c = new Comment(animalId, content);

                    // Add the comment to the repository
                    await _shopRepository.CommentAdd(c);
                }
                
                var animal = _shopRepository.AnimalGet(animalId);

                // Redirect to the catalog page with the most commented animals
                return Redirect(previousUrl);
            }
            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError(ex.ToString());

                // Return the error view
                return Error();
            }
        }

        // Action to display the privacy policy page
        public IActionResult Privacy()
        {
            return View();
        }

        // Action to display the error page
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
