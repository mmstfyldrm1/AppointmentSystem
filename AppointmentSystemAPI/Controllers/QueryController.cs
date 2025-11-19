using DataAccsessLayer.Concrete;
using DTOLayer.QueryDtos.QueryRequestDtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppointmentSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QueryController : ControllerBase
    {
        private readonly AppDbContext _context;

        public QueryController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("execute")]
        public async Task<IActionResult> ExecuteQuery(QueryRequestDto request)
        {
            if (string.IsNullOrWhiteSpace(request.Query))
                return BadRequest("Query cannot be empty.");

            using var cmd = _context.Database.GetDbConnection().CreateCommand();
            cmd.CommandText = request.Query;
            cmd.CommandType = System.Data.CommandType.Text;

            // Parametre varsa ekle, yoksa hiçbir şey ekleme
            if (request.Parameters != null)
            {
                foreach (var p in request.Parameters)
                {
                    var param = cmd.CreateParameter();
                    param.ParameterName = "@" + p.Name;
                    param.Value = p.Value ?? " ";
                    cmd.Parameters.Add(param);
                }
            }

            await _context.Database.OpenConnectionAsync();

            var reader = await cmd.ExecuteReaderAsync();

            var result = new List<Dictionary<string, object>>();

            while (await reader.ReadAsync())
            {
                var row = new Dictionary<string, object>();

                for (int i = 0; i < reader.FieldCount; i++)
                {
                    row[reader.GetName(i)] = reader.GetValue(i);
                }

                result.Add(row);
            }

            return Ok(result);
        }
    }
}
