namespace GlobalExceptionHandler.Api;

public class NotFoundException : Exception
{
    public NotFoundException(int Id) : base($"Entity with Id {Id} not found")
    {
    }
}
