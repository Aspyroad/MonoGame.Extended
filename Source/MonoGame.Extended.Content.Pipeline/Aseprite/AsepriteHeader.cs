namespace MonoGame.Extended.Content.Pipeline.Aseprite
{
    public class AsepriteHeader
    {
        public uint FileSize;
        public ushort MagicNumber; //if (magic != 0xA5e0) throw new Exception("File doesn't appear to be from Aseprite.");
        public ushort FrameCount;
        public ushort Width;
        public ushort Height;
        public Modes Mode;
        public uint Flags;
        public ushort Speed; // DEPRECATED: You should use the frame duration field from each frame header
        public uint NotUsedA;
        public uint NotUsedB;
        public byte TrasparentPaletteIndex;
        public byte[] NotUsedC;
        public ushort ColorCount; // Number of colors (0 means 256 for old sprites)
        public byte PixelWidth;
        public byte PixelHeight;
    }
}