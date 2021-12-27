using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleGameWebAPI.Data.Dto
{
    public class RestartGameDto
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
    }
}
