using Microsoft.Xna.Framework;

namespace MonoGame.Extended.Content.Pipeline.Aseprite
{
    public struct AsepriteSlice : IUserData
    {
        public int Frame;
        public string Name;
        public int OriginX;
        public int OriginY;
        public int Width;
        public int Height;
        public Point? Pivot;

        string IUserData.UserDataText { get; set; }
        Color IUserData.UserDataColor { get; set; }
    }
}