using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleGameWebAPI.Data.Dto
{
    public class GameSessionDoActionDto
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public int PointX { get; set; }
        public int PointY { get; set; }
    }
}
