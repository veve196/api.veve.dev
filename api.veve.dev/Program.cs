using veve.Authentication;
using veve.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(ApiKeyDefaults.AuthenticationScheme);
builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = options.DefaultPolicy;
});

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(corsBuilder =>
    {
        corsBuilder.WithOrigins(builder.Configuration["AllowedHosts"]!.Split(';'))
                   .AllowAnyHeader()
                   .AllowAnyMethod();
    });
});

builder.Services.AddSingleton<DiscordService>();

var app = builder.Build();

var discordService = app.Services.GetRequiredService<DiscordService>();
await discordService.Client.ConnectAsync();

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
