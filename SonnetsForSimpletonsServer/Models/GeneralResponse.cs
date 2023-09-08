namespace SonnetsForSimpletonsServer.Models;

public class GeneralResponse : IGeneralResponse
{
    public bool Success { get; set; }
    public string Description { get; set; } = "An error occurred";
}