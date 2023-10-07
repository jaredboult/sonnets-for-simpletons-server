using SonnetsForSimpletonsServer.Game.Responses;
using SonnetsForSimpletonsServer.Lobby.Responses;
using SonnetsForSimpletonsServer.Player.Responses;

namespace SonnetsForSimpletonsServer.Lobby;

public interface ILobbyClient
{
    public Task UpdateRoomDetails(RoomResponse response);
    public Task SavePlayerId(PlayerResponse response);
}