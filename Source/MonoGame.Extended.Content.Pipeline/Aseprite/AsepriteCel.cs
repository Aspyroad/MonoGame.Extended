using Microsoft.Xna.Framework;

namespace MonoGame.Extended.Content.Pipeline.Aseprite
{
    public class AsepriteCel : IUserData
    {
        public Color[] Pixels;

        public int X;
        public int Y;
        public int Width;
        public int Height;
        public float Opacity;

        public string UserDataText { get; set; }
        public Color UserDataColor { get; set; }
        public ushort LayerIndex { get; set; }
    }
}