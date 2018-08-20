using System.Collections.Generic;

namespace MonoGame.Extended.Content.Pipeline.Aseprite
{
    public class AsepriteFrame
    {
        public float Duration;
        public List<AsepriteCel> Cels;


        public AsepriteFrame()
        {
            Cels = new List<AsepriteCel>();
        }
    }
}