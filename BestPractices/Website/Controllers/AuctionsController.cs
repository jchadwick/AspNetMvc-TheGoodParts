﻿using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using Common;
using Common.DataAccess;
using Website.Models;

namespace Website.Controllers
{
    public class AuctionsController : Controller
    {
        private readonly AuctionContext _db = new AuctionContext();

        public ActionResult Categories(string id)
        {
            var category = _db.Categories.FirstOrDefault(x => x.Key == id);
            
            if (category == null)
                return HttpNotFound();

            var auctions = _db.Auctions.Where(x => x.CategoryId == category.Id);

            var viewModel = new AuctionsViewModel
            {
                Auctions = auctions.Select(Mapper.DynamicMap<AuctionViewModel>).ToArray(),
            };

            return View("Auctions", viewModel);
        }

        public ActionResult Index(string query, int page=0, int size=10)
        {
            IQueryable<Auction> auctions = _db.Auctions;

            if(!string.IsNullOrWhiteSpace(query))
            {
                auctions = auctions.Where(x =>
                           x.Title.ToLower().IndexOf(query.ToLower()) >= 0
                        || x.Description.ToLower().IndexOf(query.ToLower()) >= 0
                    );
            }

            auctions = auctions.OrderBy(x => x.EndTime).Skip(page*size).Take(size);

            var viewModel = new AuctionsViewModel {
                    Auctions = auctions.Select(Mapper.DynamicMap<AuctionViewModel>).ToArray(),
                    Page = page,
                    PageSize = size,
                };

            return View("Auctions", viewModel);
        }
    }
}