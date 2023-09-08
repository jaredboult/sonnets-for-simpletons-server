namespace SonnetsForSimpletonsServer.Models;

public interface IGeneralResponse
{
    public bool Success { get; set; }
    public string Description { get; set; }
}