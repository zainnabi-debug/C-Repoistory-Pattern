﻿using SmokersTavern.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SmokersTavern.Controllers
{
    public class OrdersController : Controller
    {
        public class OrdersController : Controller
        {
            private ApplicationDbContext db = new ApplicationDbContext();

            
            public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
            {
                ViewBag.CurrentSort = sortOrder;
                ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
                ViewBag.PriceSortParm = sortOrder == "Price" ? "price_desc" : "Price";

                if (searchString != null)
                {
                    page = 1;
                }
                else
                {
                    searchString = currentFilter;
                }

                ViewBag.CurrentFilter = searchString;

                var orders = from o in db.Orders
                             select o;

                if (!String.IsNullOrEmpty(searchString))
                {
                    orders = orders.Where(s => s.FirstName.ToUpper().Contains(searchString.ToUpper())
                                           || s.LastName.ToUpper().Contains(searchString.ToUpper()));
                }
                switch (sortOrder)
                {
                    case "name_desc":
                        orders = orders.OrderByDescending(s => s.FirstName);
                        break;
                    case "Price":
                        orders = orders.OrderBy(s => s.Total);
                        break;
                    case "price_desc":
                        orders = orders.OrderByDescending(s => s.Total);
                        break;
                    default:  
                        orders = orders.OrderBy(s => s.FirstName);
                        break;
                }

                int pageSize = 3;
                int pageNumber = (page ?? 1);
                return View(orders.ToPagedList(pageNumber, pageSize));

                
            }

            
            public async Task<ActionResult> Details(int? id)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Order order = await db.Orders.FindAsync(id);
                var orderDetails = db.OrderDetails.Where(x => x.OrderId == id);

                order.OrderDetails = await orderDetails.ToListAsync();
                if (order == null)
                {
                    return HttpNotFound();
                }
                return View(order);
            }

            
            public ActionResult Create()
            {
                return View();
            }

            
            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<ActionResult> Create(Order order)
            {
                if (ModelState.IsValid)
                {
                    db.Orders.Add(order);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }

                return View(order);
            }

            
            public async Task<ActionResult> Edit(int? id)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Order order = await db.Orders.FindAsync(id);
                if (order == null)
                {
                    return HttpNotFound();
                }
                return View(order);
            }

            
            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<ActionResult> Edit(Order order)
            {
                if (ModelState.IsValid)
                {
                    db.Entry(order).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                return View(order);
            }

            
            public async Task<ActionResult> Delete(int? id)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Order order = await db.Orders.FindAsync(id);
                if (order == null)
                {
                    return HttpNotFound();
                }
                return View(order);
            }

            
            [HttpPost, ActionName("Delete")]
            [ValidateAntiForgeryToken]
            public async Task<ActionResult> DeleteConfirmed(int id)
            {
                Order order = await db.Orders.FindAsync(id);
                db.Orders.Remove(order);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            protected override void Dispose(bool disposing)
            {
                if (disposing)
                {
                    db.Dispose();
                }
                base.Dispose(disposing);
            }
        }
    }
}