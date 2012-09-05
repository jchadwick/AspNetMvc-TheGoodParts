using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using Website.Models;

namespace Website.Controllers
{
    public class AuctionsController : Controller
    {
        private AuctionContext db = new AuctionContext();

        //
        // GET: /Auctions/

        public ActionResult Index(string query)
        {
            IEnumerable<Auction> auctions = db.Auctions;

            if (!string.IsNullOrWhiteSpace(query))
            {
                auctions = auctions.Where(x =>
                        x.Title.IndexOf(query, StringComparison.OrdinalIgnoreCase) > 0
                    || (x.Description ?? string.Empty).IndexOf(query, StringComparison.OrdinalIgnoreCase) > 0
                );
            }

            return View("Index", auctions.ToList());
        }

        public ActionResult Search(string query)
        {
            return Index(query);
        }


        //
        // GET: /Auctions/Details/5

        public ActionResult Details(long id = 0)
        {
            Auction auction = db.Auctions.Find(id);
            if (auction == null)
            {
                return HttpNotFound();
            }
            return View(auction);
        }

        //
        // GET: /Auctions/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Auctions/Create

        [HttpPost]
        public ActionResult Create(Auction auction)
        {
            if (ModelState.IsValid)
            {
                db.Auctions.Add(auction);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(auction);
        }

        //
        // GET: /Auctions/Edit/5

        public ActionResult Edit(long id = 0)
        {
            Auction auction = db.Auctions.Find(id);
            if (auction == null)
            {
                return HttpNotFound();
            }
            return View(auction);
        }

        //
        // POST: /Auctions/Edit/5

        [HttpPost]
        public ActionResult Edit(Auction auction)
        {
            if (ModelState.IsValid)
            {
                db.Entry(auction).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(auction);
        }

        //
        // GET: /Auctions/Delete/5

        public ActionResult Delete(long id = 0)
        {
            Auction auction = db.Auctions.Find(id);
            if (auction == null)
            {
                return HttpNotFound();
            }
            return View(auction);
        }

        //
        // POST: /Auctions/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(long id)
        {
            Auction auction = db.Auctions.Find(id);
            db.Auctions.Remove(auction);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}