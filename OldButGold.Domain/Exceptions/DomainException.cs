namespace OldButGold.Domain.Exceptions
{
    public abstract class DomainException : Exception
    {
        public DomainErrorCode ErrorCode { get; }


        protected DomainException(DomainErrorCode errorCode, string message) : base(message)
        {
            ErrorCode = errorCode;
        }

    }

    
}
