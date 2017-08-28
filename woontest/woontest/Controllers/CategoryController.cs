using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Text;
using woontest.Models;

namespace woontest.Controllers
{
    public class CategoryController : Controller
    {
        private store db = new store();

        public ActionResult GetImage(byte[] picture)
        {
            //PictureProduct prod = db.PictureProduct.FirstOrDefault(p => p.Id == id);
            return File(picture, "image/png"); // Might need to adjust the content type based on your actual image type
        }

        [Authorize]
        public ActionResult Index()
        {   
            return View(db.Categories);
        }

        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            //ViewBag.IdCategory = new SelectList(db.Category, "Id", "NameCategory", product.IdCategory);
            return View(category);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,NameCategory,PictureCategory,MimeTypeCateg")] Category cat, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    var filee = Request.Files[0];
                    cat.PictureCategory = new byte[filee.ContentLength];
                    filee.InputStream.Read(cat.PictureCategory, 0, filee.ContentLength);
                    db.Entry(cat).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index", "Category");

                }
                else
                {
                    var pic = db.Categories.Where(r => r.Id == cat.Id).Select(g => g.PictureCategory).First();
                    cat.PictureCategory = pic;
                    db.Entry(cat).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            else
                return View("Edit");
        }

        [Authorize]
        public ActionResult Create()
        {
            return View();

        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,NameCategory,PictureCategory,MimeTypeCateg")] Category cat)
        {
            if (ModelState.IsValid)
            {
                var file = Request.Files[0];
                cat.MimeTypeCateg = file.ContentType;
                cat.PictureCategory = new byte[file.ContentLength];
                file.InputStream.Read(cat.PictureCategory, 0, file.ContentLength);
                db.Categories.Add(new Category() {NameCategory = cat.NameCategory, MimeTypeCateg = cat.MimeTypeCateg, PictureCategory = cat.PictureCategory });
                db.SaveChanges();
                return RedirectToAction("Index","Home");
            }
            else
            return View();
        }

        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category cat = db.Categories.Find(id);
            if (cat == null)
            {
                return HttpNotFound();
            }
            return View(cat);
        }

        [Authorize]
        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Category cat = db.Categories.Find(id);
            List<Product> product = db.Products.Where(r => r.IdCategory == id).ToList();
            List<PictureProduct> pic = new List<PictureProduct>();
            List<Comment> com = new List<Comment>();
            //Поиск всех товаров данной категории
            foreach (var pr in product)
            {
                var temp = db.PictureProducts.Where(r => r.IdProduct == pr.Id).ToList();
                foreach (var pc in temp)
                {
                    pic.Add(pc);
                }
                temp.Clear();

            }
            //Поиск всех комментариев всех продуктов принадлежащие данное категории
            foreach (var comm in com)
            {
                var temp = db.Comments.Where(r => r.IdProduct == comm.Id).ToList();
                foreach (var pc in temp)
                {
                    com.Add(pc);
                }
                temp.Clear();
            }

            //Удаление комментариев 
            foreach (var c in com)
            {
                db.Comments.Remove(c);
            }
            //Удаление картинок
            foreach (var p in pic)
            {
                db.PictureProducts.Remove(p);
            }
            //Удаление продуктов
            foreach (var pro in product)
            {
                db.Products.Remove(pro);
            }

            db.Categories.Remove(cat);

            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}