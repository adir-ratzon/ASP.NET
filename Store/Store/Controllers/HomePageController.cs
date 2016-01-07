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
            /*
             * Creating the DB Connection for the homepage products view.
             * */
            var proDAL = new ProductDAL();
            List<Products> pl = proDAL.Products.ToList<Products>();

            //! Creating the Model which will be used later on.
            HomepageModel homepageModel = new HomepageModel();

            homepageModel.ProductsCollection = new List<Products>();
            homepageModel.ProductsCollection = pl;

            return View("HomePage", homepageModel);
        }

        [HttpPost]
        public ActionResult PlaceOrder(HomepageModel hpm)
        {
            /* Checking if the model isn't fake
             * or empty. and placeing the order, and customer 
             * as well.
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
                     * and take place to ensure there isn't duplicated customer data.
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
                     * 
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