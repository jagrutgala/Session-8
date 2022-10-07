using UserApp.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using UserApp.Infrastructure.DatabaseContexts;

namespace UserApp.Infrastructure.Repositories
{
    public class UserSqlRepository : IUserRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public UserSqlRepository( ApplicationDbContext dbContext )
        {
            _dbContext = dbContext;
        }
        public string Create( User user )
        {
            user.CreatedTime = DateTime.Now;
            _dbContext.AddAsync( user );
            _dbContext.SaveChanges();

            return user.Id;
        }

        public void Delete( User user )
        {
            user.DeletedTime = DateTime.Now;
            _dbContext.Remove( user );
            _dbContext.SaveChanges();
        }

        public IEnumerable<User> GetAll()
        {
            return _dbContext.Users
                .ToList();
        }

        public User GetByEmail( string email )
        {
            return _dbContext.Users
                .FirstOrDefault( u => u.Email == email );
        }

        public User GetById( string id )
        {
            return _dbContext.Users
                .FirstOrDefault( u => u.Id == id );
        }

        public User Update( User user )
        {
            user.LastUpdatedTime = DateTime.Now;
            _dbContext.Users.Update( user );
            //_dbContext.Entry( user ).State = EntityState.Modified;
            _dbContext.SaveChanges();
            return user;
        }
    }
}
