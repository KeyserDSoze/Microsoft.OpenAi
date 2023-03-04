﻿namespace Azure.Ai.OpenAi.Models
{
    public enum EditModelType
    {
        /// <summary>
        /// Optimized for text-edit tasks
        /// </summary>
        TextDavinciEdit,
        /// <summary>
        /// Optimized for code-edit tasks
        /// </summary>
        CodeDavinciEdit = 100,
    }
}
