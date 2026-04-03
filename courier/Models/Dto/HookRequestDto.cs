namespace courier.Models.Dto;

public class HookRequestDto
{
  public required string destination { get; set; }
    public required LineEventDto[] events { get; set; }
}