namespace UserApp.Api.Exceptions
{
    public class InvalidAuthenticationRequestCredentials : ApplicationException
    {
        public InvalidAuthenticationRequestCredentials()
            : base( "Invalid Email or Passowrd" )
        {

        }

        public InvalidAuthenticationRequestCredentials( string message )
            : base( message )
        {

        }
    }
}
