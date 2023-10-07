namespace SonnetsForSimpletonsServer.Player.Models;

public class Player : IPlayer
{
    public string Name { get; set; } = "Anonymous";
    public string? ConnectionId { get; set; }
    public Guid Id { get; init; }
}