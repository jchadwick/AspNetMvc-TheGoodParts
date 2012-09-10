using System;
using System.Data.Entity;
using Common;
using Common.DataAccess;

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
            private readonly Random _random;

            public AuctionContextInitializer()
            {
                _random = new Random();
            }

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

                AddAuction(context, new Auction
                                         {
                                             IsFeatured = true,
                                             Category = electronics,
                                             Title = "Apple MacBook Pro 13\"",
                                             Description = "The Apple MacBook Pro with a 13\" display",
                                             ImageUrl = "/content/auction-images/mbp.jpg",
                                             ThumbnailUrl = "/content/auction-images/mbp_thumb.jpg",
                                         });
                AddAuction(context, new Auction
                                         {
                                             IsFeatured = true,
                                             Category = electronics,
                                             Title = "Apple iPad 3",
                                             Description = "The third generation Apple iPad",
                                             ImageUrl = "/content/auction-images/ipad.jpg",
                                             ThumbnailUrl = "/content/auction-images/ipad_thumb.jpg",
                                         });
                AddAuction(context, new Auction
                                         {
                                             IsFeatured = true,
                                             Category = electronics,
                                             Title = "Barnes & Noble Nook eReader",
                                             Description = "The Barnes & Noble Nook eReader",
                                             ImageUrl = "/content/auction-images/nook.jpg",
                                             ThumbnailUrl = "/content/auction-images/nook_thumb.jpg",
                                         });
                context.SaveChanges();
            }

            private void AddAuction(AuctionContext context, Auction auction)
            {
                auction.StartingPrice = _random.Next(1, 200);

                auction.StartTime = DateTime.Now
                    .AddDays(_random.Next(-4, 0))
                    .AddHours(_random.Next(0, 23))
                    .AddMinutes(_random.Next(0, 59));

                auction.EndTime = auction.StartTime.AddDays(_random.Next(4, 7));

                context.Auctions.Add(auction);
            }
        }
    }
}