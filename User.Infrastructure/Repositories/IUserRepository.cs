using UserApp.Infrastructure.Entities;

namespace UserApp.Infrastructure.Repositories
{
    public interface IUserRepository
    {
        public string Create( User user );
        public User GetById( string id );
        public User GetByEmail( string email );
        public IEnumerable<User> GetAll();
        public User Update( User user );
        public void Delete( User user );
    }
}
