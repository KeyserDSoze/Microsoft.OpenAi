namespace Azure.Ai.OpenAi.Audio
{
    /// <summary>
    /// Given a prompt and an instruction, the model will return an edited version of the prompt.
    /// </summary>
    public interface IOpenAiAudioApi
    {
        /// <summary>
        /// Given a prompt and an instruction, the model will return an edited version of the prompt.
        /// </summary>
        /// <param name="instruction">The instruction that tells the model how to edit the prompt.</param>
        /// <returns></returns>
        AudioRequestBuilder Request(string instruction);
    }
}
