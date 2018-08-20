using System.IO;
using System.IO.Compression;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content.Pipeline;

namespace MonoGame.Extended.Content.Pipeline.Aseprite
{
    // https://github.com/aseprite/aseprite/blob/master/docs/ase-file-specs.md

    [ContentImporter(".ase", ".aseprite", DefaultProcessor = "AsepriteProcessor", DisplayName = "Aseprite Importer - MonoGame.Extended")]
    public class AsepriteImporter : ContentImporter<AsepriteContent>
    {
        public override AsepriteContent Import(string filename, ContentImporterContext context)
        {
            var aseprite = new Aseprite();

            using (var stream = File.OpenRead(filename))
            {
                var reader = new BinaryReader(stream);

                var header = new AsepriteHeader
                {
                    FileSize = reader.ReadDword(),
                    MagicNumber = reader.ReadWord(),
                    FrameCount = reader.ReadWord(),
                    Width = reader.ReadWord(),
                    Height = reader.ReadWord(),
                    Mode = (Modes) (reader.ReadWord() / 8),
                    Flags = reader.ReadDword(),
                    Speed = reader.ReadWord(),
                    NotUsedA = reader.ReadDword(),
                    NotUsedB = reader.ReadDword(),
                    TrasparentPaletteIndex = reader.ReadByte(),
                    NotUsedC = reader.ReadBytes(3), // Ignore these bytes
                    ColorCount = reader.ReadWord(),
                    PixelWidth = reader.ReadByte(),
                    PixelHeight = reader.ReadByte(),
                };

                reader.Seek(92); // For Future

                // Some temporary holders
                var colorBuffer = new byte[aseprite.Width * aseprite.Height * (int)aseprite.Mode];
                var palette = new Color[256];

                IUserData lastUserData = null;

                for (var i = 0; i < header.FrameCount; i++)
                {
                    var frame = new AsepriteFrame();
                    aseprite.Frames.Add(frame);

                    long frameEnd;
                    int chunkCount;

                    // Frame header
                    {
                        var frameStart = reader.BaseStream.Position;
                        frameEnd = frameStart + reader.ReadDword();
                        reader.ReadWord(); // Magic number (always 0xF1FA)
                        chunkCount = reader.ReadWord();
                        frame.Duration = reader.ReadWord() / 1000f;
                        reader.Seek(6); // For future (set to zero)
                    }

                    for (var j = 0; j < chunkCount; j++)
                    {
                        long chunkEnd;
                        Aseprite.Chunks chunkType;

                        // Chunk header
                        {
                            var chunkStart = reader.BaseStream.Position;
                            chunkEnd = chunkStart + reader.ReadDword();
                            chunkType = (Aseprite.Chunks)reader.ReadWord();
                        }

                        // Layer
                        if (chunkType == Aseprite.Chunks.Layer)
                        {
                            var layer = new AsepriteLayer
                            {
                                Flag = (AsepriteLayer.Flags) reader.ReadWord(),
                                Type = (AsepriteLayer.Types) reader.ReadWord(),
                                ChildLevel = reader.ReadWord()
                            };
                            reader.ReadWord(); // width
                            reader.ReadWord(); // height
                            layer.BlendMode = (AsepriteLayer.BlendModes)reader.ReadWord();
                            layer.Opacity = reader.ReadByte() / 255f;
                            reader.Seek(3);
                            layer.Name = reader.ReadFixedString();

                            lastUserData = layer;
                            aseprite.Layers.Add(layer);
                        }

                        // Cel
                        else if (chunkType == Aseprite.Chunks.Cel)
                        {
                            var cel = new AsepriteCel
                            {
                                LayerIndex = reader.ReadWord(),
                                X = reader.ReadShort(),
                                Y = reader.ReadShort(),
                                Opacity = reader.ReadByte() / 255f
                            };


                            var celType = (Aseprite.CelTypes)reader.ReadWord();
                            reader.Seek(7);
                            if (celType == Aseprite.CelTypes.RawCel || celType == Aseprite.CelTypes.CompressedImage)
                            {
                                cel.Width = reader.ReadWord();
                                cel.Height = reader.ReadWord();

                                var byteCount = cel.Width * cel.Height * (int)aseprite.Mode;

                                if (celType == Aseprite.CelTypes.RawCel)
                                {
                                    reader.Read(colorBuffer, 0, byteCount);
                                }
                                else
                                {
                                    reader.Seek(2);
                                    var deflate = new DeflateStream(reader.BaseStream, CompressionMode.Decompress);
                                    deflate.Read(colorBuffer, 0, byteCount);
                                }

                                cel.Pixels = new Color[cel.Width * cel.Height];
                                ConvertBytesToPixels(aseprite.Mode, colorBuffer, cel.Pixels, palette);
                            }
                            else if (celType == Aseprite.CelTypes.LinkedCel)
                            {
                                var targetFrame = reader.ReadWord(); // Frame position to link with

                                // Grab the cel from a previous frame
                                var targetCel = aseprite.Frames[targetFrame].Cels.First(c => c.LayerIndex == cel.LayerIndex);
                                cel.Width = targetCel.Width;
                                cel.Height = targetCel.Height;
                                cel.Pixels = targetCel.Pixels;
                            }

                            lastUserData = cel;
                            frame.Cels.Add(cel);
                        }

                        // Palette
                        else if (chunkType == Aseprite.Chunks.Palette)
                        {
                            // ReSharper disable once UnusedVariable
                            var size = reader.ReadDword();
                            var start = reader.ReadDword();
                            var end = reader.ReadDword();
                            reader.Seek(8);

                            for (var c = 0; c < (end - start); c++)
                            {
                                var hasName = BitHelper.IsBitSet(reader.ReadWord(), 0);
                                palette[start + c] = Color.FromNonPremultiplied(reader.ReadByte(), reader.ReadByte(), reader.ReadByte(), reader.ReadByte());
                                if (hasName)
                                {
                                    reader.ReadFixedString(); // Color name
                                }
                            }
                        }

                        // User data
                        else if (chunkType == Aseprite.Chunks.UserData)
                        {
                            if (lastUserData != null)
                            {
                                var flags = reader.ReadDword();

                                if (BitHelper.IsBitSet(flags, 0))
                                {
                                    lastUserData.UserDataText = reader.ReadFixedString();
                                }
                                else if (BitHelper.IsBitSet(flags, 1))
                                {
                                    lastUserData.UserDataColor = Color.FromNonPremultiplied(reader.ReadByte(), reader.ReadByte(), reader.ReadByte(), reader.ReadByte());
                                }
                            }
                        }

                        // Tag (animation reference)
                        else if (chunkType == Aseprite.Chunks.FrameTags)
                        {
                            var tagsCount = reader.ReadWord();
                            reader.Seek(8);

                            for (var t = 0; t < tagsCount; t++)
                            {
                                var tag = new AsepriteTag
                                {
                                    From = reader.ReadWord(),
                                    To = reader.ReadWord(),
                                    LoopDirection = (AsepriteTag.LoopDirections)reader.ReadByte()
                                };

                                reader.Seek(8);
                                tag.Color = Color.FromNonPremultiplied(reader.ReadByte(), reader.ReadByte(), reader.ReadByte(), 255);
                                reader.Seek(1);
                                tag.Name = reader.ReadFixedString();

                                aseprite.Tags.Add(tag);
                            }
                        }

                        // Slice
                        else if (chunkType == Aseprite.Chunks.Slice)
                        {
                            var slicesCount = reader.ReadDword();
                            var flags = reader.ReadDword();
                            reader.ReadDword();
                            var name = reader.ReadFixedString();

                            for (var s = 0; s < slicesCount; s++)
                            {
                                var slice = new AsepriteSlice
                                {
                                    Name = name,
                                    Frame = (int) reader.ReadDword(),
                                    OriginX = (int) reader.ReadLong(),
                                    OriginY = (int) reader.ReadLong(),
                                    Width = (int) reader.ReadDword(),
                                    Height = (int) reader.ReadDword()
                                };

                                // 9 slice
                                if (BitHelper.IsBitSet(flags, 0))
                                {
                                    reader.ReadLong(); // Center X position (relative to slice bounds)
                                    reader.ReadLong(); // Center Y position
                                    reader.ReadDword(); // Center width
                                    reader.ReadDword(); // Center height
                                }

                                // Pivot
                                else if (BitHelper.IsBitSet(flags, 1))
                                {
                                    slice.Pivot = new Point((int)reader.ReadDword(), (int)reader.ReadDword());
                                }

                                lastUserData = slice;
                                aseprite.Slices.Add(slice);
                            }
                        }

                        reader.BaseStream.Position = chunkEnd;
                    }

                    reader.BaseStream.Position = frameEnd;
                }
            }

            return new AsepriteContent(aseprite); 
        }

        private static void ConvertBytesToPixels(Modes mode, byte[] bytes, Color[] pixels, Color[] palette)
        {
            var length = pixels.Length;

            switch (mode)
            {
                case Modes.Rgba:
                    for (int pixel = 0, b = 0; pixel < length; pixel++, b += 4)
                    {
                        pixels[pixel].R = (byte)(bytes[b + 0] * bytes[b + 3] / 255);
                        pixels[pixel].G = (byte)(bytes[b + 1] * bytes[b + 3] / 255);
                        pixels[pixel].B = (byte)(bytes[b + 2] * bytes[b + 3] / 255);
                        pixels[pixel].A = bytes[b + 3];
                    }
                    break;

                case Modes.Grayscale:
                    for (int pixel = 0, b = 0; pixel < length; pixel++, b += 2)
                    {
                        pixels[pixel].R = pixels[pixel].G = pixels[pixel].B = (byte)(bytes[b + 0] * bytes[b + 1] / 255);
                        pixels[pixel].A = bytes[b + 1];
                    }
                    break;

                case Modes.Indexed:
                    for (int pixel = 0, paletteIndex = 0; pixel < length; pixel++, paletteIndex += 1)
                    {
                        pixels[pixel] = palette[paletteIndex];
                    }
                    break;
            }
        }
    }
}