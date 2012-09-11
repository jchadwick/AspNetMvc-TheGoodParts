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

            private void AddAuction(DataContext context, Auction auction)
            {
                auction.SellerUsername = _users[_random.Next(0, _users.Length - 1)].Username;
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