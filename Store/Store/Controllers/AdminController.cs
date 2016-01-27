using Store.DAL;
using Store.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Store.Controllers
{
    public class AdminController : Controller
    {
        //
        // GET: /Admin/

        public ActionResult Admin()
        {
            if ((Request.Cookies["cookie"] != null) && ModelState.IsValid)
            {
                LoginModel AuthUser = new LoginModel();
                AuthUser.UserName = "Authorized";

                return View("AdminArea", AuthUser);
            }

            //! in-case there isn't cookie reffer to login page
            return View(new LoginModel());
        }

        public ActionResult AdminArea(LoginModel currentLogin)
        {
            //! Case Logeedin already
            if (Request.Cookies["cookie"] != null)
            {
                if (currentLogin == null)
                {
                    LoginModel AuthUser = new LoginModel();
                    AuthUser.UserName = "Authorized";
                    currentLogin = AuthUser;

                    return View("AdminArea", currentLogin);
                }
                else return View("AdminArea", currentLogin);
            }


            //! Verifying the currentLogin details.
            LoginDAL logDAL = new LoginDAL();

            //! Testing in DB if has a match
            var testLogin = logDAL.Logins.FirstOrDefault( user => 
                (user.UserName.Equals(currentLogin.UserName)) &&
                (user.Password.Equals(currentLogin.Password)) );

            if (testLogin != null)
            {
                /*
                 * Lets set a cookie for successful connection
                 * expired date: today + one day.
                 * 
                 * Just Noting: This is not the orthodox way to work around cookies
                 * The right way is to store the cookie with a uniqe strings inside the DB.
                 * then each time we ask for auth, it should make an assessment if the cookie
                 * matches the logged on users. In other words; Real-life session cookie.
                 * 
                 * */
                FormsAuthentication.SetAuthCookie("cookie", true);
                Response.Cookies["cookie"].Expires = DateTime.Now.AddDays(1);

                //! Reffering to AdminArea page
                return View("AdminArea", currentLogin); 
            }
            else
                return View("Admin", currentLogin);
        }

        [HttpPost]
        public ActionResult Logon(LoginModel currentLogin)
        {
            if (ModelState.IsValid)
                return AdminArea(currentLogin);
            else return View("Admin", currentLogin);
        }

        public ActionResult Logout()
        {
            /*
             * This function is about removing the current cookie
             * */
            if (Response.Cookies["cookie"] != null)
                Response.Cookies["cookie"].Expires = DateTime.Now.AddDays(-1);

            return View();
        }

        [Authorize]
        public ActionResult AddProducts()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult AddProductForm(ProductUploadModel obj)
        {
            //! Lets make a connection to products inside DB
            ProductDAL proDAL = new ProductDAL();
            
            /*
             * Lets test if the current SKU is uniqe;
             * if not we would like to tell the admin
             * to change the SKU
             * */
            var uniqeSKU = proDAL.Products.FirstOrDefault(item=>item.SKU == obj.pr.SKU);

            if (uniqeSKU != null)
                return View("ProductSKUError");

            /*
             * Files uploading is-in the block below
             * */
            if (obj.pic != null && obj.pic.ContentLength > 0)
            {
                var path = "~/PicData/";
                var fname = "pic_" + obj.pr.Id + "_" + obj.pic.FileName;
                obj.pic.SaveAs(
                    Path.Combine(
                    Server.MapPath(path), fname));

                //! Setting picURL to be used farther
                obj.pr.PicURL = fname;
            }

            try
            {
                //! Lets store the product inside the Products DB instance.
                proDAL.Products.Add(obj.pr);
                proDAL.SaveChanges();

                //! Case all fine, open user message.
                return View("ProductSuccessfulAdded");
            }
            catch (Exception)
            {
                //! Case not:
                return View("ProductProccessError");
            }
        }

        [Authorize]
        public ActionResult EditProducts()
        {

            //! Pulling DATA from db using DbContext
            var proDAL = new ProductDAL();
            List<Products> pl = proDAL.Products.ToList<Products>();

            ProductModel productModel = new ProductModel();
            productModel.ProductsCollection = new List<Products>();
            productModel.ProductsCollection = pl;

            return View("EditProducts", productModel);
        }

        [HttpPost]
        [Authorize]
        public ActionResult SubmitProductsValues(ProductModel productModel)
        {
            //! Pulling DATA from db using DbContext
            var proDAL = new ProductDAL();

            //! Loop through model.ProductsCollection 
            foreach (var p in productModel.ProductsCollection)
            {
                //! Matching the current product
                var query = proDAL.Products.FirstOrDefault(q => q.Id == p.Id);

                if (query != null)
                {
                    if (p.pExist != true)
                    {
                        /*
                         * pExist it's an Indicator about the product lifetime.
                         * */
                        query.Quantity = p.Quantity;
                        proDAL.SaveChanges();
                    }
                    else
                    {
                        //! pExist True; means it was mark to be removed
                        query.pExist = false;
                        proDAL.Products.Remove(query);
                        proDAL.SaveChanges();
                    }
                }
            }
            //! All-set redirecting view.
            return View("SubmitProductsValues");
        }

        [Authorize]
        public ActionResult GetJsonOrders()
        {
            /*
             * This function intended to show all of the order details that has been commited.
             * This function passes an Json presentation to caller as requested.
             **/
            
            var orderDAL = new OrderDAL();
            var productDAL = new ProductDAL();
            var customerDAL = new CustomerDAL();

            ShowOrdersModel model = new ShowOrdersModel();
            model.Orders = new List<DetailedOrder>();

            List<Order> orders = orderDAL.Order.ToList<Order>();

            foreach (var order in orders)
            {
                /*
                 * Gathering all realtions that stands with the right conditions
                 * */
                var currentCustomer = customerDAL.Customers.FirstOrDefault(
                    cust => cust.Id == order.CustomerId);
                var currentProduct = productDAL.Products.FirstOrDefault(
                    prod => prod.Id == order.Product_Id);

                var viewOrder = new DetailedOrder();

                /**
                 * Setting the viewOrder list to be viewed.
                 **/

                viewOrder.Id = order.Id;
                viewOrder.Date = order.Date;
                viewOrder.CustomerName = currentCustomer.Name;
                viewOrder.ProductName = currentProduct.Name;
                viewOrder.ProductPrice = currentProduct.Price;

                model.Orders.Add(viewOrder);
            }

            return Json(model.Orders, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult ShowOrders()
        {
            /*
             * There's no stuff to be done here.
             * the magic goes over the AJAX call at the view.
             **/
            return View("ShowOrders");
        }
	}
}