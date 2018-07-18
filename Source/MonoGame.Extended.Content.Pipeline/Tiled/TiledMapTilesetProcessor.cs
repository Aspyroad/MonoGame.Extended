using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using System;
using MonoGame.Extended.Tiled.Serialization;

namespace MonoGame.Extended.Content.Pipeline.Tiled
{
	[ContentProcessor(DisplayName = "Tiled Map Tileset Processor - MonoGame.Extended")]
	public class TiledMapTilesetProcessor : ContentProcessor<TiledMapTilesetContent, TiledMapTilesetContent>
	{
		public override TiledMapTilesetContent Process(TiledMapTilesetContent tileset, ContentProcessorContext context)
		{
			try
			{
				ContentLogger.Logger = context.Logger;

				ContentLogger.Log($"Processing tileset '{tileset.Name}'");

                // TODO: What the heck to do here?
				// Build the Texture2D asset and load it as it will be saved as part of this tileset file.
				//tileset.Image.ContentRef = context.BuildAsset<Texture2DContent, Texture2DContent>(new ExternalReference<Texture2DContent>(tileset.Image.Source), "", new OpaqueDataDictionary() { { "ColorKeyColor", tileset.Image.TransparentColor }, { "ColorKeyEnabled", true } }, "", "");

				foreach (var tile in tileset.Tiles)
				{
				    // TODO: What the heck to do here?
                    //foreach (var obj in tile.Objects)
                    //    TiledMapObjectContent.Process(obj, context);
                }

                ContentLogger.Log($"Processed tileset '{tileset.Name}'");

				return tileset;
			}
			catch (Exception ex)
			{
				context.Logger.LogImportantMessage(ex.Message);
				throw ex;
			}
		}
	}
}
