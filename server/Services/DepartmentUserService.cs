

using server.Entities;
using server.Interfaces;
using Microsoft.AspNetCore.JsonPatch;
namespace server.Services
{
    public class DepartmentUserService : IDepartmentUserService
    {
        private readonly IDepartmentUser _repository;

        public DepartmentUserService(IDepartmentUser repository)
        {
            _repository = repository;
        }

        public IEnumerable<DepartmentUser> GetAll()
        {
            return _repository.GetAll();
        }

        public DepartmentUser GetById(long id)
        {
            return _repository.Get(x => x.Id == id) ?? throw new KeyNotFoundException("DepartmentUser not found");
        }

        public DepartmentUser Create(DepartmentUser departmentUser)
        {
            departmentUser.CreatedAt = DateTime.UtcNow;
            var created = _repository.Add(departmentUser);
            _repository.Save();
            return created;
        }

        public DepartmentUser Update(DepartmentUser departmentUser)
        {
            var updated = _repository.Update(departmentUser);
            _repository.Save();
            return updated;
        }

        public DepartmentUser Delete(long id)
        {
            var departmentUser = _repository.GetById(id.ToString());
            if (departmentUser == null)
            {
                throw new KeyNotFoundException("DepartmentUser not found");
            }

            _repository.Remove(departmentUser);
            _repository.Save(); // Đảm bảo thay đổi được lưu
            return departmentUser;
        }


        public DepartmentUser UpdatePatch(long id, JsonPatchDocument<DepartmentUser> patch)
        {
            var departmentUser = _repository.GetById(id.ToString());
            if (departmentUser == null)
            {
                throw new KeyNotFoundException("DepartmentUser not found");
            }

            // Áp dụng patch
            _repository.UpdatePatch(id.ToString(), patch);

            return departmentUser;  // Trả về DepartmentUser sau khi cập nhật
        }


    }
}

