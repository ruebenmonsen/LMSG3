using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMSG3.Api.Services
{
    public static class ModelsJoinHelper
    {
        

        public static string GetLevelName(int levelId, ApiDbContext _context)
        {
            var levelName = _context.Literatures.Join(_context.literatureLevels,
                           x => x.LiteraLevelId,
                           y => y.Id,
                          (x, y) => new { x.LiteraLevelId, y.Name })
                            .First(e => e.LiteraLevelId == levelId).Name.ToString();

            return levelName;
        }

        public static string GetTypeName(int levTypelId, ApiDbContext _context)
        {
            var typelName = _context.Literatures.Join(_context.literatureTypes,
                           x => x.LiteraTypeId,
                           y => y.Id,
                          (x, y) => new { x.LiteraTypeId, y.Name })
                            .First(e => e.LiteraTypeId == levTypelId).Name.ToString();

            return typelName;
        }

        public static string GetSubjectName(int subjecId, ApiDbContext _context)
        {
            var subjectName = _context.Literatures.Join(_context.LiteratureSubjects,
                           x => x.SubId,
                           y => y.Id,
                           (x, y) => new { x.SubId, y.Name }).First(e => e.SubId == subjecId).Name.ToString();
            return subjectName;
        }

        public static int GetAythorAge(DateTime dateTime)
        {

            // Save today's date.
            var today = DateTime.Today;

            // Calculate the age.
            var age = today.Year - dateTime.Year;
            
            // Go back to the year in which the person was born in case of a leap year
            if (dateTime.Date > today.AddYears(-age)) age--;
                        age = DateTime.Now.Year - dateTime.Year;
            return age;
        }



    }
}
