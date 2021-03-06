﻿using Store.DAL;
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
        public ActionResult Index(HomepageModel hpm)
        {

            if (hpm.ProductsCollection != null) 
                return View("HomePage", hpm);

            /*
             * Creating the DB Connection for the homepage products view.
             * */
            var proDAL = new ProductDAL();
            List<Products> pl = proDAL.Products.Where(item => item.Quantity >= 1).ToList<Products>();

            //! Creating the Model which will be used later on.
            HomepageModel homepageModel = new HomepageModel();

            homepageModel.ProductsCollection = new List<Products>();
            homepageModel.ProductsCollection = pl;

            return View("HomePage", homepageModel);
        }

        [HttpPost]
        public ActionResult SearchFor(HomepageModel searchfor)
        {
            var proDAL = new ProductDAL();
            List<Products> pl = proDAL.Products.Where(
                entity => entity.Name.Contains(searchfor.SingleProduct.Name) &&
                    (entity.Quantity >=1)).ToList<Products>();

            //! Creating the Model which will be used later on.
            searchfor = new HomepageModel();

            searchfor.ProductsCollection = new List<Products>();
            searchfor.ProductsCollection = pl;

            return View("HomePage", searchfor);
        }

        [HttpPost]
        public ActionResult FilterByPrice(HomepageModel hpm)
        {
            /*
             * Filtering by price is an option to filter the products at the homepage
             * by the lower and upper price bound.
             * */
            var proDAL = new ProductDAL();

            //! Getting the products which stands with the conditions.
            List<Products>  pl = proDAL.Products.Where(
            entity => (entity.Price >= hpm.Pricing.lower) &&
                (entity.Price <= hpm.Pricing.upper) && 
                (entity.Quantity >= 1)).ToList<Products>();

            //! Creating the Model which will be used later on.
            hpm = new HomepageModel();

            hpm.ProductsCollection = new List<Products>();
            hpm.ProductsCollection = pl;

            return View("HomePage", hpm);
        }

        [HttpPost]
        public ActionResult PlaceOrder(HomepageModel hpm)
        {
            /* Checking if the model isn't fake
             * or empty. and placeing the order, 
             * and setting up the customer as well.
             * */
            if (hpm.SingleProduct != null && hpm.CustomerEntity != null)
            {
                try
                {
                    //! Lets Insert the Customer details first:
                    CustomerDAL customerDAL = new CustomerDAL();
                    
                    CustomerEntity customer = new CustomerEntity();
                    customer.Name = hpm.CustomerEntity.Name;
                    customer.Address = hpm.CustomerEntity.Address;
                    customer.Email = hpm.CustomerEntity.Email;

                    /**
                     * CheckIfExist variable holding the current customer data.
                     * and take place to ensure there isn't duplicated customers.
                     * 
                     * We assusme that uniqe customer has uniqe Name, 
                     * and uniqe Email Addr. on any other mismatches we'll add the
                     * customer as a new customer record.
                     * */
                    var checkIfExistAlready = customerDAL.Customers.FirstOrDefault(
                        cust => (cust.Name.Equals(customer.Name)) &&
                           (cust.Address.Equals(customer.Address)) &&
                           (cust.Email.Equals(customer.Email))
                        );

                    //! Case customer not exist in customers data.
                    if (checkIfExistAlready == null)
                    {
                        /* This step above and below is about avoiding redundency
                         * and keep only exact one Customer recored.
                         * */

                        customerDAL.Customers.Add(customer);
                        customerDAL.SaveChanges();
                    }

                    //! Now Lets bind the customer Id to the Order CustomerId
                    var exactCustomer = customerDAL.Customers.FirstOrDefault(
                        cust=> (cust.Name.Equals(hpm.CustomerEntity.Name)) &&
                           (cust.Address.Equals(hpm.CustomerEntity.Address)) &&
                           (cust.Email.Equals(hpm.CustomerEntity.Email))
                        );

                    //! Now that we have the current customer details lets make an order
                    OrderDAL orderDAL = new OrderDAL();

                    Order newOrder = new Order();
                    newOrder.Date = DateTime.Now;
                    newOrder.CustomerId = exactCustomer.Id;
                    newOrder.Product_Id = hpm.SingleProduct.Id;

                    orderDAL.Order.Add(newOrder);
                    orderDAL.SaveChanges();

                    /*
                     * Quantity handaling for the current product
                     **/
                    ProductDAL productDAL = new ProductDAL();
                    var currentProduct = productDAL.Products.FirstOrDefault(
                        prod=>prod.Id == hpm.SingleProduct.Id);

                    if (currentProduct.Quantity >= 0)
                        currentProduct.Quantity--;

                    productDAL.Entry(currentProduct).CurrentValues.SetValues(currentProduct);
                    productDAL.SaveChanges();

                    return View("OrderCompleted");
                }
                catch (Exception)
                {
                    //! If there's an error, yield err.
                    return View("OrderFaild");
                }
            }
            return View("OrderCompleted");
        }
	}
}