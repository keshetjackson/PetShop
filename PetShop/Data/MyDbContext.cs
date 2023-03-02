using Microsoft.EntityFrameworkCore;
using PetShop.Models;

namespace PetShop.Data
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {

        }

        public DbSet<Animal>? Animals { get; set; }
        public DbSet<Category>? Categories { get; set; }
        public DbSet<Comment>? Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                new Category(1,"mammals"),
                new Category(2,"fish"),
                new Category(3,"birds"),
                new Category(4,"reptiles"),
                new Category(5,"amphibians")
                );
            modelBuilder.Entity<Animal>().HasData(
                new Animal(1,"Pinky", 1, "https://hips.hearstapps.com/clv.h-cdn.co/assets/17/43/4000x2666/gallery-1509123692-cow-eating-grass.jpg?resize=1200:*", "this cow will make a good man happy someday", 1),
                new Animal(2,"shrek", 7, "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRTIdCBKYNQ_kRXh6RZ9N14GjqwkqBwsczl0g&usqp=CAU","shrek is friendly shark",2),
                new Animal(3, "huakin", 3, "https://www.ourbreathingplanet.com/wp-content/uploads/2019/01/Hoatzin.jpg", "very special bird called hoatzin",3),
                new Animal(4, "gary", 12, "https://www.edgeofexistence.org/wp-content/uploads/2017/11/Gavialis-gangeticus_Josh-More_CC-BY-NC-ND-2.0_10-1000x667.jpg", "special reptile called Gharial – Gavialis gangeticus", 4),
                new Animal(5, "suzy", 4, "https://www.australiangeographic.com.au/wp-content/uploads/2021/04/sunset-frog.jpg", "sunset frog", 5)
                );
            modelBuilder.Entity<Comment>().HasData(
                new Comment(1,1, "great milk great cow"),
                new Comment(2,2, "dude keep trying to bite me for no reason"),
                new Comment(3,1, "very cute but too loud"),
                new Comment(4,3, "high as a kite"),
                new Comment(5,5, "great chick too bad shes dating with gary"),
                new Comment(6,4, "evil creature he stole my lunch"),
                new Comment(7,3, "wont shutup about the apocalipse")
                );

        }

    }
}
