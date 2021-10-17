using Bogus;
using LMSG3.Core.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace LMSG3.Api
{
    public class SeedDbData
    {

        private static Faker fake;
      //  private static TextInfo ti;

        public static async Task InitAsync(IServiceProvider services)
        {
            using (var _context = services.GetRequiredService<ApiDbContext>())
            {
                // TODO: Check all entites or just one?
               // if (await db.Literatures.AnyAsync()) return;


                // Common
                fake = new Faker("en");
                //ti = new CultureInfo("en-US", false).TextInfo;

                // API






                var leteratureLevels = CreateLevels();
                await _context.AddRangeAsync(leteratureLevels);
                await _context.SaveChangesAsync();

                var letertureTypes = GetLiteratureTypes();
                await _context.AddRangeAsync(letertureTypes);

                await _context.SaveChangesAsync();

                var subject = subjects();
                await _context.AddRangeAsync(subject);
                await _context.SaveChangesAsync();

                // Save to DB
                var literatures = GetLeterature();
                await _context.AddRangeAsync(literatures);
                await _context.SaveChangesAsync();
                
                
            }
        }

        private static List<LiteratureLevel> CreateLevels()
        {
            List<LiteratureLevel> literatureLevels = new List<LiteratureLevel>();
            var levels = new List<string> { "Advanced", "Beginner", "Expert", "Intermediate" };
            for (int j = 0; j < levels.Count; j++)
            {
                var letertureLevet = new LiteratureLevel
                {
                    //Id = j,
                    Name = levels[j]
                };
                literatureLevels.Add(letertureLevet);
            }
            return literatureLevels;
        }

        private static List<LiteratureType> GetLiteratureTypes()
        {
            List<LiteratureType> literatureTypes = new List<LiteratureType>();
            var leteraTypes = new List<string> { "Drama", "Fable", "Fiction", "Poetry", "Science", "IT" };
            for(int t = 0; t < leteraTypes.Count; t++)
            {
                var literatureType = new LiteratureType
                {
                    Name = leteraTypes[t]
               
                };
                literatureTypes.Add(literatureType);
            }
            return literatureTypes;

        }

        private static List<Subject> subjects()
        {
            List<Subject> subjectsList = new List<Subject>();
           
            for(int s = 0; s <= 5; s++)
            {
                var subjectObj = new Subject 
                {
                    Name = fake.Lorem.Sentence()
                };
                subjectsList.Add(subjectObj);
            }
            return subjectsList;


        }

        private static IEnumerable<Literature> GetLeterature()
        {

            var literatureList = new List<Literature>();
            DateTime dt2 = new DateTime(2015, 12, 31);
            
            for (int i = 0; i <= 20; i++)
            {
               
                var leterature = new Literature
                {
                    Title = fake.Lorem.Sentence(),
                    Description = fake.Lorem.Paragraph(),
                    ReleaseDate = fake.Date.Between(dt2, DateTime.Now).Date,
                    LiteraLevelId = fake.Random.Number(1, 3),
                    LiteraTypeId = fake.Random.Number(1, 5),
                    SubId = fake.Random.Number(1, 6),
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
                    //Subject = new Subject
                    //{
                    //   //Id = fake.Random.Number(0, 3),
                    //   Name = fake.Lorem.Sentence(),
                    //},
                    //LiteratureLevel = new LiteratureLevel
                    //{
                    //    //Id =  fake.Random.Number(0, 3),
                    //    Name = fake.PickRandom(levels),//levels[index]
                    //},
                    //LiteratureType = new LiteratureType
                    //{
                    //   //Id = fake.Random.Number(0, 3),
                    //    Name = fake.PickRandom(leteraTypes)
                    //}
                };
                literatureList.Add(leterature);
            }

            return literatureList;
        }

     
    }
}
