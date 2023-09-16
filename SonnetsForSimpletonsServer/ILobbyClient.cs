using SonnetsForSimpletonsServer.Models.Messages;

namespace SonnetsForSimpletonsServer;

public interface ILobbyClient
{
    public Task CreateRoom(RoomResponse response);
    public Task JoinRoom(RoomResponse response);
    public Task UpdatePlayerName(PlayerResponse response);
}