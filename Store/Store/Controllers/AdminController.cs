using Store.DAL;
using Store.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Store.Controllers
{
    public class AdminController : Controller
    {
        //
        // GET: /Admin/
        public ActionResult Admin()
        {
            //! Reffering to login page
            return View(new LoginModel());
        }

        public ActionResult AdminArea(LoginModel currentLogin)
        {
            //! Verifying the currentLogin details.
            LoginDAL logDAL = new LoginDAL();

            //! Testing in DB if has a match
            var testLogin = logDAL.Logins.FirstOrDefault( user => 
                (user.UserName.Equals(currentLogin.UserName)) &&
                (user.Password.Equals(currentLogin.Password)) );

            if (testLogin != null)
            {
                //! Lets keep the connection for farther use:
                Session["currentConnection"] = currentLogin;

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

        public ActionResult AddProducts()
        {
            //! Restricted for admin's only.
            if (Session["currentConnection"] != null)
            {
                return View();
            }
            return View("Admin", new LoginModel());
        }

        public ActionResult AddProductForm(Product product)
        {
            //! Restricted for admin's only.
            if (Session["currentConnection"] != null)
            {
                //! Here we Inserting products to DB
                ProductDAL proDAL = new ProductDAL();

                try
                {
                    proDAL.Products.Add(product);
                    proDAL.SaveChanges();

                    //! Case all fine, open user message.
                    return View("ProductSuccessfulAdded");
                }
                catch (Exception e)
                {
                    return View("ProductProccessError");
                }
            } 
            return View("Admin", new LoginModel());
        }

        public ActionResult EditProducts()
        {
            //if (Session["currentConnection"] != null)
            //{
                //! Only if logged in
                ProductDAL proDAL = new ProductDAL();
                List<Product> pl = proDAL.Products.ToList<Product>();

                ProductModel productModel = new ProductModel();
                productModel.oneProduct = new Product();
                productModel.ProductsCollection = new List<Product>();


                productModel.ProductsCollection.AddRange(pl);


                return View("EditProducts", productModel);
            //}
            //! Case not logged in
            //return View("Admin", new LoginModel());
        }

        public ActionResult SubmitProductsValues(ProductModel currentProducts)
        {
            if (Session["currentConnection"] != null)
            {
                //! Only if logged in


                return View("SubmitProductsValues", currentProducts);
            }
            //! Case not logged in
            return View("Admin", new LoginModel());
        }

	}
}