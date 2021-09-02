using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace RedisCacheVersionB.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {

        private readonly IConnectionMultiplexer _redis;
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(IConnectionMultiplexer client,ILogger<WeatherForecastController> logger)
        {
            _redis = client;
            
            _logger = logger;

        }
        [HttpGet]
        public string Get()
        {
           
            var key = "TestRedis";
            var db = _redis.GetDatabase(4);
            
            RedisValue temp = db.StringGet(key, CommandFlags.PreferReplica);            
            if (temp == RedisValue.Null)
            {
                temp = GetFromWs();
                db.StringSet("TestRedis", temp, TimeSpan.FromMinutes(10));
            }
            else
            {
                temp=Encoding.UTF8.GetString(temp);
            }
           
            return temp;
        }



        private string GetFromWs()
        {
            string json = null;
            using (StreamReader r = new StreamReader("respuesta.json"))
            {
                json = r.ReadToEnd();

            }
            return json;
        }

       
    }
}
