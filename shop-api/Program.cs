using Microsoft.EntityFrameworkCore;
using shop_api;
using shop_api.Models;
using shop_api.Validation;

var builder = WebApplication.CreateBuilder(args);


//builder.Services.AddDbContext<ShopDbContext>(opt => opt.UseInMemoryDatabase("ShopDB"));
builder.Services.AddDbContext<ShopDbContext>(opt => opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/", () => "Hello World");

app.MapPost("/products", async (Product product, ShopDbContext db) =>
{
    ProductValidator productValidator = new ProductValidator();
    var result = await productValidator.ValidateAsync(product);
    if(result.IsValid)
    {
        db.Products.Add(product);
        await db.SaveChangesAsync();
        return Results.Ok(product);
    }
    else
    {
        return Results.BadRequest(result.Errors);
    }
});

app.MapGet("/products", async (ShopDbContext db) =>
    await db.Products.ToListAsync());

app.MapPut("/products/{id}", async (int id, Product product, ShopDbContext db) =>
{
    ProductValidator productValidator = new ProductValidator();
    var result = await productValidator.ValidateAsync(product);
    if (result.IsValid)
    {
        var updatedProduct = await db.Products.FindAsync(id);
        if (product is null) return Results.NotFound();
        updatedProduct.ProductName = product.ProductName;
        updatedProduct.CategoryId = product.CategoryId;
        updatedProduct.UnitInStock = product.UnitInStock;
        updatedProduct.UnitPrice = product.UnitPrice;
        await db.SaveChangesAsync();
        return Results.Ok(updatedProduct);
    }
    else
    {
        return Results.BadRequest(result.Errors);
    }
});

app.MapGet("/products/{id}", async (int id, ShopDbContext db) =>
    await db.Products.FindAsync(id)
        is Product product
        ? Results.Ok(product)
        : Results.NotFound()
);

app.MapDelete("/products/{id}", async (int id, ShopDbContext db) =>
{
    var deletedProduct = await db.Products.FindAsync(id);
    if (deletedProduct is null) return Results.NotFound();
    db.Remove(deletedProduct);
    await db.SaveChangesAsync();
    return Results.Ok(deletedProduct);
});


app.MapPost("/categories", async (Category category, ShopDbContext db) =>
{
    CategoryValidator categoryValidator = new CategoryValidator();
    var result = await categoryValidator.ValidateAsync(category);
    if (result.IsValid)
    {
        db.Categories.Add(category);
        await db.SaveChangesAsync();
        return Results.Created($"/categories/{category.Id}", category);
    }
    else
    {
        return Results.BadRequest(result.Errors);
    }
});

app.MapGet("/categories", async (ShopDbContext db) =>
    await db.Categories.ToListAsync());

app.MapGet("/categories/{id}", async (int id, ShopDbContext db) =>
    await db.Categories.FindAsync(id)
        is Category category
        ? Results.Ok(category)
        : Results.NotFound());

app.MapPut("/categories/{id}", async (int id, Category category, ShopDbContext db) =>
{
    CategoryValidator categoryValidator = new CategoryValidator();
    var result = await categoryValidator.ValidateAsync(category);
    if (result.IsValid)
    {
        var updatedCategory = await db.Categories.FindAsync(id);
        if (updatedCategory is null) return Results.NotFound();
        updatedCategory.CategoryName = category.CategoryName;
        await db.SaveChangesAsync();
        return Results.Ok(updatedCategory);
    }
    else
    {
        return Results.BadRequest(result.Errors);
    }
});


app.MapDelete("/categories/{id}", async (int id, ShopDbContext db) =>
{
    var deletedCategory = await db.Categories.FindAsync(id);
    if(deletedCategory is null) return Results.NotFound();
    db.Remove(deletedCategory);
    await db.SaveChangesAsync();
    return Results.Ok(deletedCategory);
});



app.MapPost("/orders", async (Order order, ShopDbContext db) =>
{
    OrderValidator orderValidator = new OrderValidator();
    var result = await orderValidator.ValidateAsync(order);
    if (result.IsValid)
    {
        db.Orders.Add(order);
        await db.SaveChangesAsync();
        return Results.Ok(order);
    }
    else
    {
        return Results.BadRequest(result.Errors);
    }
});

app.MapGet("/orders", async (ShopDbContext db) =>
    await db.Orders.ToListAsync());

app.MapPut("/orders/{id}", async (int id, Order order, ShopDbContext db) =>
{
    OrderValidator orderValidator = new OrderValidator();
    var result = await orderValidator.ValidateAsync(order);
    if (result.IsValid)
    {
        var updatedOrder = await db.Orders.FindAsync(id);
        if (order is null) return Results.NotFound();
        updatedOrder.ProductId = order.ProductId;
        updatedOrder.Quantity = order.Quantity;
        await db.SaveChangesAsync();
        return Results.Ok(updatedOrder);
    }
    else
    {
        return Results.BadRequest(result.Errors);
    }
});

app.MapGet("/orders/{id}", async (int id, ShopDbContext db) =>
    await db.Orders.FindAsync(id)
        is Order order
        ? Results.Ok(order)
        : Results.NotFound()
);

app.MapDelete("/orders/{id}", async (int id, ShopDbContext db) =>
{
    var deletedOrder = await db.Orders.FindAsync(id);
    if (deletedOrder is null) return Results.NotFound();
    db.Remove(deletedOrder);
    await db.SaveChangesAsync();
    return Results.Ok(deletedOrder);
});



app.Run();

