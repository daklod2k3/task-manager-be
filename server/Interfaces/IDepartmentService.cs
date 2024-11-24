using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using server.Entities;
using System.Linq.Expressions;

namespace server.Interfaces;
    public interface IDepartmentService
    {
        public Department CreatDepartment(Department department);
        public Department UpdateDepartment(Department department);
        public Department DeleteDepartment(long idDepartment);
        public Department UpdateDepartmentPatch(long id, [FromBody] JsonPatchDocument<Department> patchDoc);
        public Department GetDepartment(long id);
        public IEnumerable<Department> GetDepartmentByFilter(Expression<Func<Department, bool>> compositeFilterExpression);
    
    }
