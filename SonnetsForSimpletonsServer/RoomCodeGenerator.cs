namespace SonnetsForSimpletonsServer;

public class RoomCodeGenerator : IRoomCodeGenerator
{
    /* In the current implementation, a four letter code length allows 143,640 unique room codes
     as no letters appear more than once. */
    private const byte CodeLength = 4;

    private readonly Random _randomNumberGenerator = new();
    private readonly char[] _alphabet =
    {
        'B', 'C', 'D', 'F', 'G', 'H', 'J', 'K', 'L', 'M', 'N', 'P', 'Q', 'R', 'S',
        'T', 'V', 'W', 'X', 'Y', 'Z'
    };

    public string GenerateRoomCode()
    {
        var code = "";
        while (code.Length != CodeLength)
        {
            var letter = GetRandomLetter();
            if (!code.Contains(letter))
            {
                code += letter;
            }
        }
        return code;
    }

    private char GetRandomLetter()
    {
        var index = _randomNumberGenerator.Next(_alphabet.Length);
        return _alphabet[index];
    }
}