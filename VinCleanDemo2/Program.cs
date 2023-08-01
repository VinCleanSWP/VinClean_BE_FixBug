using MailKit;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;

using System.Text.Json.Serialization;

using VinClean.Repo.Models;
using VinClean.Repo.Repository;
using VinClean.Service.DTO;
using VinClean.Service.Service;




var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ServiceAppContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("VinClean")));


builder.Services.AddCors(p => p.AddPolicy("VinClean", build =>
{
    build.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
}));


builder.Services.AddAuthentication()
    .AddGoogle(options =>
    {
        options.ClientId = builder.Configuration["Google:AppId"];
        options.ClientSecret = builder.Configuration["Google:AppSecret"];
        options.ClaimActions.MapJsonKey("Picture", "picture", "url");
        options.SaveTokens = true;
        options.CallbackPath = builder.Configuration["Google:CallbackPath"];
    });


builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddAutoMapper(typeof(MappingConfig));

//builder.Services.AddControllers().AddJsonOptions(options =>
//{
//    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
//});

builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IAccountService, AccountService>();

builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();

builder.Services.AddScoped<IBlogRepository, BlogRepository>();
builder.Services.AddScoped<IBlogService, BlogService>();

builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<ICommentService, CommentService>();

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();

builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IRoleService, RoleService>();

builder.Services.AddScoped<IServiceRepository, ServiceRepository>();
builder.Services.AddScoped<IServiceService, ServiceService>();

builder.Services.AddScoped<IServiceWorkInRepository, ServiceWorkInRepository>();
builder.Services.AddScoped<IServiceManageService, ServiceManageService>();

builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderService, OrderService>();

builder.Services.AddScoped<IRatingRepository, RatingRepository>();
builder.Services.AddScoped<IRatingService, RatingService>();

builder.Services.AddScoped<IOrderRequestRepository, OrderRequestRepository>();
builder.Services.AddScoped<IProcessSlotService, OrderRequestService>();

builder.Services.AddScoped<ITypeRepository, TypeRepository>();
builder.Services.AddScoped<ITypeService, TypeService>();


builder.Services.AddScoped<IWorkingByService, LocationService>();
builder.Services.AddScoped<ILocationRepository, LocationRepository>();

builder.Services.AddScoped<IOrderImageService, OrderImageService>();
builder.Services.AddScoped<IOrderImageRepository, OrderImageRepository>();

builder.Services.AddScoped<IBuildingRepository, BuildingRepository>();
builder.Services.AddScoped<IBuildingService, BuildingService>();

builder.Services.AddScoped<IBuildingTypeRepository, BuildingTypeRepository>();
builder.Services.AddScoped<IBuildingTypeService, BuildingTypeService>();


builder.Services.AddScoped<IEmailService, EmailService>();



builder.Services.AddAutoMapper(typeof(MappingConfig));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("VinClean");


app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
