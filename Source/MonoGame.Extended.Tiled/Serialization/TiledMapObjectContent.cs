using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace MonoGame.Extended.Tiled.Serialization
{
    // This content class is going to be a lot more complex than the others we use.
    // Objects can reference a template file which has starting values for the
    // object. The value in the object file overrides any value specified in the
    // template. All values have to be able to store a null value so we know if the
    // XML parser actually found a value for the property and not just a default
    // value. Default values are used when the object and any templates don't 
    // specify a value.
    public class TiledMapObjectContent : IXmlSerializable
    {
        public TiledMapObjectContent()
        {

        }

        //private uint? _globalIdentifier;
        //private int? _identifier;
        //private float? _height;
        //private float? _rotation;
        //private bool? _visible;
        //private float? _width;
        //private float? _x;
        //private float? _y;

        [XmlAttribute(DataType = "int", AttributeName = "id")]
        public int? Identifier { get; set; }
        //{
        //    get => _identifier ?? 0;
        //    set => _identifier = value;
        //}

        [XmlAttribute(DataType = "string", AttributeName = "name")]
        public string Name { get; set; }

        [XmlAttribute(DataType = "string", AttributeName = "type")]
        public string Type { get; set; }

        [XmlAttribute(DataType = "float", AttributeName = "x")]
        public float? X { get; set; }
        //{
        //    get => _x ?? 0;
        //    set => _x = value;
        //}

        [XmlAttribute(DataType = "float", AttributeName = "y")]
        public float? Y { get; set; }
        //{
        //    get => _y ?? 0;
        //    set => _y = value;
        //}

        [XmlAttribute(DataType = "float", AttributeName = "width")]
        public float? Width { get; set; }
        //{
        //    get => _width ?? 0;
        //    set => _width = value;
        //}

        [XmlAttribute(DataType = "float", AttributeName = "height")]
        public float? Height { get; set; }
        //{
        //    get => _height ?? 0;
        //    set => _height = value;
        //}

        [XmlAttribute(DataType = "float", AttributeName = "rotation")]
        public float? Rotation { get; set; }
        //{
        //    get => _rotation ?? 0;
        //    set => _rotation = value;
        //}

        [XmlAttribute(DataType = "boolean", AttributeName = "visible")]
        public bool? Visible { get; set; }
        //{
        //    get => _visible ?? true;
        //    set => _visible = value;
        //}

        [XmlArray("properties")]
        [XmlArrayItem("property")]
        public List<TiledMapPropertyContent> Properties { get; set; }

        [XmlAttribute(DataType = "unsignedInt", AttributeName = "gid")]
        public uint? GlobalIdentifier { get; set; }
        //{
        //    get => _globalIdentifier ?? 0;
        //    set => _globalIdentifier = value;
        //}

        [XmlElement(ElementName = "ellipse")]
        public TiledMapEllipseContent Ellipse { get; set; }

        [XmlElement(ElementName = "polygon")]
        public TiledMapPolygonContent Polygon { get; set; }

        [XmlElement(ElementName = "polyline")]
        public TiledMapPolylineContent Polyline { get; set; }

        [XmlAttribute(DataType = "string", AttributeName = "template")]
        public string TemplateSource { get; set; }

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            // TODO: Finish this
        }

        public void WriteXml(XmlWriter writer)
        {
            // TODO: Finish this
        }
    }
}