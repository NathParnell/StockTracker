using StockTrackerCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockTrackerApp.Services.Infrastructure
{
    public interface IOrderService
    {
        List<OrderItem> BasketItems { get; }

        #region "Basket Methods"
        void AddItemToBasket(OrderItem basketItem, decimal productPrice);
        void RemoveItemFromBasket(string productId);
        void EditQuantityOfItemInBasket(OrderItem basketItem, int newQuantity, decimal productPrice);
        void ClearBasket();
        #endregion

        #region "Get Methods"
        List<Order> GetOrderRequestsBySupplierId(string supplierId);
        List<OrderItem> GetOrderItemsByOrderItemIds(List<string> orderItemIds);
        #endregion

        #region "Add Methods"
        bool CreateOrder(List<OrderItem> itemsToOrder, string supplierId);
        #endregion

        #region "Update Methods"

        bool UpdateOrder(Order order);

        #endregion
    }
}
