using Telegram.Bot;
using veve.Authentication;
using veve.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddLogging();

builder.Services.AddAuthentication().AddScheme<ApiKeyAuthenticationSchemeOptions, ApiKeyAuthenticationHandler>(ApiKeyDefaults.AuthenticationScheme, (options) =>
{
    builder.Configuration.GetRequiredSection(ApiKeyDefaults.AuthenticationScheme).Bind(options);
});
builder.Services.AddAuthorization();

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
builder.Services.AddSingleton<ITelegramBotClient>(new TelegramBotClient(builder.Configuration["Telegram:Token"]!));
builder.Services.AddSingleton<TelegramService>();
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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
