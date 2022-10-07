namespace UserApp.Api.Exceptions
{
    public class UserAlreadyExistsError : ApplicationException
    {
        public UserAlreadyExistsError()
            : base( "User already exists" )
        {

        }

        public UserAlreadyExistsError( string message )
            : base( message )
        {

        }
    }
}
