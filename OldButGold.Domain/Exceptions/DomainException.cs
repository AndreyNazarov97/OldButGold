namespace OldButGold.Domain.Exceptions
{
    public abstract class DomainException : Exception
    {
        public ErrorCode ErrorCode { get; }


        protected DomainException(ErrorCode errorCode, string message) : base(message)
        {
            ErrorCode = errorCode;
        }

    }

    
}
