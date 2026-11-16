// using KodaStore.Infrastructure.Persistence;
using KodaStore.Infrastructure.Persistence;
using KodaStore.Application.Repositories;
using KodaStore.Infrastructure.Seeding;
using KodaStore.Application.Services;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using KodaStore.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddOpenApi();
builder.Services.AddProblemDetails();
// builder.Services.AddSingleton<InMemoryProductCatalog>();

// builder.Services.AddSingleton<IProductRepository, InMemoryProductRepository>();
// builder.Services.AddSingleton<IProductService, ProductService>();

// Repository SQL Server
// builder.Services.AddScoped<
//     IProductRepository,
//     SqlProductRepository>();

// Repository
builder.Services.AddScoped<IProductRepository, EfProductRepository>();
builder.Services.AddScoped<IOrderRepository, EfOrderRepository>();
builder.Services.AddScoped<IProductQueryRepository, EfProductQueryRepository>();
builder.Services.AddScoped<ICategoryRepository, EfCategoryRepository>();
// Service
builder.Services.AddScoped<IProductService,ProductService>();
builder.Services.AddScoped<IOrderService,OrderService>();
builder.Services.AddScoped<IProductQueryService, ProductQueryService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();

var connectionString = builder.Configuration.GetConnectionString("ecommercedb")
        ?? throw new InvalidOperationException("ConnectionString : Belum dikonfigurasi");

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(connectionString)
        .UseSeeding((context, _) =>
        {
            ProductSeedData.Seed(context);
            CategorySeedData.Seed(context);
            OrderSeedData.Seed(context);
        })
        .UseAsyncSeeding(async (context, _, ct) =>
        //    SeedAllAsync(context, ct)
            {
                await ProductSeedData.SeedAsync(context, ct);
                await CategorySeedData.SeedAsync(context, ct);
                await OrderSeedData.SeedAsync(context, ct);
            });
});

builder.Services.AddRateLimiter(option =>
{
    option.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    option.AddConcurrencyLimiter("category-writes", limiter =>
    {
        limiter.PermitLimit = 8;
        limiter.QueueLimit = 0;
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseRateLimiter();

app.MapControllers();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

// static async Task SeedAllAsync(DbContext context, CancellationToken ct)
// {
//     await ProductSeedData.SeedAsync(context, ct);
//     await OrderSeedData.SeedAsync(context, ct);
// }