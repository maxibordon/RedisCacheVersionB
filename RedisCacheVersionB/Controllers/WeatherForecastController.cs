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

            var db = _redis.GetDatabase(4);
            var key = "TestRedis";
            if (db.IsConnected(key))
            {
                
                //  var db = _redis.GetDatabase(4);


                 RedisValue temp = db.StringGet(key, CommandFlags.PreferReplica);
                  db.KeyExpire(key, TimeSpan.FromMinutes(5), flags: CommandFlags.FireAndForget); //Ventana deslizante
                  if (temp == RedisValue.Null)
                 {
                  temp = GetFromWs();
                  db.StringSet("TestRedis", temp, TimeSpan.FromMinutes(5));
                 }
                 else
                 {
                    temp=Encoding.UTF8.GetString(temp);
                 }

                return temp;
            }
            return "Desconectado";
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
