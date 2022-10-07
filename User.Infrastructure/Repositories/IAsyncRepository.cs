using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UserApp.Infrastructure.Entities;

namespace UserApp.Infrastructure.Repositories
{
    public interface IUserRepositoryAsync
    {
        public Task<string> Create( User user );

        public void Delete( User user );

        public Task<IEnumerable<User>> GetAll();

        public Task<User> GetByEmail( string email );

        public Task<User> GetById( string id );

        public Task<User> Update( User user );
    }
}
