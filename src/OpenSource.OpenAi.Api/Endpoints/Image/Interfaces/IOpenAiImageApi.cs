using System.IO;

namespace OpenSource.OpenAi.Image
{
    public interface IOpenAiImageApi
    {
        ImageCreateRequestBuilder Generate(string prompt);
        ImageVariationRequestBuilder Variate(Stream image, string imageName = "image.png");
    }
}
