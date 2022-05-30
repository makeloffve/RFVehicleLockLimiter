using System;
using System.Xml.Serialization;

namespace RFVehicleLockLimiter.Models
{
    [Serializable]
    public class Vehicle
    {
        [XmlAttribute]
        public ushort Id { get; set; }
    }
}