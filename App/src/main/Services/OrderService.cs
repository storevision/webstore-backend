using Microsoft.EntityFrameworkCore;
using Webshop.App.src.main.Models;
using Webshop.Models.DB;

namespace Webshop.App.src.main.Services;

public class OrderService
{
    private readonly ApplicationDbContext _context;
    private readonly IServiceScopeFactory _scopeFactory;
    
    public OrderService(ApplicationDbContext context, IServiceScopeFactory scopeFactory)
    {
        _context = context;
        _scopeFactory = scopeFactory;
    }
    
    public async Task addOrder(Order order)
    {
        _context.orders.Add(order);
        await _context.SaveChangesAsync();
    }

    public void createOrder(int userId, Address? address)
    {
        var scope = _scopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        
        Order order = new Order();
        decimal totalAmount = 0;
        
        order.CustomerId = userId;
        var adress = _context.addresses.FirstOrDefault(a => a.CustomerId == userId);
        
        if (adress != null)
        {
        order.AddressId = adress.Addressid;
        }
        else
        {
            throw new InvalidOperationException("Address not found");
        }
        
        var allOrders = _context.orders.ToArray().ToList();
        
        dbContext.orders.Add(order);
        dbContext.SaveChanges();

        var allOrdersToCompare = _context.orders.ToArray().ToList();

        var uniqueOrder = allOrders.Concat(allOrdersToCompare).Distinct().ToList();
        var orderId = uniqueOrder.Last().OrderId;
        
        
        var cart = _context.carts.FromSqlRaw("SELECT * FROM carts WHERE user_id = {0}", userId).ToArray();

        foreach (var item in cart)
        {
            
            var product = _context.products.FirstOrDefault(p => p.ProductId == item.ProductId);
            if (product == null)
            {
                throw new InvalidOperationException("Product not found");
            }
            
            Order.Item orderItem = new Order.Item(item.ProductId, item.Quantity, product.ProductPricePerUnit);
            order.Items.Add(orderItem);
            totalAmount += orderItem.Price * item.Quantity;
            createOrderDetailsForTheProduct(orderId, product, item.Quantity);
            
        }
        
        order.TotalAmount = totalAmount;
        dbContext.orders.Update(order);
        
        _context.SaveChanges();
    }

    public void createOrderDetailsForTheProduct(int orderId, Product product, int quantity)
    {
        OrderDetails orderDetails = new OrderDetails();
        //orderId and productId are the primary keys of the orderDetails table
        orderDetails.OrderId = orderId;
        orderDetails.ProductId = product.ProductId;

        orderDetails.Quantity = quantity;
        orderDetails.UnitPrice = product.ProductPricePerUnit;
        
        _context.orderDetails.Add(orderDetails);
        _context.SaveChanges();
    }

    public List<Order> ListOrders(int userId)
    {
        List<Order> orderList = new List<Order>();
        var orders = _context.orders.FromSqlRaw("SELECT * FROM orders WHERE user_id = {0}", userId).ToArray();
        
        foreach (var order in orders)
        {
            Order orderResponse = new Order();
            orderResponse.OrderId = order.OrderId;
            var orderDetails = _context.orderDetails.FromSqlRaw("SELECT * FROM order_items WHERE order_id = {0}", order.OrderId).ToArray();
            foreach (var orderDetail in orderDetails)
            {
                Order.Item item = new Order.Item(orderDetail.ProductId, orderDetail.Quantity, orderDetail.UnitPrice);
                orderResponse.Items.Add(item);
            }
            orderResponse.Address = _context.addresses.FirstOrDefault(a => a.Addressid == order.AddressId);
            orderList.Add(orderResponse);
        }

        return orderList;
    }
}