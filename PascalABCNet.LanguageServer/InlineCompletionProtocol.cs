using Newtonsoft.Json;

namespace PascalABCNet.LanguageServer;

internal sealed class InlineCompletionParamsDto
{
    [JsonProperty("textDocument")]
    public InlineCompletionDocumentDto? TextDocument { get; set; }

    [JsonProperty("position")]
    public InlineCompletionPositionDto? Position { get; set; }
}

internal sealed class InlineCompletionDocumentDto
{
    [JsonProperty("uri")]
    public Uri? Uri { get; set; }
}

internal sealed class InlineCompletionPositionDto
{
    [JsonProperty("line")]
    public int Line { get; set; }

    [JsonProperty("character")]
    public int Character { get; set; }
}

internal sealed class InlineCompletionResultDto
{
    [JsonProperty("items")]
    public InlineCompletionItemDto[] Items { get; set; } = Array.Empty<InlineCompletionItemDto>();
}

internal sealed class InlineCompletionItemDto
{
    [JsonProperty("insertText")]
    public string InsertText { get; set; } = string.Empty;
}
