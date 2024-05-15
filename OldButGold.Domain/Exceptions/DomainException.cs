namespace OldButGold.Domain.Exceptions
{
    public abstract class DomainException(DomainErrorCode errorCode, string message) : Exception(message)
    {
        public DomainErrorCode ErrorCode { get; } = errorCode;
    }

    
}
