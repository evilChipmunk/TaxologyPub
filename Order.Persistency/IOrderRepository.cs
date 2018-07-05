using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
 

namespace Order.Persistency
{
    public interface IOrderRepository
    {
        Task<Domain.Order> GetMostRecentOrderByCustomer(Guid customerId);
        Task<IEnumerable<Order.Domain.Order>> GetCustomerOrders(Guid customerId);
        Task<Order.Domain.Order> SaveAsync(Order.Domain.Order order);
        Task<Domain.Order> GetOrder(Guid requestOrderId);

        Task<Domain.Order> GetOpenOrderByCustomer(Guid customerId);
    }
  
    public class OrderRepository : IOrderRepository
    {
        private readonly OrderContext context;

        public OrderRepository(OrderContext context)
        {
            this.context = context;
        }
        public async Task<Domain.Order> GetMostRecentOrderByCustomer(Guid customerId)
        {
            return await context.Orders.Where(x => x.CustomerId == customerId)
                .Include(x => x.BillingAddress)
               // .Include(x => x.Products)
                .OrderByDescending(x => x.OrderDate).FirstOrDefaultAsync();
        }


        public async Task<Domain.Order> SaveAsync(Domain.Order order)
        {


            var strategy = context.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                // Achieving atomicity between original Catalog database operation and the
                // IntegrationEventLog thanks to a local transaction
                using (var transaction = context.BeginTransaction())
                {

                    try
                    {
                        context.Save(order, context.Orders);

                        context.Save(order.BillingAddress, context.BillingAddress);

                        foreach (var product in order.Products)
                        {
                            context.Save(product, context.Products);
                        }

                        context.CommitTransaction();

                    }
                    catch (Exception ex)
                    {
                        context.RollbackTransaction();
                        throw;
                    }
                    finally
                    {
                        if (transaction != null)
                        {
                            transaction.Dispose(); 
                        }
                    }


                    //context.CatalogItems.Update(catalogItem);
                    //await context.SaveChangesAsync();
                    //// Save to EventLog only if product price changed
                    //if (raiseProductPriceChangedEvent)
                    //    await _integrationEventLogService.SaveEventAsync(priceChangedEvent);
                    //transaction.Commit();
                }
            });

                    await context.Publish(order);
                    await context.Publish(order.BillingAddress);
                    await context.Publish(order.Products);


            //context.BeginTransaction();
            //context.Save(order, context.Orders);

            //context.Save(order.BillingAddress, context.BillingAddress);
             
            //foreach (var product in order.Products)
            //{
            //    context.Save(product, context.Products);
            //}

            //context.CommitTransaction();

            //await context.Publish(order);
            //await context.Publish(order.BillingAddress);
            //await context.Publish(order.Products);

            return order;
        }

        public async Task<Domain.Order> GetOrder(Guid requestOrderId)
        {
            return await context.Orders.Include(x => x.BillingAddress).Include(x => x.Products)
                .FirstOrDefaultAsync(x => x.Id == requestOrderId);
        }

        public async Task<Domain.Order> GetOpenOrderByCustomer(Guid customerId)
        {
            return await context.Orders.Include(x => x.BillingAddress).Include(x => x.Products)
                .FirstOrDefaultAsync(x => x.CustomerId == customerId && x.OrderStatus == Domain.OrderStatus.Created); 
        }


        public async Task<IEnumerable<Domain.Order>> GetCustomerOrders(Guid customerId)
        {
            return await context.Orders.Include(x => x.BillingAddress).Include(x => x.Products)
                .Where(x => x.CustomerId == customerId).ToListAsync();
        }


        //this is probably the better method here due to the data size...but for right now its just easier
        //getting the full object
        //public async Task<IEnumerable<Domain.Order>> GetCustomerOrders(Guid customerId)
        //{
        //    return await context.Orders.Where(x => x.CustomerId == customerId).ToListAsync();
        //}
    }
}