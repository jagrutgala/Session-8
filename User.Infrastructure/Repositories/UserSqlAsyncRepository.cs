using UserApp.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using UserApp.Infrastructure.DatabaseContexts;

namespace UserApp.Infrastructure.Repositories
{
    public class UserSqlAsyncRepositry : IUserRepositoryAsync
    {
        private readonly ApplicationDbContext _dbContext;

        public UserSqlAsyncRepositry( ApplicationDbContext dbContext )
        {
            _dbContext = dbContext;
        }
        public async Task<string> Create( User user )
        {
            user.CreatedTime = DateTime.Now;
            _dbContext.AddAsync( user );
            _dbContext.SaveChangesAsync();

            return user.Id;
        }

        public async void Delete( User user )
        {
            user.DeletedTime = DateTime.Now;
            _dbContext.Remove( user );
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return await _dbContext.Users
                .ToListAsync();
        }

        public async Task<User> GetByEmail( string email )
        {
            return await _dbContext.Users
                .FirstOrDefaultAsync( u => u.Email == email );
        }

        public async Task<User> GetById( string id )
        {
            return await _dbContext.Users
                .FirstOrDefaultAsync( u => u.Id == id );
        }

        public async Task<User> Update( User user )
        {
            user.LastUpdatedTime = DateTime.Now;
            _dbContext.Users.Update( user );
            //_dbContext.Entry( user ).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return user;
        }
    }
}
