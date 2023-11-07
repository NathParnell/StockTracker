using StockTrackerApp.Services.Infrastructure;
using StockTrackerCommon.Helpers;
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
        private readonly ICustomerService _customerService;
        private readonly IMessageService _messageService;

        public OrderService(IClientTransportService clientTransportService, ICustomerService customerService, IMessageService messageService)
        {
            _clientTransportService = clientTransportService;
            _customerService = customerService;
            _messageService = messageService;
        }

        //Define public variables
        public List<OrderItem> BasketItems { get; private set; } = new List<OrderItem>();

        #region "Basket Methods"
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

        #endregion

        #region "Get Methods"
        public List<Order> GetSuppliersOrderRequests(string supplierId)
        {
            string jsonResponse = _clientTransportService.TcpHandler(RequestSerializingHelper.CreateGetOrderRequestsBySupplierIdRequest(supplierId, _clientTransportService.ConnectionPortNumber));

            //if the method we tried to call did not exist
            if (String.IsNullOrEmpty(jsonResponse))
                return null;

            List<Order> orderRequests = ResponseDeserializingHelper.DeserializeResponse<List<Order>>(jsonResponse).First().ToList();
            return orderRequests;
        }

        #endregion

        #region "Add Methods"
        public bool CreateOrder(List<OrderItem> itemsToOrder, string supplierId)
        {
            string orderId = Taikandi.SequentialGuid.NewGuid().ToString();

            List<string> orderItemIds = new List<string>();
            
            //create a list of orderItemIds
            foreach (OrderItem orderItem in itemsToOrder)
            {
                orderItemIds.Add(orderItem.OrderItemId);
            }

            Order order = new Order()
            {
                OrderId = orderId,
                CustomerId = _customerService.CurrentUser.CustomerId,
                SupplierId = supplierId,
                OrderItemIds = orderItemIds,
                TotalPrice = itemsToOrder.Sum(item => item.OrderPrice),
                OrderDate = DateTime.Now,
                OrderStatus = OrderStatus.Pending,
                OrderNotes = ""
            };

            string jsonResponse = _clientTransportService.TcpHandler(RequestSerializingHelper.CreateAddOrderRequest(order, _clientTransportService.ConnectionPortNumber));

            //if the method we tried to call did not exist
            if (String.IsNullOrEmpty(jsonResponse))
                return false;

            bool addConfirmation = ResponseDeserializingHelper.DeserializeResponse<bool>(jsonResponse).First();

            //if the order request has been created successfully, then we notify the user
            if (addConfirmation)
            {
                Message message = new Message()
                {
                    MessageId = Taikandi.SequentialGuid.NewGuid().ToString(),
                    SentTime = DateTime.Now,
                    SenderId = _customerService.CurrentUser.CustomerId,
                    ReceiverId = supplierId,
                    Subject = "New Order Request",
                    MessageBody = $"A new order request has been created by {_customerService.CurrentUser.FirstNames} {_customerService.CurrentUser.Surname}"
                };

                bool messageSent = _messageService.SendMessage(message);
            }

            return addConfirmation;
        }
        #endregion
    }
}
