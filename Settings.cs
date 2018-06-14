using Optinav.Tools;
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
    [Serializable]
    [XmlRoot(ElementName = "Settings", Namespace = "", DataType = "")]
    public class Settings
    {

        public int SelectedProbeId { get; set; }
        public long ExposureTime { get; set; }
        public double ErrorThreshold { get; set; }
        public CameraOrderModel CameraOrder { get; set; }

        [XmlArrayItem("Probe")]
        public List<ProbeModel> ProbeList { get; set; }

        [XmlElement("Plane")]
        public PlaneModel Plane { get; set; }

        public Settings()
        {

        }

        public Settings(bool loadDefaults)
        {
            if (loadDefaults)
            {
                SelectedProbeId = 0;

                CameraOrder = new CameraOrderModel() { Camera0 = "U502901", Camera1 = "U502902", Camera2 = "U502903" };

                ProbeList = new List<ProbeModel>();
                ProbeList.Add(new ProbeModel() { Name = "Probe1", AngleX = 0, AngleY = 0, AngleZ = 0, Radius = 1 });

                Plane = new PlaneModel();
            }

        }

        public static Settings DeserializeSettings()
        {
            Settings settings = new Settings(true);

            var xmlSchemaSerializer = new XmlSerializer(typeof(XmlSchema));
            var XmlSerializer = new XmlSerializer(typeof(Settings));

            var schemas = new XmlSchemaSet();
            XmlSchema schema;
            using (var xsdStream = File.OpenRead("Settings.xsd"))
            {
                schema = (XmlSchema)xmlSchemaSerializer.Deserialize(xsdStream);
            }
            schemas.Add(schema);

            var setup = new XmlReaderSettings
            {
                Schemas = schemas,
                ValidationType = ValidationType.Schema,
                ValidationFlags = XmlSchemaValidationFlags.ProcessIdentityConstraints | XmlSchemaValidationFlags.ReportValidationWarnings | XmlSchemaValidationFlags.ProcessInlineSchema | XmlSchemaValidationFlags.ProcessSchemaLocation
            };

            setup.ValidationEventHandler += (sender, args) =>
            {
                Console.WriteLine($"{args.Severity}:\t{args.Message}");
            };

            using (Stream stream = File.OpenRead("Settings.xml"))
            {
                using (XmlReader reader = XmlReader.Create(stream, setup))
                {
                    settings = (Settings)XmlSerializer.Deserialize(reader);
                }
            }

            return settings;
        }

        public static Settings DeserializeSettings2()
        {
            Settings settings = null;
            ValidationHandler validationHandler = new ValidationHandler();

            XmlReaderSettings xmlSettings = new XmlReaderSettings();
            xmlSettings.CloseInput = true;
            xmlSettings.CheckCharacters = true;
            xmlSettings.ConformanceLevel = ConformanceLevel.Fragment;
            xmlSettings.IgnoreWhitespace = true;
            xmlSettings.ValidationType = ValidationType.Schema;
            xmlSettings.ValidationEventHandler += new ValidationEventHandler(validationHandler.HandleValidationError);
            xmlSettings.Schemas.Add(null, "Settings.xsd");
            xmlSettings.ValidationFlags =
                XmlSchemaValidationFlags.ReportValidationWarnings |
                XmlSchemaValidationFlags.ProcessIdentityConstraints |
                XmlSchemaValidationFlags.ProcessInlineSchema |
                XmlSchemaValidationFlags.ProcessSchemaLocation;

            try
            {
                XmlSerializer s = new XmlSerializer(typeof(Settings));
                using (Stream stream = File.OpenRead("Settings.xml"))
                {
                    using (XmlReader reader = XmlReader.Create(stream, xmlSettings))
                    {
                        settings = (Settings)s.Deserialize(reader);
                    }
                }
            }
            catch (InvalidOperationException) { }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
            }

            foreach (var error in validationHandler.MyValidationErrors)
                Console.WriteLine($"[{error.Severity}][{error.Line}:{error.Position}]{error.Message}");

            return settings;
        }

        [Serializable]
        public class CameraOrderModel
        {
            public CameraOrderModel()
            {
                Camera0 = "";
                Camera1 = "";
                Camera2 = "";
            }

            public string Camera0 { get; set; }
            public string Camera1 { get; set; }
            public string Camera2 { get; set; }
        }

        [Serializable]
        public class ProbeModel
        {
            public ProbeModel()
            {
                Name = "Probe";
                AngleX = 0;
                AngleY = 0;
                AngleZ = 0;
                Radius = 1;
                TCP = new Vector3D(0, 0, 0);
            }

            public string Name { get; set; }
            public double AngleX { get; set; }
            public double AngleY { get; set; }
            public double AngleZ { get; set; }
            public double Radius { get; set; }
            public Vector3D TCP { get; set; }
        }

        [Serializable]
        public class PlaneModel
        {
            public PlaneModel()
            {
                P1 = new Vector3D(0, 0, 0);
                P2 = new Vector3D(1, 0, 0);
                P3 = new Vector3D(0, 1, 0);
            }

            public Vector3D P1 { get; set; }
            public Vector3D P2 { get; set; }
            public Vector3D P3 { get; set; }
        }
    }
}


