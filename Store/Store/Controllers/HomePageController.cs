using Store.DAL;
using Store.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Store.Controllers
{
    public class HomePageController : Controller
    {
        //
        // GET: /HomePage/
        public ActionResult Index()
        {
            //! Pulling DATA from db using DbContext
            var proDAL = new ProductDAL();
            List<Products> pl = proDAL.Products.ToList<Products>();

            HomepageModel homepageModel = new HomepageModel();
            homepageModel.ProductsCollection = new List<Products>();
            homepageModel.ProductsCollection = pl;

            return View("HomePage", homepageModel);
        }
	}
}