using SonnetsForSimpletonsServer.Game;
using SonnetsForSimpletonsServer.Lobby;
using SonnetsForSimpletonsServer.Lobby.Models;
using SonnetsForSimpletonsServer.Player;
using SonnetsForSimpletonsServer.Player.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR();

builder.Services.AddCors(options =>
{
    // TODO: Make named policies for both development and production environments
    options.AddDefaultPolicy(
        policyBuilder =>
        {
            policyBuilder.SetIsOriginAllowed(origin => new Uri(origin).IsLoopback)
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
});

builder.Services.AddSingleton<IRoomFacade, RoomFacade>();
builder.Services.AddSingleton<IRoomCodeGenerator, RoomCodeGenerator>();
builder.Services.AddSingleton<IPlayerFacade, PlayerFacade>();
builder.Services.AddTransient<IPlayer, Player>();
builder.Services.AddTransient<IRoom, Room>();

var app = builder.Build();

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment()) {}

app.UseHttpsRedirection();

app.UseCors();

app.MapHub<LobbyHub>("lobby");
app.MapHub<GameHub>("game");

app.Run();