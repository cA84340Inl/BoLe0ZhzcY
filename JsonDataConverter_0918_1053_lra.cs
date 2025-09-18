// 代码生成时间: 2025-09-18 10:53:31
using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Components;

namespace BlazorApp
{
    /// <summary>
    /// A component that converts JSON data to a specified format.
    /// </summary>
    public partial class JsonDataConverter
    {
        [Parameter]
        public string InputJson { get; set; }

        [Parameter]
        public RenderFragment<JsonElement> ChildContent { get; set; }

        private string convertedJson = "";
        private bool conversionSuccess = true;
        private string errorMessage = "";

        /// <summary>
        /// Converts the input JSON to the specified format.
        /// </summary>
        protected override void OnInitialized()
        {
            try
            {
                if (!string.IsNullOrEmpty(InputJson))
                {
                    // Deserialize the input JSON to a JsonElement.
                    var jsonElement = JsonSerializer.Deserialize<JsonElement>(InputJson);

                    // Convert the JsonElement to a string in the desired format.
                    convertedJson = jsonElement.ToString();
                }
            }
            catch (JsonException ex)
            {
                // Handle any JSON deserialization errors.
                conversionSuccess = false;
                errorMessage = $"Error converting JSON: {ex.Message}";
            }
        }

        /// <summary>
        /// Renders the converted JSON or an error message if the conversion failed.
        /// </summary>
        /// <returns></returns>
        protected override RenderFragment GetChildContent(RenderTreeBuilder builder)
        {
            builder.OpenComponent<ChildContent>(0);
            builder.AddAttribute(1, "jsonElement", conversionSuccess ? JsonSerializer.Deserialize<JsonElement>(convertedJson) : null);
            builder.CloseComponent();
            return ChildContent;
        }
    }
}
