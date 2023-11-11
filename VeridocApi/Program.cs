using App.Bal.Services;
using App.Bal.Repositories;
using App.Dal;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddTransient<IMailService, MailService>();
builder.Services.AddTransient<ICaptchaService, CaptchaService>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IPaymentService, PaymentService>();
builder.Services.AddTransient<IStorageService, StorageService>();
builder.Services.AddTransient<IMainAppService, MainAppService>();

builder.Services.AddDbContext<AppDbContext>(config => { config.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")); });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(option =>
{
    option.AddPolicy("default", builder =>
    {
        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{

//}
app.UseSwagger();
app.UseSwaggerUI();
app.UseCors("default");
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
