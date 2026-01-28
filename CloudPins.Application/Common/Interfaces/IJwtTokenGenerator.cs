namespace CloudPins.Application.Common.Interfaces;

public interface IJwtTokenGenerator
{
    string Generate(Guid userId, string email);
}

