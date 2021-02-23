using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using PersonelTakip.Models;

namespace PersonelTakip.DataAccess
{
    class EmployeeService
    {
        private Context db = new Context();

        public void GetAll()
        {
            var employees = db.Employees.ToList();
        }

        public void GetById(int id)
        {
            var employees = db.Employees.FirstOrDefault(x => x.Id == id);
        }

        public void Add(Employee employees)
        {
            db.Employees.Add(employees);
            db.SaveChanges();
        }

        public void Update(Employee employees)
        {
            var updatedEmployee = db.Employees.Find(employees.Id);

            updatedEmployee.StaffCode = employees.StaffCode;
            updatedEmployee.FirstName = employees.FirstName;
            updatedEmployee.LastName = employees.LastName;
            updatedEmployee.Birthdate = employees.Birthdate;
            updatedEmployee.CellPhone = employees.CellPhone;
            updatedEmployee.Email = employees.Email;
            updatedEmployee.IdNumber = employees.IdNumber;
            updatedEmployee.Gender = employees.Gender;
            updatedEmployee.Payments = employees.Payments;
            updatedEmployee.Nationality = employees.Nationality;
            updatedEmployee.StarDate = employees.StarDate;
            updatedEmployee.EndDate = employees.EndDate;
            updatedEmployee.EmployeeType = employees.EmployeeType;
            updatedEmployee.EmployeeStatus = employees.EmployeeStatus;
            updatedEmployee.DistrictId = employees.DistrictId;
            updatedEmployee.DeparmantId = employees.DeparmantId;
            updatedEmployee.AgiId = employees.AgiId;
            updatedEmployee.TaskId = employees.TaskId;
            updatedEmployee.Amount = employees.Amount;

            db.SaveChanges();

        }

        public void Delete(int id)
        {
            var employess = db.Employees.Find(id);

            db.Employees.Remove(employess);
            db.SaveChanges();
        }

        public List<Contact> GetEmplooyeeContacts(int employeeId)
        {
            return db.Contacts.Where(x => x.EmployeeId == employeeId).ToList();
        }

        public List<Permission> GetEmployeePermissions(int employeeId)
        {
            return db.Permissions.Where(x => x.EmployeeId == employeeId).ToList();
        }

        public List<Payment> GetEmployeePayment(int employeeId)
        {
            return db.Payments.Where(x => x.EmployeeId == employeeId).ToList();
        }

    }
}
