using Bogus;
using LMSG3.Core.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMSG3.Data
{
    public class SeedData
    {

        private static Faker fake;

        public static async Task InitAsync(ApplicationDbContext context, IServiceProvider services, string adminPW)
        {
            using (var db = services.GetRequiredService<ApplicationDbContext>())
            {
                if (await db.Literatures.AnyAsync()) return;

                fake = new Faker("en");

                var letertures = GetLeterature();
                await db.AddRangeAsync(letertures);

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
    }
}
