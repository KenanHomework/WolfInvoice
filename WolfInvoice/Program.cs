using Microsoft.EntityFrameworkCore;
using WolfInvoice.Data;
using WolfInvoice.Extensions;

namespace WolfInvoice;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddDbContext<WolfInvoiceContext>(
            options => options.UseSqlite("Data Source=WolfInvoiceDB")
        );

        builder.Services.AddControllers();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // DI
        DI.UseSerilog();
        builder.Services.AddFluentValidation();
        builder.Services.AddConfigs(builder.Configuration);
        builder.Services.AddSwagger();
        builder.Services.AddServices();
        builder.Services.AuthenticationAndAuthorization(builder.Configuration);

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(x => x.EnablePersistAuthorization());
        }

        app.UseHttpsRedirection();

        app.UseAuthentication();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
