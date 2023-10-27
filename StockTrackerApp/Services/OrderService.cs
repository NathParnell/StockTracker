
/* Unmerged change from project 'StockTrackerApp (net7.0-maccatalyst)'
Before:
using StockTrackerCommon.Models;
After:
using StockTrackerApp;
using StockTrackerApp.Services;
using StockTrackerApp.Services;
using StockTrackerApp.Services.Infrastructure;
using StockTrackerCommon.Models;
*/
using StockTrackerApp.Services.Infrastructure;
using StockTrackerCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockTrackerApp.Services
{
    public class OrderService : IOrderService
    {
        //setup logger
        private static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(typeof(OrderService));

        //define services
        private readonly IClientTransportService _clientTransportService;

        public OrderService(IClientTransportService clientTransportService)
        {
            _clientTransportService = clientTransportService;
        }

        //Define public variables
        public List<OrderItem> BasketItems { get; private set; } = new List<OrderItem>();

        /// <summary>
        /// Method which adds an item to the basket
        /// </summary>
        /// <param name="basketItem"></param>
        public void AddItemToBasket(OrderItem basketItem, decimal productPrice)
        {
            //if the product is already in the basket, if so we just update the quantity and price
            if (BasketItems != null && BasketItems.Any(orderItem => orderItem.ProductId == basketItem.ProductId))
            {
                BasketItems.Where(orderItem => orderItem.ProductId == basketItem.ProductId).FirstOrDefault().Quantity = basketItem.Quantity;
                //check that there still is a quantity of the item in the basket, if not remove the item from basket and return
                if (BasketItems.Where(orderItem => orderItem.ProductId == basketItem.ProductId).FirstOrDefault().Quantity <= 0)
                {
                    RemoveItemFromBasket(basketItem.ProductId);
                    return;
                }
                BasketItems.Where(orderItem => orderItem.ProductId == basketItem.ProductId).FirstOrDefault().OrderPrice
                    = BasketItems.Where(orderItem => orderItem.ProductId == basketItem.ProductId).FirstOrDefault().Quantity * productPrice;
            }
            else
            {
                BasketItems.Add(basketItem);
            }
        }

        /// <summary>
        /// Method which removes an item from the basket by productId
        /// </summary>
        /// <param name="productId"></param>
        public void RemoveItemFromBasket(string productId)
        {
            if (BasketItems != null && BasketItems.Any(orderItem => orderItem.ProductId == productId))
            {
                BasketItems.Remove(BasketItems.Where(orderItem => orderItem.ProductId == productId).FirstOrDefault());
            }
        }

        public void EditQuantityOfItemInBasket(OrderItem basketItem, int newQuantity, decimal productPrice)
        {
            //if the product is already in the basket, if so we just update the quantity and price
            if (BasketItems != null && BasketItems.Any(orderItem => orderItem.ProductId == basketItem.ProductId))
            {
                BasketItems.Where(orderItem => orderItem.ProductId == basketItem.ProductId).FirstOrDefault().Quantity = newQuantity;
                //check that there still is a quantity of the item in the basket, if not remove the item from basket and return
                if (BasketItems.Where(orderItem => orderItem.ProductId == basketItem.ProductId).FirstOrDefault().Quantity <= 0)
                {
                    RemoveItemFromBasket(basketItem.ProductId);
                    return;
                }
                BasketItems.Where(orderItem => orderItem.ProductId == basketItem.ProductId).FirstOrDefault().OrderPrice
                    = BasketItems.Where(orderItem => orderItem.ProductId == basketItem.ProductId).FirstOrDefault().Quantity * productPrice;
            }
        }

        /// <summary>
        /// Method which clears the basket
        /// </summary>
        public void ClearBasket()
        {
            BasketItems.Clear();
        }
    }
}
