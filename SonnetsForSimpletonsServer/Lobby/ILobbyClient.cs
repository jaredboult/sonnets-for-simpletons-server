using SonnetsForSimpletonsServer.Lobby.Responses;
using SonnetsForSimpletonsServer.Player.Responses;

namespace SonnetsForSimpletonsServer.Lobby;

public interface ILobbyClient
{
    public Task UpdateRoomDetails(RoomResponse response);
    public Task SavePlayerId(PlayerResponse response);
    public Task StartGame(GeneralResponse response);
}