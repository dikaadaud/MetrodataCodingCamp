using System.Net;
using System.Reflection;
using System.Text;
using API.Contracts;
using API.Data;
using API.Repositories;
using API.Services;
using API.Utilities.Enums;
using API.Utilities.Handlers;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TokenHandler = API.Utilities.Handlers.TokenHandler;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
       .ConfigureApiBehaviorOptions(options =>
        {
            // Custom validation response
            options.InvalidModelStateResponseFactory = context =>
            {
                var errors = context.ModelState.Values
                                    .SelectMany(v => v.Errors)
                                    .Select(v => v.ErrorMessage);

                return new BadRequestObjectResult(new ResponseValidationHandler {
                    Code = StatusCodes.Status400BadRequest,
                    Status = HttpStatusCode.BadRequest.ToString(),
                    Message = "Validation error",
                    Errors = errors.ToArray()
                });
            };
        });

builder.Services.AddDbContext<BookingManagementDbContext>(option =>
                                                              option.UseSqlServer(builder.Configuration
                                                                 .GetConnectionString("DefaultConnection")));

// Register repositories
builder.Services.AddScoped<IUniversityRepository, UniversityRepository>();
builder.Services.AddScoped<IAccountRoleRepository, AccountRoleRepository>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IEducationRepository, EducationRepository>();
builder.Services.AddScoped<IRoomRepository, RoomRepository>();
builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
//builder.Services.AddTransient();
//builder.Services.AddSingleton();

// Register services
builder.Services.AddScoped<AccountService>();
builder.Services.AddScoped<AccountRoleService>();
builder.Services.AddScoped<BookingService>();
builder.Services.AddScoped<EducationService>();
builder.Services.AddScoped<EmployeeService>();
builder.Services.AddScoped<RoleService>();
builder.Services.AddScoped<RoomService>();
builder.Services.AddScoped<UniversityService>();

// Register Fluent validation
builder.Services.AddFluentValidationAutoValidation()
       .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

// Register Handler
builder.Services.AddScoped<GenerateHandler>();
builder.Services.AddScoped<ITokenHandler, TokenHandler>();

// Add SmtpClient
builder.Services.AddTransient<IEmailHandler, EmailHandler>(_ => new EmailHandler(
    builder.Configuration["EmailService:SmtpServer"],
    int.Parse(builder.Configuration["EmailService:SmtpPort"]),
    builder.Configuration["EmailService:FromEmailAddress"]
));

// Jwt Configuration
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
       .AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false; // For development
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidIssuer = builder.Configuration["JWTService:Issuer"],
                ValidateAudience = true,
                ValidAudience = builder.Configuration["JWTService:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWTService:Key"])),
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };
        });

// CORS Configuration
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin();
        policy.AllowAnyHeader();
        policy.AllowAnyMethod();
    });
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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

app.UseCors();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
