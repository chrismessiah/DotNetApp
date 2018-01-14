using System;
using System.Collections.Generic;

namespace DotNetApp
{
    public static class Globals
    {
        public static Dictionary<string, string> env = new Dictionary<string, string>();
        private static bool haveRead = false;

        public static void ClearEnviromentVariables()
        {
          env = new Dictionary<string, string>();
        }

        public static void ReadEnviromentVariables()
        {
            if (!haveRead) {
                ReadEnviromentVariablesDevelopment("./.env");
                Console.WriteLine("Starting");
                if (!Globals.env.ContainsKey("DOTNET_ENV") || Globals.env["DOTNET_ENV"] != "Development") {
                    Console.WriteLine("Entered");
                    ClearEnviromentVariables();
                    ReadEnviromentVariablesProduction();
                    Console.WriteLine("Environment is PRODUCTION");
                }
                haveRead = true;
            }
        }

        private static void ReadEnviromentVariablesProduction()
        {
          var enumerator = System.Environment.GetEnvironmentVariables().GetEnumerator();
          while (enumerator.MoveNext())
          {
              // adds ALL env vars not only those passed by docker
              Console.WriteLine(enumerator.Value.ToString());
              Globals.env.Add(enumerator.Key.ToString(), enumerator.Value.ToString());
          }
          if (Globals.env.ContainsKey("DATABASE_URL")) {
            var tmp = Globals.env["DATABASE_URL"].Replace("postgres://", "");

            var user = tmp.Split(":")[0];
            tmp = tmp.Replace($"{user}:", "");

            var password = tmp.Split("@")[0];
            tmp = tmp.Replace($"{password}@", "");

            var host = tmp.Split(":")[0];
            tmp = tmp.Replace($"{host}:", "");

            var port = tmp.Split("/")[0];
            var db = tmp.Split("/")[1];

            Globals.env["CONNECTION_STRING"] = $"User ID={user};Password={password};Server={host};Port={port};Database={db}";
          }
        }

        private static void ReadEnviromentVariablesDevelopment(string path)
        {
            if (System.IO.File.Exists(path))
            {
                string[] lines = System.IO.File.ReadAllLines(path);
                foreach (string line in lines)
                {
                    var index = line.IndexOf("=");
                    Globals.env.Add(line.Substring(0, index), line.Substring(index+1));
                }
            }

            if (!Globals.env.ContainsKey("DOTNET_ENV"))
            {
                Globals.env.Add("DOTNET_ENV", "Development");
            }
        }
    }
}
