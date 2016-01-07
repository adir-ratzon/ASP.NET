﻿using Store.DAL;
using Store.Models;
using System;
using System.Collections.Generic;
using System.IO;
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

        [HttpPost]
        public ActionResult AddProductForm(ProductUploadModel obj)
        {
            //! Restricted for admin's only.
            if (Session["currentConnection"] != null)
            {
                //! Files handle should be here:
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


                //! Here we Inserting products to DB
                ProductDAL proDAL = new ProductDAL();

                try
                {
                    proDAL.Products.Add(obj.pr);
                    proDAL.SaveChanges();

                    //! Case all fine, open user message.
                    return View("ProductSuccessfulAdded");
                }
                catch (Exception)
                {
                    return View("ProductProccessError");
                }
            } 
            else
                return View("Admin", new LoginModel());
        }

        public ActionResult EditProducts()
        {
            if (Session["currentConnection"] != null)
            {
                //! Pulling DATA from db using DbContext
                var proDAL = new ProductDAL();
                List<Products> pl = proDAL.Products.ToList<Products>();

                ProductModel productModel = new ProductModel();
                productModel.ProductsCollection = new List<Products>();
                productModel.ProductsCollection = pl;

                return View("EditProducts", productModel);
            }
            else
                return View("Admin", new LoginModel());
        }

        [HttpPost]
        public ActionResult SubmitProductsValues(ProductModel productModel)
        {
            if (Session["currentConnection"] != null)
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
            else
                return View("Admin", new LoginModel());
        }

        public ActionResult ShowOrders(ShowOrdersModel model)
        {
            if (Session["currentConnection"] == null)
                return View("Admin", new LoginModel());

            /*
             * Creating the DB Connection for the orders view.
             **/

            var orderDAL = new OrderDAL();
            var productDAL = new ProductDAL();
            var customerDAL = new CustomerDAL();

            model.Orders = new List<DetailedOrder>();

            List<Order> orders = orderDAL.Order.ToList<Order>();

            foreach (var order in orders)
            {
                var currentCustomer = customerDAL.Customers.FirstOrDefault(
                    cust=>cust.Id == order.CustomerId);
                var currentProduct = productDAL.Products.FirstOrDefault(
                    prod=>prod.Id == order.Product_Id);

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
            return View("ShowOrders", model);
        }



	}
}