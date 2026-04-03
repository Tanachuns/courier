using courier.Models.Dto;

namespace courier.Services;

public class ArguementService
{
    public ArguementService(LineEventDto eventDto)
    {
        string[]? args = eventDto?.Message?.Text?.Split(" ");
        if (args != null)
        {
            foreach (var a in args)
            {
                Console.WriteLine(a);
            }
        }
        //TODO: implement args classification 
        //TODO: help => show all command
        //TODO: finance || fin commands
    }
  
}