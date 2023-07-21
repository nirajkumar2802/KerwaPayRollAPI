namespace Version.KerwaException
{
   

    public class KerwaAppException : Exception
    {
        public KerwaAppException(string message) : base(message)
        {
        }

        public KerwaAppException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
