using server.Entities;
using server.Interfaces;

namespace server.Services;

public class UserService
{
    private readonly IUnitOfWork _unitOfWork;

    public UserService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public Profile GetProfile(Guid uuid)
    {
        return _unitOfWork.User.GetById(uuid);
    }
}