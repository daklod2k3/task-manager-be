using server.Entities;
using System.Linq.Expressions;

namespace server.Interfaces;
    public interface IDepartmentService
    {
        Department CreatDepartment(Department department);
        Department UpdateDepartment(Department department);
        Department DeleteDepartment(long idDepartment);
        public IEnumerable<Department> GetDepartmentByFilter(Expression<Func<Department, bool>> compositeFilterExpression);
    
    }
