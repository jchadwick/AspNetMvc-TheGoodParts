using System;
using System.Data.Entity;
using Common;
using Common.DataAccess;
using Website.Models;

namespace Website.App_Start
{
    public class DataContextConfig
    {
        public static void InitializeDataContext()
        {
            Database.SetInitializer(new AuctionContextInitializer());
        }

        class AuctionContextInitializer
//            : DropCreateDatabaseIfModelChanges<AuctionContext>
            : DropCreateDatabaseAlways<AuctionContext>
        {
            protected override void Seed(AuctionContext context)
            {
                var collectibles = new Category { Name = "Collectibles" };
                var electronics = new Category { Name = "Electronics" };
                var homeAndGarden = new Category { Name = "Home / Garden", Key = "home_and_garden" };
                var toys = new Category { Name = "Toys" };

                context.Categories.Add(collectibles);
                context.Categories.Add(electronics);
                context.Categories.Add(homeAndGarden);
                context.Categories.Add(toys);
                context.SaveChanges();

                context.Auctions.Add(new Auction
                                         {
                                             IsFeatured = true,
                                             Category = electronics,
                                             Title = "Apple MacBook Pro 13\"",
                                             Description = "The Apple MacBook Pro with a 13\" display",
                                             StartingPrice = 200,
                                             StartTime = DateTime.Now,
                                             EndTime = DateTime.Now.AddDays(7),
                                             ImageUrl = "/content/auction-images/mbp.jpg",
                                             ThumbnailUrl = "/content/auction-images/mbp_thumb.jpg",
                                         });
                context.Auctions.Add(new Auction
                                         {
                                             IsFeatured = true,
                                             Category = electronics,
                                             Title = "Apple iPad 3",
                                             Description = "The third generation Apple iPad",
                                             StartingPrice = 50,
                                             StartTime = DateTime.Now,
                                             EndTime = DateTime.Now.AddDays(7),
                                             ImageUrl = "/content/auction-images/ipad.jpg",
                                             ThumbnailUrl = "/content/auction-images/ipad_thumb.jpg",
                                         });
                context.Auctions.Add(new Auction
                                         {
                                             IsFeatured = true,
                                             Category = electronics,
                                             Title = "Barnes & Noble Nook eReader",
                                             Description = "The Barnes & Noble Nook eReader",
                                             StartingPrice = 100,
                                             StartTime = DateTime.Now,
                                             EndTime = DateTime.Now.AddDays(7),
                                             ImageUrl = "/content/auction-images/nook.jpg",
                                             ThumbnailUrl = "/content/auction-images/nook_thumb.jpg",
                                         });
                context.SaveChanges();
            }
        }
    }
}