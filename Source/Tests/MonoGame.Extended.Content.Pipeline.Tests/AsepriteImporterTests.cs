using Microsoft.Xna.Framework.Content.Pipeline;
using MonoGame.Extended.Content.Pipeline.Aseprite;
using NSubstitute;
using Xunit;

namespace MonoGame.Extended.Content.Pipeline.Tests
{
    public class AsepriteImporterTests
    {
        [Fact]
        public void AsepriteImporter_Import_Test()
        {
            var filePath = PathExtensions.GetApplicationFullPath("TestData", "snowman.aseprite");
            var importer = new AsepriteImporter();
            var context = Substitute.For<ContentImporterContext>();
            context.Logger.Returns(Substitute.For<ContentBuildLogger>());

            var result = importer.Import(filePath, context);
            var data = result.Data;

        }
    }
}