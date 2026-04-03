namespace courier.Models.Dto;

public class HookRequestDto
{
  public string destination { get; set; }
  public LineEventDto[] events { get; set; }
}