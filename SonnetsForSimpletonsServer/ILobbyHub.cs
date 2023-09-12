using SonnetsForSimpletonsServer.Models.Messages;

namespace SonnetsForSimpletonsServer;

public interface ILobbyHub
{
    public Task CreateRoom(RoomResponse response);
    public Task JoinRoom(RoomResponse response);
    public Task UpdatePlayerName(PlayerResponse response);
}