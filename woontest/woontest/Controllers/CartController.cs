using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Net.Mail;
using woontest.Models;
using System.Threading.Tasks;
using System.Text;
using System.Globalization;

namespace woontest.Controllers
{
    public class CartController : Controller
    {

        private store db = new store();

        public ActionResult Index(string returnUrl)
        {
            return View(new CartIndexViewModel
            {
                Cart = GetCart(),
                ReturnUrl = returnUrl
            });
        }

        public ActionResult CartTable(CartIndexViewModel carts)
        {

            return PartialView(carts);
        }

        public Cart GetCart()
        {

            Cart cart = (Cart)Session["Cart"];
            if (cart == null)
            {
                cart = new Cart();
                Session["Cart"] = cart;
            }
            return cart;
        }

        public RedirectToRouteResult AddToCart(int Id, string returnUrl)
        {
            Product pro = db.Products
                .FirstOrDefault(b => b.Id == Id);

            if (pro != null)
            {
                GetCart().AddItem(pro, 1);
            }

            return RedirectToAction("Index", new { returnUrl });
        }

        [HttpPost]
        public ActionResult AddCart(int Id)
        {
            Product pro = db.Products
                .FirstOrDefault(b => b.Id == Id);

            if (pro != null)
            {
                GetCart().AddItem(pro, 1);
            }
            return PartialView();
        }

        [HttpPost]
        public ActionResult RemoveFromCart(int Id)
        {
            Product pro = db.Products
                .FirstOrDefault(b => b.Id == Id);

            if (pro != null)
            {
                GetCart().RemoveItem(pro);
            }

            return PartialView(new CartIndexViewModel
            {
                Cart = GetCart(),
                ReturnUrl = null
            });
        }

        public PartialViewResult carts()
        {

            return PartialView();
        }

        public ActionResult ShippingDetail()
        {
            if (Session["Cart"] == null)
                return RedirectToAction("Index");

            if (((Cart)Session["Cart"]).Lines.Count() == 0)
                return RedirectToAction("Index");

            return View(new ShippingDetails());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ShippingDetail(ShippingDetails model)
        {
            if (ModelState.IsValid)
            {
                StringBuilder body = BodyMessage(model);
                MailMessage message = MailMessage(model, body);

                using (var smtp = new SmtpClient())
                {
                    var credential = new NetworkCredential
                    {
                        UserName = "wol2for@hotmail.com",  // � ��������� � �������� �����
                        Password = "������ �� ������������"  // � ��������� � �������� ������ 
                    };
                    smtp.Credentials = credential;
                    smtp.Host = "smtp-mail.outlook.com";
                    smtp.Port = 587;
                    smtp.EnableSsl = true;
                    await smtp.SendMailAsync(message);
                    Session["Cart"] = null;
                    return RedirectToAction("Sent");
                }
            }

            return View("Index");
        }

        private static MailMessage MailMessage(ShippingDetails model, StringBuilder body)
        {
            var message = new MailMessage();
            message.Bcc.Add(new MailAddress(model.Email));
            message.Bcc.Add(new MailAddress("ifkfdf@tuta.io"));
            message.IsBodyHtml = true;
            message.From = new MailAddress("wol2for@hotmail.com");
            message.Subject = "��� ����� ��������. Woonshpoonsh";
            message.Body = string.Format(body.ToString());
            message.IsBodyHtml = true;
            return message;
        }

        private StringBuilder BodyMessage(ShippingDetails model)
        {
            StringBuilder body = new StringBuilder()
            .AppendLine("<p><h3>����� ������</h3></p>")
            .AppendLine("</br>")
            .AppendLine("</hr>")
            .AppendLine(model.Name + ", ������� �� �������")
            .AppendLine("</hr>")
            .AppendLine("<p><h4>������:</h4></p>")
            .AppendLine("</br>");

            Cart tempCart = (Cart)Session["Cart"];

            foreach (var product in tempCart.Lines)
            {
                var subtotal = product.Product.PriceProduct * product.Quantity;
                body.AppendFormat("<p>" + product.Quantity +
                    " x {0:c}" + " (�����: " + subtotal.ToString("c2", CultureInfo.CreateSpecificCulture("uk-UA")) + ")</p>", product.Product.NameProduct.ToString(new CultureInfo("uk-UA")));
            }

            body.AppendFormat("<p><h3>�����: {0} ���</p></h3>", tempCart.TotalCost())
            .AppendLine("</hr>")
            .AppendLine("</br>")
            .AppendLine("<p>����� ������. ��� �������� �������� � ����, � ����� ��������� ����� ��� ��������� ������� �������� � ������, �� ������ ������� ��, ������� ��� ���������� ������ " + model.PhoneNumber + ".</p>").
            AppendLine("<p>���������� �� ��� �����!</p>");
            return body;
        }

        public ActionResult Sent()
        {
            return View();
        }

    }
}
