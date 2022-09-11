using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace NttDataTest.Api.Gateway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string inicio = "{\n\"Routes\": [";

            string api_cliente = File.ReadAllText(@"RutasApiCliente.json").Trim().TrimStart('[').TrimEnd(']');
            string api_cuenta = File.ReadAllText(@"RutasApiCuenta.json").Trim().TrimStart('[').TrimEnd(']');
            string api_movimiento = File.ReadAllText(@"RutasApiMovimiento.json").Trim().TrimStart('[').TrimEnd(']');

            string fin = "],";

            string routes = inicio +
                            api_cliente +
                            "," +
                            api_cuenta +
                            "," +
                            api_movimiento +
                                    fin;

            string api_config = File.ReadAllText(@"config.json").Trim().TrimStart('{');

            string configuracion = routes + api_config;

            using (StreamWriter fs = new("RutasWebApis.json", false))
            {
                fs.Write(configuracion);
                fs.Close();
            }

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
