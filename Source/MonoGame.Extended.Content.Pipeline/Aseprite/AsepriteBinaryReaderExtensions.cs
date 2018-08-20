using System.IO;
using System.Text;

namespace MonoGame.Extended.Content.Pipeline.Aseprite
{
    public static class AsepriteBinaryReaderExtensions
    {
        public static ushort ReadWord(this BinaryReader reader) => reader.ReadUInt16();
        public static short ReadShort(this BinaryReader reader) => reader.ReadInt16();
        public static uint ReadDword(this BinaryReader reader) => reader.ReadUInt32();
        public static long ReadLong(this BinaryReader reader) => reader.ReadInt32();
        public static string ReadFixedString(this BinaryReader reader) => Encoding.UTF8.GetString(reader.ReadBytes(ReadWord(reader)));
        public static void Seek(this BinaryReader reader, int number) => reader.BaseStream.Position += number;
    }
}