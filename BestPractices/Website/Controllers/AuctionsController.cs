using System;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using Website.Models;

namespace Website.Controllers
{
    public class AuctionsController : Controller
    {
        public ActionResult Index(string query, int page=0, int size=10)
        {
            IQueryable<Auction> auctions = new AuctionContext().Auctions;

            if(!string.IsNullOrWhiteSpace(query))
            {
                auctions = auctions.Where(x =>
                           x.Title.ToLower().IndexOf(query.ToLower()) >= 0
                        || x.Description.ToLower().IndexOf(query.ToLower()) >= 0
                    );
            }

            auctions = auctions.OrderBy(x => x.EndTime).Skip(page*size).Take(size);

            var viewModel = new AuctionsViewModel {
                    Auctions = auctions.Select(Mapper.Map<AuctionViewModel>).ToArray(),
                    Page = page,
                    PageSize = size,
                };

            return View("Auctions", viewModel);
        }
    }
}