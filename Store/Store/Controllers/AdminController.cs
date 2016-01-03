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

        public ActionResult AddProductForm(Products product)
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

            //! Pulling DATA from db using DbContext
            var proDAL = new ProductDAL();
            List<Products> pl = proDAL.Products.ToList<Products>();

            ProductModel productModel = new ProductModel();
            productModel.ProductsCollection = new List<Products>();
            productModel.ProductsCollection = pl;

            return View("EditProducts", productModel);

        }

        [HttpPost]
        public ActionResult SubmitProductsValues(ProductModel productModel)
        {
            //! Pulling DATA from db using DbContext
            var proDAL = new ProductDAL();
            
            //! Loop through model.ProductsCollection 
            foreach (var p in productModel.ProductsCollection)
            {
                //! Matching the current product
                var query = proDAL.Products.FirstOrDefault(q=>q.Id == p.Id);

                if (query != null)
                {
                    if (p.pExist != true)
                    {
                        query.Quantity = p.Quantity;
                        proDAL.SaveChanges();
                    }
                    else
                    {
                        //! pExist True means it was mark to be remove
                        query.pExist = false;
                        proDAL.Products.Remove(query);
                        proDAL.SaveChanges();
                    }
                }                
            }
            //! Save and redirect
            return View("SubmitProductsValues");
        }

	}
}