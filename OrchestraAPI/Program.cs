using OrchestraAPI.Context;
using Microsoft.OpenApi.Models;
using System.Reflection;
using OrchestraAPI.Repositories.Concerts;
using OrchestraAPI.Repositories.Instruments;
using OrchestraAPI.Repositories.Players;
using OrchestraAPI.Repositories.Sections;
using OrchestraAPI.Repositories.Users;
using OrchestraAPI.Services.Concerts;
using OrchestraAPI.Services.Enrollments;
using OrchestraAPI.Services.Instruments;
using OrchestraAPI.Services.Players;
using OrchestraAPI.Services.Sections;
using OrchestraAPI.Services.Users;
using OrchestraAPI.Services.Orchestras;
using OrchestraAPI.Repositories.Orchestras;
using OrchestraAPI.Services.Conductors;
using OrchestraAPI.Repositories.Conductors;
using OrchestraAPI.Repositories.Enrollments;
using OrchestraAPI.Services.PasswordHashing;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

// Add header documentation in swagger
builder.Services.AddSwaggerGen(options =>
{

    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Orchestra Management API",
        Description = "This is the best API for Orchestra Management!",
        Contact = new OpenApiContact
        {
            Name = "", // changes
            Url = new Uri("https://github.com/CITUCCS/csit327-project-quaranteam")
        },
    });
    //Feed generated xml api docs to swagger
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});
ConfigureServices(builder.Services);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

void ConfigureServices(IServiceCollection services)
{
    // Configure Automapper
    services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

    // Trasient -> create new instance of DapperContext everytime.
    services.AddTransient<DapperContext>();

    //Services
    services.AddScoped<IConductorService, ConductorService>();
    services.AddScoped<IConcertService, ConcertService>();
    services.AddScoped<IInstrumentService, InstrumentService>();
    services.AddScoped<IPlayerService, PlayerService>();
    services.AddScoped<ISectionService, SectionService>();
    services.AddScoped<IEnrollmentService, EnrollmentService>();
    services.AddScoped<IOrchestraService, OrchestraService>();
    services.AddScoped<IUserService, UserService>();
    services.AddScoped<IEnrollmentService, EnrollmentService>();
    services.AddScoped<IInstrumentService, InstrumentService>();
    services.AddScoped<IPasswordService, PasswordService>();


    //Repository
    services.AddScoped<IConductorRepository, ConductorRepository>();
    services.AddScoped<IConcertRepository, ConcertRepository>();
    services.AddScoped<IInstrumentRepository, InstrumentRepository>();
    services.AddScoped<IPlayerRepository, PlayerRepository>();
    services.AddScoped<ISectionRepository, SectionRepository>();
    services.AddScoped<IOrchestraRepository, OrchestraRepository>();
    services.AddScoped<IUserRepository, UserRepository>();
    services.AddScoped<IEnrollmentRepository, EnrollmentRepository>();
    services.AddScoped<IInstrumentRepository, InstrumentRepository>();
}