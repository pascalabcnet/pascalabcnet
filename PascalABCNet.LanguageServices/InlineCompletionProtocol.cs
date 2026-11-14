using Newtonsoft.Json;

namespace PascalABCNet.LanguageServices;

public sealed class InlineCompletionParamsDto
{
    [JsonProperty("textDocument")]
    public InlineCompletionDocumentDto? TextDocument { get; set; }

    [JsonProperty("position")]
    public InlineCompletionPositionDto? Position { get; set; }
}

public sealed class InlineCompletionDocumentDto
{
    [JsonProperty("uri")]
    public Uri? Uri { get; set; }
}

public sealed class InlineCompletionPositionDto
{
    [JsonProperty("line")]
    public int Line { get; set; }

    [JsonProperty("character")]
    public int Character { get; set; }
}

public sealed class InlineCompletionResultDto
{
    [JsonProperty("items")]
    public InlineCompletionItemDto[] Items { get; set; } = Array.Empty<InlineCompletionItemDto>();
}

public sealed class InlineCompletionItemDto
{
    [JsonProperty("insertText")]
    public string InsertText { get; set; } = string.Empty;
}
