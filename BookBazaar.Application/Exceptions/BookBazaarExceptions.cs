namespace BookBazaar.Application.Exceptions 
{
    public class BookBazaarException : Exception
    {
        public BookBazaarException(string message) : base(message) { }
    }

    // Common logical errors

    public class InValidData : BookBazaarException
    {
        public InValidData(string message) : base(message) { }
    }
    public class NotFoundException : BookBazaarException
    {
        public NotFoundException(string message) : base(message) { }
    }

    public class ForbiddenOperationException : BookBazaarException
    {
        public ForbiddenOperationException(string message) : base(message) { }
    }

    public class UnauthorizedException : BookBazaarException
    {
        public UnauthorizedException(string message) : base(message) { }
    }

}

