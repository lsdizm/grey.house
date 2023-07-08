using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ListenAnyIP(8091);
    serverOptions.ListenAnyIP(8090, (httpsOpt) => {
        httpsOpt.UseHttps();
    });
});

// builder.Services.AddSingleton<IConfigurations, Configurations>();
// builder.Services.AddScoped<IDataService, DataService>();
// builder.Services.AddScoped<IDataBases, DataBases>();
// builder.Services.AddScoped<IDataAPI, DataAPI>();

builder.Services.AddCors(options =>{
    options.AddDefaultPolicy(
        policyBuilder => policyBuilder
            .AllowAnyMethod()
            .AllowAnyHeader()
            .WithOrigins(new string[]{
                "http://localhost:8080",
                              
            })
    );

    options.AddPolicy("MjServicePolicy",
        policyBuilder => policyBuilder
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowAnyOrigin()
    );
});


var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpLogging();
app.UseHttpsRedirection();
app.UseCertificateForwarding();
app.UseAuthorization();
app.MapControllers();

app.UseCors();


app.UseForwardedHeaders(new ForwardedHeadersOptions 
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

app.Run();

/*var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
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

app.UseAuthorization();

app.MapControllers();

app.Run();
*/