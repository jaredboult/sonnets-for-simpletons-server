namespace SonnetsForSimpletonsServer.Player.Models;

public interface IPlayer
{
    string Name { get; set; }
    string? ConnectionId { get; set; }
}