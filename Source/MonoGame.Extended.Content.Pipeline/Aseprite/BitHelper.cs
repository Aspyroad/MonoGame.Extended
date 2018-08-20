namespace MonoGame.Extended.Content.Pipeline.Aseprite
{
    public static class BitHelper
    {
        public static bool IsBitSet(uint value, int pos)
        {
            return (value & (1 << pos)) != 0;
        }
    }
}