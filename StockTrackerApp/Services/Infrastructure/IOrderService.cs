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

        void AddItemToBasket(OrderItem basketItem, decimal productPrice);
        void RemoveItemFromBasket(string productId);
        void EditQuantityOfItemInBasket(OrderItem basketItem, int newQuantity, decimal productPrice);
        void ClearBasket();
    }
}
