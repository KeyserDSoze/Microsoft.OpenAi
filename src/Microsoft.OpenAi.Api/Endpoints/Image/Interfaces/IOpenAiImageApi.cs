namespace Azure.Ai.OpenAi
{
    public interface IOpenAiImageApi
    {
        ImageGenerationRequestBuilder Generate(string prompt);
        ImageVariationRequestBuilder Variate(Stream image, string imageName = "image.png");
    }
}
