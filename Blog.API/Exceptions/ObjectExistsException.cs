namespace Blog.API.Exceptions
{
    public class ObjectExistsException : Exception
    {
        public ObjectExistsException(string message) : base(message)
        {

        }
    }
}
