using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using woontest.Models;

namespace woontest.Controllers
{
    public class ProductsController : Controller
    {
        private store db = new store();

        public ActionResult AddProductToAllList()
        {
            int temp;
            if (Session["Skip"] == null)
            {
                return View("All");
            }

            if ((int)Session["TotalCountProduct"] < (int)Session["Skip"])
            {
                return new EmptyResult();
            }

            if (((int)Session["TotalCountProduct"] - (int)Session["Skip"]) <= 9)
            {
                temp = (int)Session["Skip"];
                int take = (int)Session["TotalCountProduct"] - (int)Session["Skip"];
                Session["Skip"] = temp + 9;

                return PartialView(db.Products.OrderBy(r => r.Id).Skip(temp).Take(take).ToList());
            }

            temp = (int)Session["Skip"];
            Session["Skip"] = temp + 9;
            return PartialView(db.Products.OrderBy(r => r.Id).Skip(temp).Take(9).ToList());
        }

        public async Task<ActionResult> All()
        {
            Session["TotalCountProduct"] = db.Products.Count();
            Session["Skip"] = 9;
            return View(await db.Products.OrderBy(r => r.Id).Take(9).ToListAsync());
        }

        public PartialViewResult Review(int idProduct)
        {
            var allComments = db.Comments.Where(r => r.IdProduct == idProduct).ToList();
            return PartialView(allComments);
        }

        [HttpGet]
        public PartialViewResult createCommentForProduct()
        {
            return PartialView();
        }

        [HttpPost]
        public PartialViewResult createCommentForProduct(Comment comm)
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> upddateComments(Comment comm)
        {
              db.Comments.Add(comm);
              await  db.SaveChangesAsync();
              return PartialView(comm.IdProduct);

        }

        public async Task<ActionResult> List(int idCategory)
        {
            var productsOfCategory = await db.Products.Where(r => r.IdCategory == idCategory).ToListAsync();
            return View(productsOfCategory);
        }


        public RedirectToRouteResult Redict(int id)
        {
            // Сюда можно передавать не id продукта, а сам продукт!!!
            return RedirectToAction("Detail", "Products", new { idProduct = id });
        }

        public ActionResult GreatIdea()
        {
            
            return View(db.Products.FirstOrDefault(r=> r.Id == 24));
        }

        public ActionResult HappyBox()
        {
            return View();
        }

        public ActionResult Detail(int idProduct)
        {
            if (idProduct == 24)
            {
                return RedirectPermanent("/Products/GreatIdea");
            }

            if (idProduct == 80)
            {
                return RedirectPermanent("/Products/HappyBox");
            }
            Product product = db.Products.AsParallel().Where(r => r.Id == idProduct).FirstOrDefault();
            return View(product);
        }

        [OutputCache(Duration = 3600, Location = System.Web.UI.OutputCacheLocation.Any, VaryByParam = "productId")]
        public FileContentResult GetImage(int productId)
        {
            PictureProduct image = db.PictureProducts.FirstOrDefault(p => p.IdProduct == productId);
            if (image != null)
            {
                return File(image.Picture, "image/jpg");
            }
            else
            {
                return null;
            }
        }

        public FileContentResult GetImageForPicId(int picId)
        {
            PictureProduct image = db.PictureProducts.First(r => r.Id == picId);
            if (image != null)
            {
                return File(image.Picture, "image/jpg");
            }
            else
            {
                return null;
            }
        }

        // GET: Products
        [Authorize]
        public ActionResult Index()
        {
            var product = db.Products.Include(p => p.Category);
            return View(product.ToList());
        }

        // GET: Products/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }


        // GET: Products/Create
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.IdCategory = new SelectList(db.Categories, "Id", "NameCategory");
            return View();
        }

        // POST: Products/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,IdCategory,NameProduct,PriceProduct,DescriptionProduct,PictureProduct,DateProduct")] Product product)
        {
            if (ModelState.IsValid)
            {
                //db.Product.Add(product);
                //db.SaveChanges();
                //return RedirectToAction("Index");
                var file = Request.Files[0];

                byte[] temp = new byte[file.ContentLength];
                file.InputStream.Read(temp, 0, file.ContentLength);
                ICollection<PictureProduct> pictureOfProduct = new List<PictureProduct>();
                pictureOfProduct.Add(new PictureProduct { IdProduct = product.Id, MimeTypeProduct = file.ContentType, Picture = temp });
                //PictureProduct pictureOfProduct = new PictureProduct() { IdProduct = product.Id, MimeTypeProduct = file.ContentType, Picture = temp };
                //db.PictureProducts.Add(pictureOfProduct);
                //db.SaveChanges();
                db.Products.Add(new Product { IdCategory = product.IdCategory, NameProduct = product.NameProduct, PictureProducts = pictureOfProduct, PriceProduct = product.PriceProduct, DescriptionProduct = product.DescriptionProduct, DateProduct = DateTime.Now });
                db.SaveChanges();
                return RedirectToAction("Index", "Home");

            }

            ViewBag.IdCategory = new SelectList(db.Categories, "Id", "NameCategory", product.IdCategory);
            return View(product);
        }

        // GET: Products/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdCategory = new SelectList(db.Categories, "Id", "NameCategory", product.IdCategory);
            return View(product);
        }

        // POST: Products/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,IdCategory,NameProduct,PriceProduct,DescriptionProduct,PictureProduct,DateProduct")] Product product)
        {

            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdCategory = new SelectList(db.Categories, "Id", "NameCategory", product.IdCategory);
            return View(product);
        }

        // GET: Products/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            List<PictureProduct> picture = db.PictureProducts.Where(r => r.IdProduct == id).ToList();
            List<Comment> comments = db.Comments.Where(r => r.IdProduct == id).ToList();

            foreach (var pic in picture)
            {
                db.PictureProducts.Remove(pic);
            }
            foreach (var com in comments)
            {
                db.Comments.Remove(com);
            }

            db.Products.Remove(product);
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
