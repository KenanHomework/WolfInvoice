using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using WolfInvoice.Data;
using WolfInvoice.Models.DataModels;
using WolfInvoice.Services.EntityService;

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var context = new WolfInvoiceContext(
                new DbContextOptionsBuilder<WolfInvoiceContext>()
                    .UseInMemoryDatabase("test")
                    .Options
            );

            var user = new User()
            {
                Id = "user1",
                Name = "kanan",
                Email = "k@gmail.com",
                Password = "P@ass1234"
            };

            var customer1 = new Customer()
            {
                Id = "customer1",
                Name = "Customer 1",
                Email = "customer1@gmail.com",
                User = user
            };

            var invoice = new Invoice()
            {
                Id = "invoice1",
                User = user,
                Customer = customer1,
                Comment = "salam"
            };
            var userService = new UserService(
                context,
                new WolfInvoice.Services.CryptService(new WolfInvoice.Configurations.BCryptConfig())
            );
            var service = new InvoiceService(context, userService);

            context.Users.Add(user);
            context.Customers.Add(customer1);
            context.Invoices.Add(invoice);

            context.SaveChanges();

            Console.WriteLine("salam");

            var b = context.Invoices.ToList();
            var c = context.Invoices.First(i => i.User.Id == user.Id && i.Id == invoice.Id);
            var a = service.GetInvoice(user.Id, invoice.Id).Result;
            string json = JsonSerializer.Serialize(a);
            Console.WriteLine(json);
        }
    }
}
