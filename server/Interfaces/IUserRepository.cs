using server.Entities;

namespace server.Interfaces;

public interface IUserRepository : IRepository<Profile>
{
    Profile IRepository<Profile>.GetById(int id)
    {
        throw new NotImplementedException();
    }

    public Profile GetById(Guid id);
}