using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Manect.Data.Entities
{
    public class DataToChange
    {
        public int UserId { get; set; }
        public int ProjectId { get; set; }
        public int StageId { get; set; }
        public int FileId { get; set; }
        public Status Status { get; set; }
        public IList<IFormFile> Files { get; set; }
    }
}