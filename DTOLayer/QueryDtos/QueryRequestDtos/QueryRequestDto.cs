using DTOLayer.QueryDtos.QueryParameterDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOLayer.QueryDtos.QueryRequestDtos
{
    public class QueryRequestDto
    {
        public string Query { get; set; }
        public List<QueryParameterDto>? Parameters { get; set; }
    }
}
