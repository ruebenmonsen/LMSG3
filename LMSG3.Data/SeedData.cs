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

        private static string defaultPassword;
        private static string teacherRole;
        private static string studentRole;

        private static List<ActivityType> activityTypes;
        private static List<DocumentType> documentTypes;

        public static async Task InitAsync(IServiceProvider services)
        {
            using (var db = services.GetRequiredService<ApplicationDbContext>())
            {
                // TODO: Check all entites or just one?
                if (await db.Literatures.AnyAsync()) return;

                //Common
                fake = new Faker("en");
                ti = new CultureInfo("en-US", false).TextInfo;

                // API
                //var letertures = GetLeterature();
                //await db.AddRangeAsync(letertures);

                // MVC
                defaultPassword = "Abc123!";

                roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

                teacherRole = "Teacher";
                studentRole = "Student";

                await AddRoleAsync(teacherRole);
                await AddRoleAsync(studentRole);

                activityTypes = new List<ActivityType> {
                    new ActivityType { Name = "Lecture" },
                    new ActivityType { Name = "Assignment" },
                    new ActivityType { Name = "E-Learning" }
                };

                documentTypes = new List<DocumentType> {
                    new DocumentType { Name = "Assignment" },
                    new DocumentType { Name = "Information" },
                    new DocumentType { Name = "Excerice" }
                };

                // (static + minimal bogus) Uncomment to add nice default seed data 
                await AddDefaultMvcDataAsync(db);

                // (all bogus) Uncomment to add more bogus seed data
                // Modify amounts in sub method signatures
                // Too much data will slow the program
                await AddBogusMvcDataAsync(db);


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

        private static async Task AddDefaultMvcDataAsync(ApplicationDbContext db)
        {
            // Keep a day between the modules to reduce logic for activities.
            var currentModuleStart = DateTime.Now.AddDays(-7);
            var currentModuleEnd = currentModuleStart.AddDays(13);
            var previousModuleStart = currentModuleStart.AddDays(-7);
            var previousModuleEnd = currentModuleStart.AddDays(-1);
            var nextModuleStart = currentModuleEnd.AddDays(1);
            var nextModuleEnd = currentModuleEnd.AddDays(7);


            // Default course
            var defaultCourse = new Course
            {
                Name = "Fullstack in C# and JavaScript",
                Description = fake.Lorem.Paragraph(),
                StartDate = currentModuleStart
            };
            await db.AddAsync(defaultCourse);

            // Default modules
            var modules = new List<Module>();

            var previousModule = new Module
            {
                Name = "HTML5 and JavaScript",
                Description = fake.Lorem.Sentence(),
                StartDate = previousModuleStart,
                EndDate = previousModuleEnd,
                Course = defaultCourse
            };
            modules.Add(previousModule);

            var currentModule = new Module
            {
                Name = "C# with MVC and API",
                Description = fake.Lorem.Sentence(),
                StartDate = currentModuleStart,
                EndDate = currentModuleEnd,
                Course = defaultCourse
            };
            modules.Add(currentModule);

            var nextModule = new Module
            {
                Name = "Fullstack project",
                Description = fake.Lorem.Sentence(),
                StartDate = nextModuleStart,
                EndDate = nextModuleEnd,
                Course = defaultCourse
            };
            modules.Add(nextModule);
            await db.AddRangeAsync(modules);

            // Bogus activities for default modules
            var activities = new List<Activity>();
            foreach (var module in modules)
            {
                activities.AddRange(GetActivities(module, activityTypes, 3));
            }
            await db.AddRangeAsync(activities);

            // Default teachers
            List<Teacher> teachers = new List<Teacher>();
            teachers.Add(new Teacher
            {
                FName = "David",
                LName = "Novell",
                Email = "d@lexi.com",
                UserName = "d@lexi.com"
            });
            teachers.Add(new Teacher
            {
                FName = "Dimitris",
                LName = "Backman",
                Email = "t@lexi.com",
                UserName = "t@lexi.com"
            });
            foreach (var teacher in teachers)
            {
                var iResult = await userManager.CreateAsync(teacher, defaultPassword);
                if (!iResult.Succeeded)
                {
                    throw new Exception(String.Join("\n", iResult.Errors));
                }
                await userManager.AddToRoleAsync(teacher, teacherRole);
            }

            // Default students
            List<Student> students = new List<Student>();
            students.Add(new Student
            {
                FName = "Student",
                LName = "Teacherson",
                Email = "s@lexi.com",
                UserName = "s@lexi.com",
                Course = defaultCourse
            });
            students.Add(new Student
            {
                FName = "Kalle",
                LName = "Anka",
                Email = "s2@lexi.com",
                UserName = "s2@lexi.com",
                Course = defaultCourse
            });
            students.Add(new Student
            {
                FName = "Pelle",
                LName = "Svanslös",
                Email = "s3@lexi.com",
                UserName = "s3@lexi.com",
                Course = defaultCourse
            });
            foreach (var student in students)
            {
                var iResult = await userManager.CreateAsync(student, defaultPassword);
                if (!iResult.Succeeded)
                {
                    throw new Exception(String.Join("\n", iResult.Errors));
                }
                await userManager.AddToRoleAsync(student, studentRole);
            }

            // Default bogus documents
            var teacherDocuments = GetDocuments(teachers, teacherRole, documentTypes, new List<Course> { defaultCourse }, modules, activities, 50);
            await db.AddRangeAsync(teacherDocuments);

            var studentDocuments = GetDocuments(students, studentRole, documentTypes, new List<Course> { defaultCourse }, modules, activities, 75);
            await db.AddRangeAsync(studentDocuments);

        }

        private static async Task AddBogusMvcDataAsync(ApplicationDbContext db)
        {
            // MVC : Courses
            var courses = GetCourses(2);
            await db.AddRangeAsync(courses);

            // MVC : Modules
            var modules = new List<Module>();
            foreach (var course in courses)
            {
                modules.AddRange(GetModules(course, fake.Random.Int(2, 5)));
            }
            await db.AddRangeAsync(modules);

            // MVC : Activities

            var activities = new List<Activity>();
            foreach (var module in modules)
            {
                activities.AddRange(GetActivities(module, activityTypes, fake.Random.Int(1, 3)));
            }
            await db.AddRangeAsync(activities);


            // MVC : Users

            var teachers = (await GetTeachersAsync(defaultPassword, 2)).ToList();
            foreach (var teacher in teachers)
            {
                await userManager.AddToRoleAsync(teacher, teacherRole);
            }


            var students = (await GetStudentsAsync(courses, defaultPassword, 8)).ToList();
            foreach (var student in students)
            {
                await userManager.AddToRoleAsync(student, studentRole);
            }


            // MVC : Documents

            var teacherDocuments = GetDocuments(teachers, teacherRole, documentTypes, courses, modules, activities, 12);
            await db.AddRangeAsync(teacherDocuments);

            var studentDocuments = GetDocuments(students, studentRole, documentTypes, courses, modules, activities, 16);
            await db.AddRangeAsync(studentDocuments);
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

        private static IEnumerable<Course> GetCourses(int amount)
        {
            var courses = new List<Course>();

            for (int i = 0; i < amount; i++)
            {
                var e = new Course
                {
                    Name = ti.ToTitleCase(fake.Hacker.Noun() + " " + fake.Hacker.Verb()),
                    Description = fake.Lorem.Paragraph(),
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
                endDate = startDate.AddDays(fake.Random.Int(1, 30));
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

        private static IEnumerable<Activity> GetActivities(Module module, IEnumerable<ActivityType> types, int amountPerDay)
        {
            var activities = new List<Activity>();
            DateTime startDay = module.StartDate;
            DateTime startDate;
            DateTime endDate;
            int startHour = 0; // TODO: maybe some logic here
            int days = (int) (module.EndDate - module.StartDate).TotalDays; // could fail

            for (int day = 0; day < days; day++)
            {
                startDate = startDay.AddDays(day).AddHours(startHour);

                for (int i= 0; i < amountPerDay; i++)
                {
                    endDate = startDate.AddHours(fake.Random.Int(1, 2)); // TODO: fix bug
                    var e = new Activity
                    {
                        Name = ti.ToTitleCase(fake.Hacker.Noun() + " " + fake.Hacker.IngVerb()),
                        Description = fake.Lorem.Sentence(),
                        StartDate = startDate,
                        EndDate = endDate,
                        Module = module,
                        ActivityType = fake.PickRandom(types)                  
                    };
                    activities.Add(e);
                    startDate = endDate;
                }
            }
            return activities;
        }

        private static IEnumerable<Document> GetDocuments(IEnumerable<ApplicationUser> users, string role,
            IEnumerable<DocumentType> documentTypes, IEnumerable<Course> courses, 
            IEnumerable<Module> modules, IEnumerable<Activity> activities, int amount)
        {
            var documents = new List<Document>();

            for (int i = 0; i < amount; i++)
            {
                var document = new Document
                {
                    Name = fake.Hacker.Noun(),
                    Description = fake.Lorem.Sentence(),
                    UploadDate = DateTime.Now.AddDays(fake.Random.Int(-14, 1)), // TODO: logic
                    DocumentType = fake.PickRandom(documentTypes),
                    ApplicationUser = fake.PickRandom(users)             
                };
                switch (fake.PickRandom( new string[] {"Course", "Module", "Activity", "Personal"}))
                {
                    case "Course":
                        document.Course = fake.PickRandom(courses);
                        document.UploadDate = document.Course.StartDate.AddDays(fake.Random.Int(0, 30));
                        break;
                    case "Module":
                        document.Module = fake.PickRandom(modules);
                        document.UploadDate = document.Module.StartDate.AddDays(fake.Random.Int(0, 14));
                        break;
                    case "Activity":
                        document.Activity = fake.PickRandom(activities);
                        if (role == teacherRole)
                        {
                            document.UploadDate = document.Activity.StartDate;
                        }
                        else
                        {
                            document.UploadDate = document.Activity.StartDate.AddHours(fake.Random.Int(0, 4));
                        }
                        break;
                    case "Personal":
                        break;
                }

                documents.Add(document);
            }

            return documents;
        }
    }
}
