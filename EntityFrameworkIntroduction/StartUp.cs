using SoftUni.Data;
using SoftUni.Models;
using System;
using System.Globalization;
using System.Linq;
using System.Text;

namespace SoftUni
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            var db = new SoftUniContext();

            using (db)
            {
                //var date1 = new DateTime(2008, 5, 1, 8, 30, 52);
                //Console.WriteLine(date1);

                //3. Employees Full Information
                //Console.WriteLine(GetEmployeesFullInformation(db));

                //4.	Employees with Salary Over 50 000
                //Console.WriteLine(GetEmployeesWithSalaryOver50000(db));

                //5.	Employees from Research and Development
                //Console.WriteLine(GetEmployeesFromResearchAndDevelopment(db));

                //6.	Adding a New Address and Updating Employee
                //Console.WriteLine(AddNewAddressToEmployee(db));

                //7.	Employees and Projects
                //Console.WriteLine(GetEmployeesInPeriod(db));

                //8.	Addresses by Town
                //Console.WriteLine(GetAddressesByTown(db));

                //9.	Employee 147
                //Console.WriteLine(GetEmployee147(db));

                //10.	Departments with More Than 5 Employees
                //Console.WriteLine(GetDepartmentsWithMoreThan5Employees(db));

                //11.	Find Latest 10 Projects
                //Console.WriteLine(GetLatestProjects(db));

                //12.	Increase Salaries
                //Console.WriteLine(IncreaseSalaries(db));

                //13.	Find Employees by First Name Starting with "Sa"
                //Console.WriteLine(GetEmployeesByFirstNameStartingWithSa(db));

                //14.	Delete Project by Id
               // Console.WriteLine(DeleteProjectById(db));

               //15.	Remove Town
               Console.WriteLine(RemoveTown(db));
               //RemoveTown(db);

            }
        }
        public static string GetEmployeesFullInformation(SoftUniContext context)
        {
            var employees = context.Employees
            .Select(e => new
            {
                e.EmployeeId,
                e.FirstName,
                e.LastName,
                e.MiddleName,
                e.JobTitle,
                e.Salary
            })
            .OrderBy(e => e.EmployeeId)
            .ToList();

            var sb = new StringBuilder();

            foreach (var emp in employees)
            {
                sb.AppendLine(string.Join(' ',
                    //emp.EmployeeId,
                    emp.FirstName,
                    emp.LastName,
                    emp.MiddleName,
                    emp.JobTitle,
                    $"{emp.Salary:f2}"));
            }

            return sb.ToString().Trim();
        }

        public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
        {
            var employees = context.Employees
                .Select(e => new
                {
                    e.FirstName,
                    e.Salary
                })
                .Where(e => e.Salary > 50000)
                .OrderBy(e => e.FirstName)
                .ToList();

            var sb = new StringBuilder();

            foreach (var emp in employees)
            {
                sb.AppendLine(String.Join(" - ", emp.FirstName, $"{emp.Salary:f2}"));

            }

            return sb.ToString().Trim();
        }

        public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
        {
            var employees = context.Employees
                .Where(e => e.Department.Name == "Research and Development")
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    DeptName = e.Department.Name,
                    e.Salary
                })
                .OrderBy(e => e.Salary)
                .ThenByDescending(e => e.FirstName)
                .ToList();

            var sb = new StringBuilder();

            foreach (var emp in employees)
            {
                sb.AppendLine($"{emp.FirstName} {emp.LastName} from {emp.DeptName} - ${emp.Salary:f2}");
            }

            return sb.ToString().Trim();
        }

        public static string AddNewAddressToEmployee(SoftUniContext context)
        {
            var newAdress = new Address
            {
                AddressText = "Vitoshka 15",
                TownId = 4
            };

            var Nakov = context.Employees
                .FirstOrDefault(e => e.LastName == "Nakov");

            Nakov.Address = newAdress;

            context.SaveChanges();

            var employees = context.Employees
                .OrderByDescending(e => e.AddressId)
                .Take(10)
                .Select(e => new
                {
                    e.Address.AddressText
                })
                .ToList();

            var sb = new StringBuilder();

            foreach (var emp in employees)
            {
                sb.AppendLine(emp.AddressText);
            }

            return sb.ToString().Trim();
        }

        public static string GetEmployeesInPeriod(SoftUniContext context)
        {
            var employees = context.Employees
                .Where(e => e.EmployeesProjects
                .Any(e => e.Project.StartDate.Year >= 2001 && e.Project.StartDate.Year <= 2003))
                .Select(e => new
                {
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    ManagerFirstName = e.Manager.FirstName,
                    ManagerLastName = e.Manager.LastName,

                    Projects = e.EmployeesProjects
                        .Select(p => new
                        {
                            Name = p.Project.Name,
                            StartDate = p.Project.StartDate
                            .ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture),
                            EndDate = p.Project.EndDate.HasValue ?
                                            p.Project.EndDate.Value.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture) :
                                            "not finished"

                        })
                })
                .Take(10)
                .ToList();

            var sb = new StringBuilder();

            foreach (var emp in employees)
            {
                sb.AppendLine($"{emp.FirstName} {emp.LastName} - Manager: {emp.ManagerFirstName} {emp.ManagerLastName}");

                foreach (var prj in emp.Projects)
                {
                    sb.AppendLine($"--{prj.Name} - {prj.StartDate} - {prj.EndDate}");
                }
            }

            return sb.ToString().Trim();
        }

        public static string GetAddressesByTown(SoftUniContext context)
        {
            var addresses = context.Addresses
                .Select(a => new
                {
                    a.AddressText,
                    TownName = a.Town.Name,
                    EmployeeCount = a.Employees.Count
                })
                .OrderByDescending(a => a.EmployeeCount)
                .ThenBy(a => a.TownName)
                .ThenBy(a => a.AddressText)
                .Take(10);

            var sb = new StringBuilder();

            foreach (var a in addresses)
            {
                sb.AppendLine($"{a.AddressText}, {a.TownName} - {a.EmployeeCount} employees");
            }

            return sb.ToString().Trim();
        }

        public static string GetEmployee147(SoftUniContext context)
        {
            var employee = context.Employees
                .Select(e => new
                {
                    e.EmployeeId,
                    e.FirstName,
                    e.LastName,
                    e.JobTitle,
                    Projects = e.EmployeesProjects
                                .Select(p => p.Project.Name)
                                .OrderBy(p => p)
                                .ToList()
                })
                .SingleOrDefault(e => e.EmployeeId == 147);

            var sb = new StringBuilder();

            sb.AppendLine($"{employee.FirstName} {employee.LastName} - {employee.JobTitle}");

            foreach (var prj in employee.Projects)
            {
                sb.AppendLine(prj);
            }

            return sb.ToString().Trim();
        }

        public static string GetDepartmentsWithMoreThan5Employees(SoftUniContext context)
        {
            var departments = context.Departments
                .Where(d => d.Employees.Count > 5)
                .OrderBy(d => d.Employees.Count)
                .ThenBy(d => d.Name)
                .Select(d => new
                {
                    DepartmentName = d.Name,
                    ManagerFirstName = d.Manager.FirstName,
                    ManagerLastName = d.Manager.LastName,
                    Employees = d.Employees
                        .Select(e => new
                        {
                            e.FirstName,
                            e.LastName,
                            e.JobTitle
                        })
                        .OrderBy(e => e.FirstName)
                        .ThenBy(e => e.LastName)
                        .ToList()
                })
                .ToList();

            var sb = new StringBuilder();

            foreach (var dept in departments)
            {
                sb.AppendLine($"{dept.DepartmentName} - {dept.ManagerFirstName} {dept.ManagerLastName}");

                foreach (var empl in dept.Employees)
                {
                    sb.AppendLine($"{empl.FirstName} {empl.LastName} - {empl.JobTitle}");
                }
            }

            return sb.ToString().Trim();
        }

        public static string GetLatestProjects(SoftUniContext context)
        {
            var projects = context.Projects
                .OrderByDescending(p => p.StartDate)
                .Take(10)
                .Select(p => new
                {
                    p.Name,
                    p.Description,
                    p.StartDate
                })
                .OrderBy(p => p.Name)
                .ToList();

            var sb = new StringBuilder();

            foreach (var proj in projects)
            {
                sb.AppendLine(proj.Name);
                sb.AppendLine(proj.Description);
                sb.AppendLine(proj.StartDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture));
            }

            return sb.ToString().Trim();
        }

        public static string IncreaseSalaries(SoftUniContext context)
        {
            var employees = context.Employees
                .Where(e => e.Department.Name == "Engineering" || 
                e.Department.Name == "Tool Design" ||
                e.Department.Name == "Marketing" ||
                e.Department.Name == "Information Services")
                .OrderBy(e => e.FirstName)
                .ThenBy(e => e.LastName)
                .ToList();

            foreach (var empl in employees)
	        {
                empl.Salary *= 1.12M;
	        }

            context.SaveChanges();

            var sb = new StringBuilder();

            foreach (var empl in employees)
	        {
                sb.AppendLine($"{empl.FirstName} {empl.LastName} (${empl.Salary:F2})");
	        }

            return sb.ToString().Trim();
        }

        public static string GetEmployeesByFirstNameStartingWithSa(SoftUniContext context)
        {
            var employees = context.Employees
                .Where(e => e.FirstName.StartsWith("Sa") || e.FirstName.StartsWith("SA"))
                .OrderBy(e => e.FirstName)
                .ThenBy(e => e.LastName)
                .ToList();

            var sb = new StringBuilder();

            foreach (var empl in employees)
	        {
                sb.AppendLine($"{empl.FirstName} {empl.LastName} - {empl.JobTitle} - (${empl.Salary:f2})");
	        }

            return sb.ToString().Trim();
        }

        public static string DeleteProjectById(SoftUniContext context)
        {
            var employeesProjects = context.EmployeesProjects
                .Where(e => e.ProjectId == 2)
                .ToList();

            foreach (var emplProject in employeesProjects)
	        {
                context.EmployeesProjects.Remove(emplProject);
	        }

            var project = context.Projects.Find(2);
            context.Projects.Remove(project);

            context.SaveChanges();

            var projects = context.Projects
                .Take(10)
                .ToList();

            var sb = new StringBuilder();

            foreach (var proj in projects)
	        {
                sb.AppendLine($"{proj.Name}");
	        }

            return sb.ToString().Trim();
        }

        public static string RemoveTown(SoftUniContext context)
        {
            var employees = context.Employees
                .Where(e => e.Address.Town.Name == "Seattle")
                .ToList();

            foreach (var empl in employees)
	        {
                empl.AddressId = null;
	        }

            var addresses = context.Addresses
                .Where(e => e.Town.Name == "Seattle")
                .ToList();

            int count = addresses.Count;

            foreach (var adr in addresses)
	        {
                context.Addresses.Remove(adr);
	        }

            var town = context.Towns
                .FirstOrDefault(e => e.Name == "Seattle");

            context.Towns.Remove(town);

            context.SaveChanges();

            return $"{count} addresses in Seattle were deleted";
        }
    }
}
