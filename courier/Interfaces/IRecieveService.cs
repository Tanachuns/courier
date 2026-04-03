using courier.Models.Dto;

namespace courier.Interfaces;

public interface IRecieveService
{
    public bool ValidateSignature(string body, string signature);
}