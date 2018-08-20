using System.Collections.Generic;

namespace MonoGame.Extended.Content.Pipeline.Aseprite
{
    public class Aseprite
    {
        public Aseprite()
        {
            Frames = new List<AsepriteFrame>();
            Layers = new List<AsepriteLayer>();
            Tags = new List<AsepriteTag>();
            Slices = new List<AsepriteSlice>();
        }

        public Modes Mode { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public List<AsepriteFrame> Frames { get; }
        public List<AsepriteLayer> Layers { get; }
        public List<AsepriteTag> Tags { get; }
        public List<AsepriteSlice> Slices { get; }

        public enum Chunks
        {
            OldPaletteA = 0x0004,
            OldPaletteB = 0x0011,
            Layer = 0x2004,
            Cel = 0x2005,
            CelExtra = 0x2006,
            Mask = 0x2016,
            Path = 0x2017,
            FrameTags = 0x2018,
            Palette = 0x2019,
            UserData = 0x2020,
            Slice = 0x2022
        }

        public enum CelTypes
        {
            RawCel = 0,
            LinkedCel = 1,
            CompressedImage = 2
        }
    }
}