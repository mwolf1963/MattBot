using System;
using System.IO;
using System.Threading.Tasks;
using System.Security.AccessControl;
using Newtonsoft.Json;


namespace MattBot
{
    public class ConfigHandler
    {
        private Config conf;
        private string configPath, line;

        struct Config
        {
            public string token;
        }

        public ConfigHandler()
        {
            conf = new Config()
            {
                token = ""
            };
         }

        public async Task PopulateConfig()
        {
            configPath = Path.Combine(Directory.GetCurrentDirectory(), "config.json").Replace(@"\", @"\\");
            Console.WriteLine(configPath);

            if (!File.Exists(configPath))
            {
                using (StreamWriter sw = File.AppendText(configPath))
                {
                    sw.WriteLine(JsonConvert.SerializeObject(conf));
                }
                Console.WriteLine("Warning!! New Config initialized! Need to fill in values before running commands!");
                throw new Exception("No Config Available! Go to executable path and fill out  newly created file!");
                
            }

            using (StreamReader srReader = new StreamReader(configPath))
            {
                
                    conf = JsonConvert.DeserializeObject<Config>(srReader.ReadLine());
                
            }

            await Task.CompletedTask;
        }

        public string GetToken()
        {
            return conf.token;
        }
    }
}
