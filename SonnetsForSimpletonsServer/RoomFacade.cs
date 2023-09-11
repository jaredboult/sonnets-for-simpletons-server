using SonnetsForSimpletonsServer.Models;
using SonnetsForSimpletonsServer.Models.Lobby;
using ApplicationException = System.ApplicationException;

namespace SonnetsForSimpletonsServer;

public class RoomFacade : IRoomFacade
{
    private readonly IRoomCodeGenerator _roomCodeGenerator;
    private readonly HashSet<string> _roomCodesInUse = new ();
    private readonly Dictionary<string, Room> _rooms = new ();
    
    /* Setting an arbitrary maximum of 1,000 rooms for now */
    private const int MaxRooms = 1_000;

    public RoomFacade(IRoomCodeGenerator roomCodeGenerator)
    {
        _roomCodeGenerator = roomCodeGenerator;
    }
    
    public Room CreateRoom(Player host)
    {
        if (_roomCodesInUse.Count >= MaxRooms)
        {
            throw new ApplicationException("Room limit reached");
        }
        
        var roomCode = _roomCodeGenerator.GenerateRoomCode();
        
        while (!_roomCodesInUse.Add(roomCode))
        {
            roomCode = _roomCodeGenerator.GenerateRoomCode();
        }
        
        var room = new Room { RoomCode = roomCode };
        _rooms.Add(roomCode, room);
        
        room.AddPlayer(host);
        return room;
    }

    public bool JoinRoom(Player joiner, string roomCode)
    {
        var room = GetRoom(roomCode);
        room?.AddPlayer(joiner);
        return room is not null;
    }
    
    private Room? GetRoom(string roomCode)
    {
        return _rooms.TryGetValue(roomCode, out var room) ? room : null;
    }
}