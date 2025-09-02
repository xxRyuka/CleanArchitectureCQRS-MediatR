using CleanArchitectureCQRS.Application.Services;
using CleanArchitectureCQRS.Persistence;
using CleanArchitectureCQRS.Persistence.Service;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ICarService, CarService>();

builder.Services.AddControllers()
    .AddApplicationPart(typeof(CleanArchitectureCQRS.Presentation.PresentationAssembly).Assembly);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddMediatR(opt =>
{
    opt.RegisterServicesFromAssembly(typeof(CleanArchitectureCQRS.Application.ApplicationAssembly).Assembly);
});
builder.Services.AddPersistanceServices(builder.Configuration);


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();