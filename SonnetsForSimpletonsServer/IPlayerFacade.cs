using SonnetsForSimpletonsServer.Models;

namespace SonnetsForSimpletonsServer;

public interface IPlayerFacade
{
    public Player CreatePlayer(string connectionId);
    public bool UpdatePlayerName(string name, string connectionId);
}