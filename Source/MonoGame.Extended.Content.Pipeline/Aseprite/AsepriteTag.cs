using Microsoft.Xna.Framework;

namespace MonoGame.Extended.Content.Pipeline.Aseprite
{
    public class AsepriteTag
    {
        public enum LoopDirections
        {
            Forward = 0,
            Reverse = 1,
            PingPong = 2
        }

        public string Name;
        public LoopDirections LoopDirection;
        public int From;
        public int To;
        public Color Color;
    }
}