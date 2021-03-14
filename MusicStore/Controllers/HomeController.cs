using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace MusicStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly Data.MusicStoreDbContext context = new Data.MusicStoreDbContext();

        public ActionResult Index()
        {
            return View(context.Albums.OrderByDescending(p => p.Date).Take(12));
        }
        public ActionResult Search(string keyword)
        {
            if (string.IsNullOrEmpty(keyword))
                return RedirectToAction("Index");

            var keywords = Regex.Split(keyword, @"\s+");
            var result = context.Albums.Where(p => keywords.Any(q => p.Name.Contains(q)) || keywords.Any(q => p.Artist.Name.Contains(q)) || keywords.Any(q => p.Genre.Name.Contains(q))).ToList();
            return View(result);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Genre(int id)
        {
            return View(context.Genres.Find(id));
        }

        public ActionResult Artist(int id)
        {
            return View(context.Artists.Find(id));
        }

        public ActionResult ArtistList()
        {
            return View(context.Artists.OrderBy(p => p.Name));
        }


        public ActionResult Album(int id)
        {
            var model = context.Albums.Find(id);

            var previews = Session["previews"] as List<int>;
            if (!previews.Any(p => p == id))
            {
                model.Previews++;
                context.Entry(model).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
                previews.Add(id);
            }
            return View(model);
        }

        public ActionResult CategoryMenu()
        {
            return View(context.Genres.OrderBy(p => p.Name));
        }

        public ActionResult TopReviews()
        {
            return View(context.Albums.OrderByDescending(p => p.Previews).Take(5));
        }

        public ActionResult ShoppingCart() => View(Session["shoppingcart"]);

        public ActionResult Add2Cart(int id)
        {
            var album = context.Albums.Find(id);
            var cart = Session["shoppingcart"] as ShoppingCart;
            cart.Add(new CartItem
            {
                AlbumId = id,
                Price = album.Price,
                Quantity = 1,
                Name = album.Name,
                CoverImage = album.CoverImage
            });
            TempData["success"] = $"{album.Name} isimli albüm sepeninize eklenmiştir!";
            return Redirect(Request.UrlReferrer.ToString());
        }

        public ActionResult IncreaseCartItem(int id)
        {
            var cart = Session["shoppingcart"] as ShoppingCart;
            cart.Add(id);
            TempData["success"] = $"Sepetiniz güncellenmiştir!";
            return Redirect(Request.UrlReferrer.ToString());
        }

        public ActionResult DecreaseCartItem(int id)
        {
            var cart = Session["shoppingcart"] as ShoppingCart;
            cart.DecreaseQuantity(id);
            TempData["success"] = $"Sepetiniz güncellenmiştir!";
            return Redirect(Request.UrlReferrer.ToString());
        }

        public ActionResult RemoveCartItem(int id)
        {
            var cart = Session["shoppingcart"] as ShoppingCart;
            cart.Remove(id);
            TempData["success"] = $"Sepetiniz güncellenmiştir!";
            return Redirect(Request.UrlReferrer.ToString());
        }

        public ActionResult ClearShoppingCart()
        {
            var cart = Session["shoppingcart"] as ShoppingCart;
            cart.Clear();
            TempData["success"] = $"Sepetiniz güncellenmiştir!";
            return Redirect(Request.UrlReferrer.ToString());
        }

        public ActionResult ShoppingCartButton()
        {
            return View(Session["shoppingcart"]);
        }
               
    }
}