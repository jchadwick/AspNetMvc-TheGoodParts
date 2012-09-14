using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using Common;
using Common.DataAccess;
using WebMatrix.WebData;
using Website.Controllers;

namespace Website.App_Start
{
    public class DatabaseConfig
    {
        public static void InitializeDatabases()
        {
            new SimpleMembershipInitializer().Initialize();

            Database.SetInitializer(new DataContextInitializer(new MembershipContext()));
        }

        private class SimpleMembershipInitializer
        {
            public void Initialize()
            {
                Database.SetInitializer<MembershipContext>(null);

                try
                {
                    using (var context = new MembershipContext())
                    {
                        if (!context.Database.Exists())
                        {
                            // Create the SimpleMembership database without Entity Framework migration schema
                            ((IObjectContextAdapter)context).ObjectContext.CreateDatabase();
                        }
                    }

                    WebSecurity.InitializeDatabaseConnection("Membership", "UserProfiles", "UserId", "Username", autoCreateTables: true);
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException("The ASP.NET Simple Membership database could not be initialized. For more information, please see http://go.microsoft.com/fwlink/?LinkId=256588", ex);
                }
            }
        }

        class DataContextInitializer
//            : DropCreateDatabaseIfModelChanges<DataContext>
            : DropCreateDatabaseAlways<DataContext>
        {
            private readonly MembershipContext _membership;
            private readonly Random _random;
            private UserProfile[] _users;

            public DataContextInitializer(MembershipContext membership)
            {
                _membership = membership;
                _random = new Random();
            }

            protected override void Seed(DataContext context)
            {
                // Users
                _users = new[] {
                    new UserProfile { Username = "homer_simpson", FeebackPercentage = 80 },
                    new UserProfile { Username = "oldblueeyes213", FeebackPercentage = 90 },
                    new UserProfile { Username = "bill-gates", FeebackPercentage = 96 },
                    new UserProfile { Username = "steve.jobs", FeebackPercentage = 94 },
                };

                foreach (var user in _users)
                {
                    _membership.UserProfiles.Add(user);
                }
                _membership.SaveChanges();

                // Categories
                var collectibles = new Category { Name = "Collectibles" };
                var electronics = new Category { Name = "Electronics" };
                var homeAndGarden = new Category { Name = "Home / Garden", Key = "home_and_garden" };
                var toys = new Category { Name = "Toys" };

                context.Categories.Add(collectibles);
                context.Categories.Add(electronics);
                context.Categories.Add(homeAndGarden);
                context.Categories.Add(toys);
                context.SaveChanges();


                // Auctions
                AddAuction(context, new Auction
                                         {
                                             IsFeatured = true,
                                             Category = collectibles,
                                             Title = "Hand Painted 24x18 Velvet Elvis",
                                             Description = "This is a fantastic price on a New Hand Painted 24x18 Velvet Elvis Presley White Jump Suit w/Guitar Painting.",
                                             ImageUrl = "/content/auction-images/velvet-elvis.jpg",
                                             ThumbnailUrl = "/content/auction-images/velvet-elvis_thumb.jpg",
                                         });


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


                AddAuction(context, new Auction
                                         {
                                             IsFeatured = true,
                                             Category = homeAndGarden,
                                             StartingPrice = 120,
                                             Title = "Breville BES860XL Barista Express Automatic Espresso Machine",
                                             Description = "The Breville Barista Express goes from beans to espresso in under one minute. Grind Size Dial and Grind Amount Dial both feature pre-set and customizable settings to eliminate guesswork. Integrated conical burr grinder with grind size dial accurately adjusts fineness of the grind for correct dispersion amount and is activated simply by a one-touch grinding cradle with auto-stop. Removable tamper with tamping pressure guideline helps to ensure that grinds are compressed correctly. The 15-bar pump pressure system is Italian designed and made, and the pressure gauge assists in optimum extraction pressure. 67-ounce water tank has charcoal water filteration system. Includes 2 single wall filters, 2 dual wall filters, coffee scoop for pre-ground espresso, Latte art forthing jug, cleaning tool for filter and steam wand, cleaning brush for conical burrs, cleaning tablets and disc for overall machine cleaning cycle. Measures 13 1/2\" W x 11 1/2\" D x 14 1/2\" H",
                                             ImageUrl = "/content/auction-images/breville-bes860xl.jpg",
                                             ThumbnailUrl = "/content/auction-images/breville-bes860xl_thumb.jpg",
                                         });
                AddAuction(context, new Auction
                                         {
                                             IsFeatured = true,
                                             Category = homeAndGarden,
                                             StartingPrice = 500,
                                             Title = "DeLonghi EC702 15-Bar-Pump driven Espresso Maker",
                                             Description = "The DeLonghi EC702 is a 5.5-cup espresso and coffee machine that uses pods or ground coffee with the dual function filter holder. Latte and cappuccino can also be prepared with this Delonghi espresso machine with the cappuccino system frother that mixes steam and milk to create a rich, creamy froth. Moreover, the DeLonghi EC702 brews espresso or cappuccino with two separate thermostats which allow for water and steam pressure to be controlled separately. This 5.5-cup espresso and coffee machine has a 15 bar pump pressure, is made with a durable stainless steel boiler, and holds a cup warming tray. The user can clean this DeLonghi espresso machine with the removable 44-oz water tank with indicator light and removable drip tray. Other specifications of this DeLonghi espresso machine include its weight of 11.10 lbs, width of 11.25 inches, depth of 8.19 inches, and height of 12.5 inches. The input power of this espresso machine is 1100 watts.",
                                             ImageUrl = "/content/auction-images/DeLonghi-EC702.jpg",
                                             ThumbnailUrl = "/content/auction-images/DeLonghi-EC702_thumb.jpg",
                                         });

                
                AddAuction(context, new Auction
                                         {
                                             IsFeatured = true,
                                             Category = toys,
                                             StartingPrice = 10,
                                             Title = "500 assorted LEGO pieces",
                                             Description = "You will receive over 500 pieces of LEGO brand building toys pulled from a huge lot from various sets: Town, LEGO City, Star Wars, Pirates, Castle, Technic, and just about every set out there. All parts are in great condition. Lots of cool parts: including possibly spaceship hatches, plates, doors, bricks, tires, bricks, weapons, and specialty legos as well - a combination from different sets including Castle, Space, Town, UnderSea, Technic, Star Wars etc. released by the LEGO company over the last 10 years. some pieces are vintage. A great way to get started with Legos, or to bulk up your existing collection. 100% LEGO Brand, We use a two-step inspection procedure just to make sure!",
                                             ImageUrl = "/content/auction-images/legos.jpg",
                                             ThumbnailUrl = "/content/auction-images/legos_thumb.jpg",
                                         });


                context.SaveChanges();
            }

            private void AddAuction(DataContext context, Auction auction)
            {
                auction.SellerUsername = _users[_random.Next(0, _users.Length - 1)].Username;

                if (auction.StartingPrice == default(decimal))
                    auction.StartingPrice = _random.Next(1, 200);

                auction.StartTime = DateTime.Now
                    .AddDays(_random.Next(-4, 0))
                    .AddHours(_random.Next(0, 23))
                    .AddMinutes(_random.Next(0, 59));

                auction.EndTime = auction.StartTime.AddDays(_random.Next(4, 7));

                auction.Condition = (ItemCondition) _random.Next(0, 2);

                context.Auctions.Add(auction);
                context.SaveChanges();

                var bidders = _users.Where(x => x.Username != auction.SellerUsername).ToArray();
                for (int i = 0; i < _random.Next(0, 20); i++)
                {
                    var bidder = bidders[_random.Next(0, bidders.Length - 1)];
                    var amount = auction.CurrentPrice + _random.Next(1, 10) ?? auction.StartingPrice;

                    auction.PlaceBid(bidder.Username, amount);
                }
            }
        }
    }
}