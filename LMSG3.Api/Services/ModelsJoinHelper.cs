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

    }
}
