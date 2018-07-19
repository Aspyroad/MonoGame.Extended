using System.Collections.Generic;
using System.Xml.Serialization;

namespace MonoGame.Extended.Tiled.Serialization
{
    // Objects can reference a template file which has starting values for the
    // object. The value in the object file overrides any value specified in the
    // template.
    //
    // To handle this we're using an annoying quirk of the XmlSerializer known as
    // the specified pattern to determin if the attribute actually exists in the XML.
    //
    // Unfortunately, there's no great way to do this unless we change to a different
    // XML serializer.
    public class TiledMapObjectContent
    {
        public TiledMapObjectContent()
        {
        }

        [XmlAttribute(DataType = "int", AttributeName = "id")]
        public int Identifier { get; set; }
        [XmlIgnore]
        public bool IdentifierSpecified { get; set; }

        [XmlAttribute(DataType = "string", AttributeName = "name")]
        public string Name { get; set; }

        [XmlAttribute(DataType = "string", AttributeName = "type")]
        public string Type { get; set; }

        [XmlAttribute(DataType = "float", AttributeName = "x")]
        public float X { get; set; }
        [XmlIgnore]
        public bool XSpecified { get; set; }

        [XmlAttribute(DataType = "float", AttributeName = "y")]
        public float Y { get; set; }
        [XmlIgnore]
        public bool YSpecified { get; set; }
        
        [XmlAttribute(DataType = "float", AttributeName = "width")]
        public float Width { get; set; }
        [XmlIgnore]
        public bool WidthSpecified { get; set; }

        [XmlAttribute(DataType = "float", AttributeName = "height")]
        public float Height { get; set; }
        [XmlIgnore]
        public bool HeightSpecified { get; set; }

        [XmlAttribute(DataType = "float", AttributeName = "rotation")]
        public float Rotation { get; set; }
        [XmlIgnore]
        public bool RotationSpecified { get; set; }

        [XmlAttribute(DataType = "boolean", AttributeName = "visible")]
        public bool Visible { get; set; }
        [XmlIgnore]
        public bool VisibleSpecified { get; set; }

        [XmlAttribute(DataType = "unsignedInt", AttributeName = "gid")]
        public uint GlobalIdentifier { get; set; }
        [XmlIgnore]
        public bool GlobalIdentifierSpecified { get; set; }

        [XmlArray("properties")]
        [XmlArrayItem("property")]
        public List<TiledMapPropertyContent> Properties { get; set; }

        [XmlElement(ElementName = "ellipse")]
        public TiledMapEllipseContent Ellipse { get; set; }

        [XmlElement(ElementName = "polygon")]
        public TiledMapPolygonContent Polygon { get; set; }

        [XmlElement(ElementName = "polyline")]
        public TiledMapPolylineContent Polyline { get; set; }

        [XmlAttribute(DataType = "string", AttributeName = "template")]
        public string TemplateSource { get; set; }
    }
}