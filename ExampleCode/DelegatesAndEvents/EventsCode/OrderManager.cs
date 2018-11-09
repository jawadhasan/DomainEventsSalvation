using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ExampleCode.DelegatesAndEvents.EventsCode
{
    public class OrderManager
    {
        private List<Order> _orders;

        public event EventHandler<OrderEventArgs> OrderCreated;
        public event EventHandler<OrderEventArgs> OrderApproved;
        public event EventHandler<OrderEventArgs> OrderRejected;

        public OrderManager()
        {
            _orders = new List<Order>();
        }
        
        public void AddOrder(Order order)
        {
            if (_orders.Any(o => o.Id == order.Id))
            {
                OrderRejected?.Invoke(this, new OrderEventArgs(order.Id, order.CustomerName));
            }
            else
            {
                _orders.Add(order);
                OrderCreated?.Invoke(this, new OrderEventArgs(order.Id, order.CustomerName));
            }
        }

        public void ProcessOrders()
        {
            foreach (var order in _orders)
            {
                //process orders
                Thread.Sleep(1000);
                OrderApproved?.Invoke(this, new OrderEventArgs(order.Id, order.CustomerName));
            }
        }
    }


    public class Order
    {
        public int Id { get; }
        public DateTime OrderDate { get;}
        public string CustomerName { get;}

        public Order(int id, string customerName)
        {
            Id = id;
            CustomerName = customerName;
            OrderDate = DateTime.UtcNow;
        }
    }


    public class OrderEventArgs : EventArgs
    {
        public int Id { get; }
        public string CustomerName { get; }

        public OrderEventArgs(int id, string customerName)
        {
            Id = id;
            CustomerName = customerName;
        }
    }
}
