namespace UserApp.Api.Exceptions
{
    public class UserNotFoundError : ApplicationException
    {
        public UserNotFoundError()
            : base( "User not found" )
        {

        }

        public UserNotFoundError( string message )
            : base( message )
        {

        }
    }
}
