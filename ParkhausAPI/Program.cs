using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.ModelBuilder;
using ParkhausAPI.Data;
using ParkhausAPI.Models;

var builder = WebApplication.CreateBuilder(args);
var modelBuilder = new ODataConventionModelBuilder();
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
modelBuilder.EntitySet<Tickets>("Tickets");
modelBuilder.EntityType<Tickets>().HasKey(t => t.Id);
builder.Services.AddDbContext<ParkingContext>(options => options.UseSqlServer(connectionString));
var edmModel = modelBuilder.GetEdmModel();

builder.Services.AddControllers().AddOData(options => {
    options.Select()
           .Filter()
           .OrderBy()
           .Expand()
           .Count()
           .SetMaxTop(null)
           .AddRouteComponents("", edmModel);
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
   c.SwaggerEndpoint("/swagger/v1/swagger.json", "Parking Simulator API V1");
   c.RoutePrefix = string.Empty;
});

app.UseRouting();

app.MapControllers();

app.Run();