using Dapper;
using Microsoft.AspNetCore.Mvc;

namespace grey.house.api.Controllers;

[Route("services")]
public class ProductItemController : ControllerBase
{
    private readonly ILogger<ProductItemController> _logger;

    public ProductItemController(ILogger<ProductItemController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "database")]
    public async Task<List<dynamic>> Get([FromQuery] string sqlId)
    {
        // var parameters = new Dictionary<string, string>();
        var result = await SelectAsync<dynamic>(sqlId, null).ConfigureAwait(false);

                            
        // var result = new List<dynamic>();
        // result.Add(new {
        //     Id = "id-880111222333",
        //     Sequence = 1,
        //     Code = "880111222333",
        //     Name = "신일티아민염산염정 10mg",
        //     Amount = 22
        // });
        // result.Add(new {
        //     Id = "id-880111222444",
        //     Sequence = 2,
        //     Code = "880111222444",
        //     Name = "신일티아민염산염정 20mg",
        //     Amount = 45
        // });

        return result;
    }




    private MySql.Data.MySqlClient.MySqlConnection Connect()
    {

        var _connectionString = "host=localhost;port=3306;user id=greyhouse;password=greyhouse;database=greyhouse;";
        return new MySql.Data.MySqlClient.MySqlConnection(_connectionString);
    }

    private async Task<List<T>> SelectAsync<T>(string sqlId, Dictionary<string, string> parameters)
    {
        using (var connection = Connect()) 
        {
            await connection.OpenAsync().ConfigureAwait(false);
            var sqlContent = $"select SQL_CONTENT from SQL_STORAGE where id = '{sqlId}'";
            var sql = await Dapper.SqlMapper.QueryFirstAsync<string>(connection, sqlContent).ConfigureAwait(false);

            if (!string.IsNullOrWhiteSpace(sql)) 
            {
                // var dynamicParameters = new DynamicParameters();
                // foreach(var item in parameters)
                // {
                //     dynamicParameters.Add("@" + item.Key.Replace("@", ""), item.Value);
                // }

                var result = await Dapper.SqlMapper.QueryAsync<T>(connection, sql).ConfigureAwait(false);
                return result.ToList();
            }
            else 
            {
                return new List<T>();
            }
        }   
    }
}
