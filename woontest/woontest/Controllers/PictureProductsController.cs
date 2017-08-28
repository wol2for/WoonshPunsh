using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using woontest.Models;

namespace woontest.Controllers
{
    public class PictureProductsController : Controller
    {
        private store db = new store();


        public FileContentResult GetImage(int PictureOfProductId)
        {
            PictureProduct image = db.PictureProducts.FirstOrDefault(p => p.Id == PictureOfProductId);
            if (image != null)
            {
                return File(image.Picture, "image/jpg");
            }
            else
            {
                return null;
            }
        }

        // GET: PictureProducts
        public ActionResult Index()
        {
            var pictureProducts = db.PictureProducts.Include(p => p.Product);
            return View(pictureProducts.ToList());
        }

        [Authorize]
        // GET: PictureProducts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PictureProduct pictureProduct = db.PictureProducts.Find(id);
            if (pictureProduct == null)
            {
                return HttpNotFound();
            }
            return View(pictureProduct);
        }

        [Authorize]
        // GET: PictureProducts/Create
        public ActionResult Create()
        {
            ViewBag.IdProduct = new SelectList(db.Products, "Id", "NameProduct");
            return View();
        }

        // POST: PictureProducts/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,IdProduct,Picture,MimeTypeProduct")] PictureProduct pictureProduct)
        {
            if (ModelState.IsValid)
            {
                var file = Request.Files[0];
                byte[] temp = new byte[file.ContentLength];
                file.InputStream.Read(temp, 0, file.ContentLength);
                pictureProduct.Picture = temp;
                db.PictureProducts.Add(pictureProduct);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdProduct = new SelectList(db.Products, "Id", "NameProduct", pictureProduct.IdProduct);
            return View(pictureProduct);
        }

        // GET: PictureProducts/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PictureProduct pictureProduct = db.PictureProducts.Find(id);
            if (pictureProduct == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdProduct = new SelectList(db.Products, "Id", "NameProduct", pictureProduct.IdProduct);
            return View(pictureProduct);
        }

        // POST: PictureProducts/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,IdProduct,Picture,MimeTypeProduct")] PictureProduct pictureProduct, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    var filee = Request.Files[0];
                    pictureProduct.Picture = new byte[filee.ContentLength];
                    filee.InputStream.Read(pictureProduct.Picture, 0, filee.ContentLength);
                    db.Entry(pictureProduct).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    var pic = db.PictureProducts.Where(r => r.Id == pictureProduct.Id).Select(g => g.Picture).First();
                    pictureProduct.Picture= pic;
                    db.Entry(pictureProduct).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }




            }
            ViewBag.IdProduct = new SelectList(db.Products, "Id", "NameProduct", pictureProduct.IdProduct);
            return View(pictureProduct);
        }

        // GET: PictureProducts/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PictureProduct pictureProduct = db.PictureProducts.Find(id);
            if (pictureProduct == null)
            {
                return HttpNotFound();
            }
            return View(pictureProduct);
        }

        // POST: PictureProducts/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PictureProduct pictureProduct = db.PictureProducts.Find(id);
            db.PictureProducts.Remove(pictureProduct);
            db.SaveChanges();
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
