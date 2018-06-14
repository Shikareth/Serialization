using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Serialization
{
    class Program
    {
        private static Settings settings;

        static void Main(string[] args)
        {
            settings = Settings.DeserializeSettings2();

            Console.WriteLine("Finished ...");
            Console.ReadKey();
        }

        [Obsolete]
        public static void LoadSettings(out Settings settings)
        {
            try
            {
                if (File.Exists("Settings.xml"))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(Settings));
                    using (var fs = File.Open("Settings.xml", FileMode.Open))
                        settings = (Settings)serializer.Deserialize(fs);

                    Console.WriteLine("Loaded settings");
                }
                else
                {
                    settings = new Settings(true);

                    //create empty file with default settings
                    SaveSettings(settings);
                    Console.WriteLine("Loaded default settings");
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine($"{ex.Message}\n{ex.StackTrace}");
                settings = new Settings();
            }
        }

        public static bool SaveSettings(Settings settings)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Settings));

                using (var fs = File.Open("Settings.xml", FileMode.Create))
                    serializer.Serialize(fs, settings);

                Console.WriteLine("Saved settings");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}\n{ex.StackTrace}");
            }

            return false;
        }


    }
}
