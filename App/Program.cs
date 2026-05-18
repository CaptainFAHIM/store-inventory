using BLL.Services;
using DAL.EF;
using DAL.EF.Tables;
using DAL.Repos;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<PmsCSp26Context>(opt => {
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DbConn"));
});
builder.Services.AddScoped<CategoryRepo>();
builder.Services.AddScoped<ProductRepo>();
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<CategoryService>();
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<PmsCSp26Context>();
    db.Database.EnsureCreated();

    if (!db.Categories.Any())
    {
        db.Categories.AddRange(
            new Category { Name = "Electronics" },
            new Category { Name = "Office Supplies" },
            new Category { Name = "Furniture" }
        );
        db.SaveChanges();
    }

    if (!db.Products.Any())
    {
        var electronics = db.Categories.FirstOrDefault(x => x.Name == "Electronics");
        var office = db.Categories.FirstOrDefault(x => x.Name == "Office Supplies");
        var furniture = db.Categories.FirstOrDefault(x => x.Name == "Furniture");

        if (electronics != null && office != null && furniture != null)
        {
            db.Products.AddRange(
                new Product { Name = "Laptop", Price = 78000, Qty = 8, Cid = electronics.Id },
                new Product { Name = "Printer Paper Pack", Price = 450, Qty = 35, Cid = office.Id },
                new Product { Name = "Office Chair", Price = 14500, Qty = 6, Cid = furniture.Id }
            );
            db.SaveChanges();
        }
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Product}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
