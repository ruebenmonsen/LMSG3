using Bogus;
using LMSG3.Core.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace LMSG3.Data
{
    public class SeedData
    {
        private static RoleManager<IdentityRole> roleManager;
        private static UserManager<ApplicationUser> userManager;
        private static Faker fake;
        private static TextInfo ti;

        public static async Task InitAsync(IServiceProvider services)
        {
            using (var db = services.GetRequiredService<ApplicationDbContext>())
            {
                // TODO: Check all entites or just one?
                if (await db.Literatures.AnyAsync()) return;


                // Common
                fake = new Faker("en");
                ti = new CultureInfo("en-US", false).TextInfo;


                // API
                var letertures = GetLeterature();
                //await db.AddRangeAsync(letertures);


                // MVC

                var defaultPassword = "Abc123!";
                var teacherRole = "Teacher";
                var studentRole = "Student";

                roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

                await AddRoleAsync(teacherRole);
                await AddRoleAsync(studentRole);


                // MVC : Courses
                var courses = GetCourses(3);
                await db.AddRangeAsync(courses);

                // MVC : Modules
                var modules = new List<Module>();
                foreach (var course in courses)
                {
                    modules.AddRange(GetModules(course, 5));
                }
                await db.AddRangeAsync(modules);

                // MVC : Activities
                List<ActivityType> activityTypes = new List<ActivityType> {
                    new ActivityType { Name = "Lecture" },
                    new ActivityType { Name = "Assignment" },
                    new ActivityType { Name = "E-Learning" }
                };

                var activities = new List<Activity>();
                foreach (var module in modules)
                {
                    activities.AddRange(GetActivities(module, activityTypes));
                }


                // MVC : Users
                var teachers = await GetTeachersAsync(defaultPassword, 2);
                foreach (var teacher in teachers)
                {
                    await userManager.AddToRoleAsync(teacher, teacherRole);
                }

                var students = await GetStudentsAsync(courses, defaultPassword, 12);
                foreach (var student in students)
                {
                    await userManager.AddToRoleAsync(student, studentRole);
                }


                // MVC : Documents
                List<DocumentType> documentTypes = new List<DocumentType> {
                    new DocumentType { Name = "Assignment" },
                    new DocumentType { Name = "Information" },
                    new DocumentType { Name = "Excerice" }
                };

                var teacherDocuments = GetDocuments(teachers, documentTypes, courses, modules, activities, 10);
                await db.AddRangeAsync(teacherDocuments);

                var studentDocuments = GetDocuments(students, documentTypes, courses, modules, activities, 5);
                await db.AddRangeAsync(studentDocuments);


                // Save to DB
                await db.SaveChangesAsync();
            }
        }

        private static List<Literature> GetLeterature()
        {

            var literatures = new List<Literature>();
            DateTime dt2 = new DateTime(2015, 12, 31);
            var levels = new List<string> { "Advanced", "Beginner", "Expert", "Intermediate" };
            var leteraTypes = new List<string> { "Drama", "Fable", "Fiction", "Poetry", "Science", "IT"};
            var random = new Random();
            for (int i = 0; i < 200; i++)
            {
                int index = random.Next(levels.Count);
                var title = fake.Lorem.Sentence();
                var description = fake.Lorem.Paragraph();
                var releaseDate = fake.Date.Between(dt2, DateTime.Now);
                var leterature = new Literature
                {
                    Title = title,
                    Description = description,
                    ReleaseDate = releaseDate.Date,
                    Authors = new LiteratureAuthor[]
                    {
                        new LiteratureAuthor
                        {
                            FirstName = fake.Name.FirstName(),
                            LastName = fake.Name.LastName(),
                            DateOfBirth = fake.Date.Between(new DateTime(1940, 12, 31), new DateTime(1995, 12, 31)).Date
                        },
                        new LiteratureAuthor
                        {
                            FirstName = fake.Name.FirstName(),
                            LastName = fake.Name.LastName(),
                            DateOfBirth = fake.Date.Between(new DateTime(1940, 12, 31), new DateTime(1995, 12, 31)).Date
                        },
                         new LiteratureAuthor
                        {
                            FirstName = fake.Name.FirstName(),
                            LastName = fake.Name.LastName(),
                            DateOfBirth = fake.Date.Between(new DateTime(1940, 12, 31).Date, new DateTime(1995, 12, 31).Date).Date
                        }

                    },
                    Subject = new Subject
                    {
                        Name = fake.Lorem.Sentence(),
                    },
                    LiteratureLevel = new LiteratureLevel
                    {
                        Name = levels[index]
                    },
                    LiteratureType = new LiteratureType
                    {
                        Name = leteraTypes[index]
                    }
                };
                literatures.Add(leterature);
            }

            return literatures;
        }

        private static async Task AddRoleAsync(string roleName)
        {
            if (await roleManager.RoleExistsAsync(roleName) is false)
            {
                var role = new IdentityRole { Name = roleName };
                var result = await roleManager.CreateAsync(role);


                if (!result.Succeeded)
                {
                    throw new Exception(String.Join("\n", result.Errors));
                }
            }
        }

        private static async Task<IEnumerable<Teacher>> GetTeachersAsync(string password, int amount)
        {
            var faker = new Faker<Teacher>()
                .RuleFor(u => u.FName, f => f.Person.FirstName)
                .RuleFor(u => u.LName, f => f.Person.LastName)
                .RuleFor(u => u.Email, f => f.Person.Email)
                .RuleFor(u => u.UserName, f => f.Person.Email);

            var teachers = faker.Generate(amount).ToList();

            foreach (var teacher in teachers)
            {
                var iResult = await userManager.CreateAsync(teacher, password);
                if (!iResult.Succeeded)
                {
                    throw new Exception(String.Join("\n", iResult.Errors));
                }
            }

            return teachers;
        }

        private static async Task<IEnumerable<Student>> GetStudentsAsync(IEnumerable<Course> courses, string password, int amount)
        {
            var faker = new Faker<Student>()
                .RuleFor(u => u.FName, f => f.Person.FirstName)
                .RuleFor(u => u.LName, f => f.Person.LastName)
                .RuleFor(u => u.Email, f => f.Person.Email)
                .RuleFor(u => u.UserName, f => f.Person.Email);

            var students = faker.Generate(amount).ToList();

            foreach (var student in students)
            {
                //student.Course = courses.ElementAt(fake.Random.Int(0, courses.Count() - 1));  // TODO: remove
                student.Course = fake.PickRandom(courses);
                var iResult = await userManager.CreateAsync(student, password);
                Console.WriteLine("ping");
                if (!iResult.Succeeded)
                {
                    Console.WriteLine("ping");
                    throw new Exception(String.Join("\n", iResult.Errors));
                }
            }
            Console.WriteLine("ping");

            return students;
        }

        // TODO: remove
        //private static async Task<IEnumerable<Student>> GetStudentsAsync(string password, int amount)
        //{
        //    var students = new List<Student>();

        //    for (int i = 0; i < amount; i++)
        //    {
        //        var e = new Student
        //        {
        //            FName = fake.Person.FirstName,
        //            LName = fake.Person.LastName,
        //            Email = fake.Person.Email,
        //            UserName = fake.Person.Email
        //        };

        //        var iResult = await userManager.CreateAsync(e, password);
        //        if (!iResult.Succeeded)
        //        {
        //            throw new Exception(String.Join("\n", iResult.Errors));
        //        }

        //        students.Add(e);
        //    }
        //    Console.WriteLine("ping");

        //    return students;
        //}

        private static IEnumerable<Course> GetCourses(int amount)
        {
            var courses = new List<Course>();

            for (int i = 0; i < amount; i++)
            {
                var e = new Course
                {
                    Name = ti.ToTitleCase(fake.Hacker.Noun() + " " + fake.Hacker.Verb()),
                    Description = fake.Lorem.Sentence(),
                    StartDate = DateTime.Now.AddDays(fake.Random.Int(1, 364))
                };

                courses.Add(e);
            }

            return courses;
        }

        private static IEnumerable<Module> GetModules(Course course, int amount)
        {
            var modules = new List<Module>();

            DateTime startDate = course.StartDate;
            DateTime endDate;

            for (int i = 0; i < amount; i++)
            {
                endDate = startDate.AddDays(fake.Random.Int(1, 90));
                var e = new Module
                {
                    Name = ti.ToTitleCase(fake.Hacker.Noun() + " " + fake.Hacker.Verb()),
                    Description = fake.Lorem.Sentence(),
                    StartDate = startDate,
                    EndDate = endDate,
                    Course = course
                };
                modules.Add(e);
                startDate = endDate;
            }

            return modules;
        }

        private static IEnumerable<Activity> GetActivities(Module module, IEnumerable<ActivityType> types)
        {
            var activities = new List<Activity>();
            DateTime startDay = module.StartDate;
            DateTime startDate;
            DateTime endDate;
            int startHour = 8; // assuming midnight
            int amount = 3; // per day
            int days = (int) (module.EndDate - module.StartDate).TotalDays; // could fail

            for (int day = 0; day < days; day++)
            {
                startDate = startDay.AddDays(day).AddHours(startHour);

                for (int i= 0; i < amount; i++)
                {
                    endDate = startDate.AddHours(fake.Random.Int(1, 2));
                    var e = new Activity
                    {
                        Name = ti.ToTitleCase(fake.Hacker.Noun() + " " + fake.Hacker.IngVerb()),
                        Description = fake.Lorem.Sentence(),
                        StartDate = startDate,
                        EndDate = endDate,
                        Module = module,
                        ActivityType = fake.PickRandom(types)
                        //ActivityType = types.ElementAtOrDefault(fake.Random.Int(0, types.Count() - 1)) // TODO: remove                      
                    };
                    activities.Add(e);
                    startDate = endDate;
                }
            }
            return activities;
        }
        private static IEnumerable<Document> GetDocuments(IEnumerable<ApplicationUser> users, 
            IEnumerable<DocumentType> documentTypes, IEnumerable<Course> courses, 
            IEnumerable<Module> modules, IEnumerable<Activity> activities, int amount)
        {
            var documents = new List<Document>();

            for (int i = 0; i < amount; i++)
            {
                var e = new Document
                {
                    Name = fake.Hacker.Noun(),
                    Description = fake.Lorem.Sentence(),
                    UploadDate = DateTime.Now.AddDays(fake.Random.Int(1, 364)), // TODO: logic
                    DocumentType = fake.PickRandom(documentTypes),
                    ApplicationUser = fake.PickRandom(users)             
                };
                switch (fake.PickRandom( new string[] {"Course", "Module", "Activity", "Personal"}))
                {
                    case "Course":
                        e.Course = fake.PickRandom(courses);
                        break;
                    case "Module":
                        e.Module = fake.PickRandom(modules);
                        break;
                    case "Activity":
                        e.Activity = fake.PickRandom(activities);
                        break;
                    case "Personal":
                        break;
                }

                documents.Add(e);
            }

            return documents;
        }
    }
}
