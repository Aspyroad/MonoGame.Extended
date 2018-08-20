using System;
using Microsoft.Xna.Framework;

namespace MonoGame.Extended.Content.Pipeline.Aseprite
{
    public class AsepriteLayer : IUserData
    {
        [Flags]
        public enum Flags
        {
            Visible = 1,
            Editable = 2,
            LockMovement = 4,
            Background = 8,
            PreferLinkedCels = 16,
            Collapsed = 32,
            Reference = 64
        }

        public enum Types
        {
            Normal = 0,
            Group = 1
        }

        public enum BlendModes
        {
            Normal = 0,
            Multiply = 1,
            Screen = 2,
            Overlay = 3,
            Darken = 4,
            Lighten = 5,
            ColorDodge = 6,
            ColorBurn = 7,
            HardLight = 8,
            SoftLight = 9,
            Difference = 10,
            Exclusion = 11,
            Hue = 12,
            Saturation = 13,
            Color = 14,
            Luminosity = 15,
            Addition = 16,
            Subtract = 17,
            Divide = 18
        }

        public Flags Flag;
        public Types Type;

        public string Name;
        public float Opacity;
        public BlendModes BlendMode;
        public int ChildLevel;

        string IUserData.UserDataText { get; set; }
        Color IUserData.UserDataColor { get; set; }
    }
}