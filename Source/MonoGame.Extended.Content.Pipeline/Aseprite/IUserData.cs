using Microsoft.Xna.Framework;

namespace MonoGame.Extended.Content.Pipeline.Aseprite
{
    public interface IUserData
    {
        string UserDataText { get; set; }
        Color UserDataColor { get; set; }
    }
}