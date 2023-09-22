using SonnetsForSimpletonsServer.Lobby.Responses;

namespace SonnetsForSimpletonsServer.Lobby;

public interface ILobbyClient
{
    public Task UpdateRoomDetails(RoomResponse response);
}