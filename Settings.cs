using Optinav.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                ProbeList.Add(new ProbeModel() { Name = "Probe1", AngleX = 0, AngleY = 1, AngleZ = 0, Radius = 1 });

                Plane = new PlaneModel();
            }

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
                Name = "unknown";
                AngleX = 0;
                AngleY = 0;
                AngleZ = 0;
                Radius = 1;
                TCP = new TCPModel();
            }

            public string Name { get; set; }
            public double AngleX { get; set; }
            public double AngleY { get; set; }
            public double AngleZ { get; set; }
            public double Radius { get; set; }
            public TCPModel TCP { get; set; }
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

        [Serializable]
        public class TCPModel
        {
            public TCPModel()
            {
                X = 0;
                Y = 0;
                Z = 0;
            }

            public double X { get; set; }
            public double Y { get; set; }
            public double Z { get; set; }
        }

    }
}


