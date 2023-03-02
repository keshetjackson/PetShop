using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Castle.Components.DictionaryAdapter.Xml;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Presentation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PetShop.Data;
using PetShop.Models;
using PetShop.Repositories;

namespace PetShop.Controllers
{
    public class AdminController : Controller
    {
        private readonly IShopRepository _shopRepository;
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<AdminController> _logger;

        //The constructor of the class AdminController that initializes the database context and logger.
        public AdminController(IShopRepository repository, IWebHostEnvironment environment, ILogger<AdminController> logger)
        {
            _shopRepository = repository;
            _environment = environment;
            _logger = logger;
        }

        // GET: Admin

        //An asynchronous method that returns the view to display a list of all animals in the database.


        public async Task<IActionResult> Index()
        {
            try
            {
                return _shopRepository.AnimalsGet != null ?
                         View(await _shopRepository.AnimalsGet()) :
                         Problem("Entity set 'MyDbContext.Animals'  is null.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return Error();
            }
             
        }

        // GET: Admin/Details/5
        //An asynchronous method that returns the details of a particular animal based on the specified animal ID.


        public async Task<IActionResult> Details(int id)
        {
            try
            {
                _logger.LogInformation($"details request for {id}");
                if (id == null || _shopRepository.AnimalGet(id) == null)
                {
                    return NotFound();
                }

                var animal = await _shopRepository.AnimalGet(id);

                return View(animal);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return Error();
            }
            
        }

        // GET: Admin/Create
        //An asynchronous method that returns the view to create a new animal category.


        public async Task<IActionResult> Create()
        {
            try
            {
                ViewBag.Categories = await _shopRepository.Categories();
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return Error();
            }
            
        }

        // POST: Admin/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //An asynchronous method that handles the creation of a new animal category in the database. It receives an Animal object and an image file.


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Animal animal, IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    return Content("File not selected");
                }

                var path = Path.Combine(_environment.WebRootPath, "images", animal.Name+animal.Id);
                await using (FileStream stream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                    stream.Close();
                }
                var pic = $"/images/{file.FileName}";

                animal.Picture = pic;
                if (ModelState.IsValid)
                {
                    animal.Picture = $"/images/{file.FileName}";
                    _shopRepository?.AnimalAdd(animal);

                    return RedirectToAction(nameof(Index));
                }

                return View(animal);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return Error();
            }
           
        }

        // GET: Admin/Edit/5
        //An asynchronous method that returns the view to edit an existing animal category.


        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                if (id == null || _shopRepository.AnimalGet(id) == null)
                {
                    return NotFound();
                }

                var animal = await _shopRepository.AnimalGet(id);
                if (animal == null)
                {
                    return NotFound();
                }
                ViewBag.Categories = await _shopRepository.Categories();
                return View(animal);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return Error();
            }
           
        }

        // POST: Admin/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //An asynchronous method that updates an existing animal category in the database. It receives an Animal object, an image file, and an animal ID.


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Animal animal, IFormFile file,int id)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    return Content("File not selected");
                }
               
                var path = Path.Combine(_environment.WebRootPath, "images", animal.Name + animal.Id);
                await using (FileStream stream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                    stream.Close();
                }
                var pic = $"/images/{file.FileName}";

                Animal updated = new Animal(id, animal.Name, animal.Age, pic, animal.Description, animal.CategoryId);
                if (ModelState.IsValid)
                {
                    try
                    {
                        _shopRepository?.AnimalUpdate(updated);
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (_shopRepository.AnimalGet(updated.Id) == null)
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return Error();
            }
           
        }

        // GET: Admin/Delete/5
        //An asynchronous method that returns the view to delete an existing animal category.




        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                if (id == null || _shopRepository.AnimalGet(id) == null)
                {
                    return NotFound();
                }

                var animal = await _shopRepository.AnimalGet(id);
                if (animal == null)
                {
                    return NotFound();
                }

                return View(animal);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return Error();
            }
           
        }

        // POST: Admin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                if (_shopRepository.AnimalGet(id) == null)
                {
                    return Problem("Entity set 'MyDbContext.Animals'  is null.");
                }
                var animal = await _shopRepository.AnimalGet(id);
                if (animal != null)
                {
                    _shopRepository.AnimalDelete(id);
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {

                _logger.LogError(ex.StackTrace);
                return Error();
            }

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public void HandleImage()
        {

        }
    }
}
