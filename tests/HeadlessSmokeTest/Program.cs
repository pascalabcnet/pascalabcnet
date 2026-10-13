using CodeCompletion;
using Languages.Facade;
using Languages.Pascal;
using PascalABCNet.LanguageServices;

foreach (var assembly in new[]
         {
             typeof(PascalABCCompiler.Compiler).Assembly,
             typeof(CodeCompletionController).Assembly,
             typeof(PascalLanguageService).Assembly
         })
{
    Check(string.Equals(Path.GetDirectoryName(assembly.Location),
        Path.TrimEndingDirectorySeparator(AppContext.BaseDirectory),
        StringComparison.OrdinalIgnoreCase),
        $"{assembly.GetName().Name} is loaded from the shared runtime directory");
}
Console.WriteLine("PASS compiler, CodeCompletion and LanguageServices use one runtime directory");

const string fileName = "HeadlessSmoke.pas";
const string source = """
program HeadlessSmoke;

var
  text: string;

begin
  text := 'PascalABC.NET';
  var result := text.Substring(1);
  Writeln(result);
end.
""";

var language = PascalLanguageRegistration.RegisterPascalLanguage();
var secondRegistration = PascalLanguageRegistration.RegisterPascalLanguage();
Check(ReferenceEquals(language, secondRegistration), "Pascal registration is idempotent");
Check(ReferenceEquals(language, LanguageProvider.Instance.MainLanguage), "Pascal is the main language");
Check(
    ReferenceEquals(language, LanguageProvider.Instance.SelectLanguageByExtension(fileName)),
    "A .pas document resolves to the explicitly registered Pascal language");
Console.WriteLine("PASS registration without LoadAllLanguages()");

CodeCompletionBootstrap.Initialize("ru");
Check(
    PascalABCCompiler.StringResourcesLanguage.CurrentTwoLetterISO == "ru",
    "Russian headless bootstrap is active");
CodeCompletionBootstrap.Initialize("en");
Check(
    PascalABCCompiler.StringResourcesLanguage.CurrentTwoLetterISO == "en",
    "English headless bootstrap is active");
Console.WriteLine("PASS bootstrap for ru and en");

Check(source.Length > 0, "Pascal source text is loaded");
Console.WriteLine("PASS Pascal source text loaded");

var controller = new CodeCompletionController();
var domConverter = controller.Compile(fileName, source);
Check(domConverter is not null && domConverter.is_compiled, "DomConverter was built");
Check(domConverter!.LastConversionError is null, "Successful DOM conversion has no error");
Console.WriteLine("PASS DomConverter created");

const string memberAccess = "text.Substring";
var memberStart = source.IndexOf(memberAccess, StringComparison.Ordinal);
Check(memberStart >= 0, "Completion marker exists");
var dotOffset = memberStart + "text.".Length;
var completionPrefix = source[..dotOffset];
var completionPosition = GetPosition(completionPrefix, completionPrefix.Length);
var completionContext = new CompletionTriggerContext { DotPressed = true };
var expressionInfo = CompletionExpressionService.AnalyzeAtCaret(
    dotOffset,
    completionPrefix,
    completionPosition.Line,
    completionPosition.Column,
    language.LanguageIntellisenseSupport,
    completionContext,
    PascalABCCompiler.Parsers.KeywordKind.None);
var parseResult = CompletionExpressionService.Parse(
    language.Parser,
    fileName,
    expressionInfo.ExpressionText,
    completionContext);
Check(!parseResult.ShouldAbortCompletion && parseResult.Expression is not null, "Completion expression parsed");
var completion = CompletionSymbolService.GetSymbols(
    domConverter,
    completionPosition.Line,
    completionPosition.Column,
    completionContext,
    language.CaseSensitive,
    expressionInfo.ExpressionText,
    expressionInfo.CtrlOrShiftSpaceAfterDot,
    expressionInfo.InsidePatternWithDots,
    expressionInfo.Pattern,
    parseResult.Expression,
    expressionInfo.Keyword,
    smartIntellisense: true,
    namespaceVisibleRange: 0);
var completionNames = completion.Symbols is null
    ? "<null>"
    : string.Join(", ", completion.Symbols.Where(symbol => symbol is not null).Select(symbol => symbol.name));
Check(
    completion.Symbols?.Any(symbol => symbol is not null && symbol.name == "Substring") == true,
    $"Completion after text. contains Substring (expression: {expressionInfo.ExpressionText}; symbols: {completionNames})");
Console.WriteLine("PASS completion after dot");

var hoverOffset = memberStart + "text.".Length + 1;
var hoverPosition = GetPosition(source, hoverOffset);
var hover = HoverService.GetDescription(
    language,
    domConverter,
    fileName,
    source,
    hoverOffset,
    hoverPosition.Line,
    hoverPosition.Column);
Check(!string.IsNullOrWhiteSpace(hover), "Hover returned a description");
Check(hover.Contains("Substring", StringComparison.OrdinalIgnoreCase), "Hover describes Substring");
Console.WriteLine("PASS hover over identifier");

var openParenthesisOffset = memberStart + memberAccess.Length + 1;
var signaturePrefix = source[..openParenthesisOffset];
var signaturePosition = GetPosition(signaturePrefix, signaturePrefix.Length);
var signatureHelp = SignatureHelpService.GetSignatureHelpAtCaret(
    language,
    domConverter,
    fileName,
    signaturePrefix,
    openParenthesisOffset,
    signaturePosition.Line,
    signaturePosition.Column,
    '(',
    currentParameter: 1,
    currentParameterForSelection: 1);
Check(signatureHelp?.Signatures?.Length > 0, "Signature help returned at least one signature");
Check(
    signatureHelp!.Signatures.Any(signature => signature.Contains("Substring", StringComparison.OrdinalIgnoreCase)),
    "Signature help describes Substring");
Console.WriteLine("PASS signature help for method call");

const string invalidSource = "program Broken; begin var value := ; end.";
var invalidDomConverter = controller.Compile("Broken.pas", invalidSource);
Check(!invalidDomConverter.is_compiled, "Invalid source must not produce a compiled DOM");
Console.WriteLine(
    invalidDomConverter.LastConversionError is null
        ? "PASS invalid source rejected before DOM conversion"
        : $"PASS invalid source rejected during DOM conversion: {invalidDomConverter.LastConversionError.GetType().Name}");

const string documentId = "file:///HeadlessSmoke.pas";
var languageService = new PascalLanguageService("en");
var analysis = await languageService.OpenOrUpdateDocumentAsync(documentId, fileName, source, version: 1);
Check(analysis.IsCompiled && analysis.LastConversionError is null, "LanguageServices compiled the document");
Check(
    languageService.Documents.TryGet(documentId, out var storedDocument) && storedDocument?.Version == 1,
    "LanguageServices stores the open document snapshot");

var completionTask = languageService.GetCompletionAfterDotAsync(documentId, dotOffset);
var hoverTask = languageService.GetHoverAsync(documentId, hoverOffset);
var signatureTask = languageService.GetSignatureHelpAsync(documentId, openParenthesisOffset, '(');
await Task.WhenAll(completionTask, hoverTask, signatureTask);

Check(
    completionTask.Result.Any(item => item.Label == "Substring"),
    "LanguageServices completion contains Substring");
Check(
    hoverTask.Result?.Contents.Contains("Substring", StringComparison.OrdinalIgnoreCase) == true,
    "LanguageServices hover describes Substring");
Check(
    signatureTask.Result?.Signatures.Any(
        signature => signature.Label.Contains("Substring", StringComparison.OrdinalIgnoreCase)) == true,
    "LanguageServices signature help describes Substring");
Check(
    signatureTask.Result?.Signatures.Any(signature => signature.Parameters.Count > 0) == true,
    "LanguageServices signature help contains parameters");

const string printDocumentId = "file:///PrintSignature.pas";
const string printSource = """
begin
  Print(1)
end.
""";
var russianLanguageService = new PascalLanguageService("ru");
var printAnalysis = await russianLanguageService.OpenOrUpdateDocumentAsync(
    printDocumentId,
    "PrintSignature.pas",
    printSource,
    version: 1);
Check(printAnalysis.IsCompiled, "Print signature document is analyzed");
var printSignatureOffset = printSource.IndexOf("Print(", StringComparison.Ordinal) + "Print(".Length;
var printSignatureHelp = await russianLanguageService.GetSignatureHelpAsync(
    printDocumentId,
    printSignatureOffset,
    '(');
Check(
    printSignatureHelp?.Signatures.Any(signature =>
        signature.Label.Contains("Print", StringComparison.OrdinalIgnoreCase) &&
        signature.Parameters.Count > 0) == true,
    "Print signature help contains parameters");

const string dateTimeDocumentId = "file:///DateTimeCompletion.pas";
const string dateTimeBeforeDot = """
begin
  DateTime
end.
""";
const string dateTimeAfterDot = """
begin
  DateTime.
end.
""";
var dateTimeInitialAnalysis = await languageService.OpenOrUpdateDocumentAsync(
    dateTimeDocumentId,
    "DateTimeCompletion.pas",
    dateTimeBeforeDot,
    version: 1);
Check(dateTimeInitialAnalysis.IsCompiled, "DateTime document is analyzed before typing dot");
await languageService.QueueDocumentUpdateAsync(
    dateTimeDocumentId,
    "DateTimeCompletion.pas",
    dateTimeAfterDot,
    version: 2);
var dateTimeCaretOffset = dateTimeAfterDot.IndexOf("DateTime.", StringComparison.Ordinal) + "DateTime.".Length;
var dateTimeCompletion = await languageService.GetCompletionAfterDotAsync(
    dateTimeDocumentId,
    dateTimeCaretOffset);
Check(
    new[] { "Now", "Today", "UtcNow" }.All(expected =>
        dateTimeCompletion.Any(item => item.Label == expected)),
    $"DateTime completion contains Now, Today and UtcNow; actual: {string.Join(", ", dateTimeCompletion.Select(item => item.Label))}");
Console.WriteLine("PASS DateTime completion after interactive dot");

const string arrayDocumentId = "file:///ArrayAsSpanCompletion.pas";
const string arrayBeforeDot = """
begin
  var a := Arr(1,2,3,4,5,6);
  var s := a
end.
""";
const string arrayAfterDot = """
begin
  var a := Arr(1,2,3,4,5,6);
  var s := a.
end.
""";
var arrayInitialAnalysis = await languageService.OpenOrUpdateDocumentAsync(
    arrayDocumentId,
    "ArrayAsSpanCompletion.pas",
    arrayBeforeDot,
    version: 1);
Check(arrayInitialAnalysis.IsCompiled, "Array document is analyzed before typing dot");
await languageService.QueueDocumentUpdateAsync(
    arrayDocumentId,
    "ArrayAsSpanCompletion.pas",
    arrayAfterDot,
    version: 2);
var arrayCaretOffset = arrayAfterDot.IndexOf("a.", StringComparison.Ordinal) + "a.".Length;
var arrayCompletion = await languageService.GetCompletionAfterDotAsync(arrayDocumentId, arrayCaretOffset);
Check(
    arrayCompletion.Any(item => item.Label.StartsWith("AsSpan", StringComparison.Ordinal)),
    $"Array completion contains AsSpan; actual: {string.Join(", ", arrayCompletion.Select(item => item.Label))}");
Console.WriteLine("PASS array AsSpan completion after interactive dot");

const string arraySignatureDocumentId = "file:///ArrayAsSpanSignature.pas";
const string arraySignatureSource = """
begin
  var a := Arr(1,2,3,4,5,6);
  var s := a.AsSpan()
end.
""";
var arraySignatureAnalysis = await languageService.OpenOrUpdateDocumentAsync(
    arraySignatureDocumentId,
    "ArrayAsSpanSignature.pas",
    arraySignatureSource,
    version: 1);
Check(arraySignatureAnalysis.IsCompiled, "Array AsSpan signature document is analyzed");
var arraySignatureOffset =
    arraySignatureSource.IndexOf("AsSpan(", StringComparison.Ordinal) + "AsSpan(".Length;
var arraySignatureHelp = await languageService.GetSignatureHelpAsync(
    arraySignatureDocumentId,
    arraySignatureOffset,
    '(');
Check(
    arraySignatureHelp?.Signatures.Any(signature =>
        signature.Label.Contains("AsSpan", StringComparison.OrdinalIgnoreCase)) == true,
    "Array AsSpan signature help contains AsSpan");
Console.WriteLine("PASS array AsSpan signature help");

const string queueDocumentId = "file:///PriorityQueueCompletion.pas";
const string queueBeforeDot = """
uses System.Collections.Generic;

begin
  var q := new PriorityQueue<string,integer>;
  q
end.
""";
const string queueAfterDot = """
uses System.Collections.Generic;

begin
  var q := new PriorityQueue<string,integer>;
  q.
end.
""";
var queueInitialAnalysis = await languageService.OpenOrUpdateDocumentAsync(
    queueDocumentId,
    "PriorityQueueCompletion.pas",
    queueBeforeDot,
    version: 1);
Check(queueInitialAnalysis.IsCompiled, "PriorityQueue document is analyzed before typing dot");
await languageService.QueueDocumentUpdateAsync(
    queueDocumentId,
    "PriorityQueueCompletion.pas",
    queueAfterDot,
    version: 2);
var queueCaretOffset = queueAfterDot.IndexOf("q.", StringComparison.Ordinal) + "q.".Length;
var queueCompletion = await languageService.GetCompletionAfterDotAsync(queueDocumentId, queueCaretOffset);
Check(
    queueCompletion.Any(item => item.Label == "Enqueue"),
    $"PriorityQueue completion contains Enqueue; actual: {string.Join(", ", queueCompletion.Select(item => item.Label))}");
Check(
    queueCompletion.All(item => item.Label != "ReadInteger"),
    "PriorityQueue completion does not contain string.ReadInteger");
Console.WriteLine("PASS PriorityQueue completion uses the correct generic type");

const string queueCallDocumentId = "file:///PriorityQueueCall.pas";
const string queueCallSource = """
uses System.Collections.Generic;

begin
  var q := new PriorityQueue<string,integer>;
  q.Enqueue('low',10)
end.
""";
var queueCallAnalysis = await languageService.OpenOrUpdateDocumentAsync(
    queueCallDocumentId,
    "PriorityQueueCall.pas",
    queueCallSource,
    version: 1);
Check(queueCallAnalysis.IsCompiled, "PriorityQueue call document is analyzed");
var queueHoverOffset = queueCallSource.IndexOf("Enqueue", StringComparison.Ordinal) + 1;
var queueHover = await languageService.GetHoverAsync(queueCallDocumentId, queueHoverOffset);
Check(
    queueHover?.Contents.Contains("Enqueue", StringComparison.OrdinalIgnoreCase) == true,
    "PriorityQueue hover describes Enqueue");
var queueSignatureOffset =
    queueCallSource.IndexOf("Enqueue(", StringComparison.Ordinal) + "Enqueue(".Length;
var queueSignatureHelp = await languageService.GetSignatureHelpAsync(
    queueCallDocumentId,
    queueSignatureOffset,
    '(');
Check(
    queueSignatureHelp?.Signatures.Any(signature =>
        signature.Label.Contains("Enqueue", StringComparison.OrdinalIgnoreCase)) == true,
    "PriorityQueue signature help contains Enqueue");
Check(
    queueSignatureHelp?.Signatures.Any(signature => signature.Parameters.Count >= 2) == true,
    "PriorityQueue signature help contains both parameters");
Console.WriteLine("PASS PriorityQueue hover and signature help");

var dependencyDirectory = Path.Combine(Path.GetTempPath(), "PascalABCNet.HeadlessSmokeTest." + Guid.NewGuid().ToString("N"));
var dependencyFileName = Path.Combine(dependencyDirectory, "DependencyA.pas");
var consumerFileName = Path.Combine(dependencyDirectory, "DependencyB.pas");
var dependencyDocumentId = new Uri(dependencyFileName).AbsoluteUri;
var consumerDocumentId = new Uri(consumerFileName).AbsoluteUri;

const string dependencySourceV1 = """
unit DependencyA;

interface

type
  TDependency = class
    procedure Foo;
  end;

implementation

procedure TDependency.Foo;
begin
end;

end.
""";

const string dependencySourceV2 = """
unit DependencyA;

interface

type
  TDependency = class
    procedure Bar;
  end;

implementation

procedure TDependency.Bar;
begin
end;

end.
""";

const string dependencyBrokenSource = """
unit DependencyA;

interface

type
  TDependency = class
    procedure
  end;

implementation

procedure TDependency.Foo;
begin
  Self.Foo;
end;

end.
""";

const string dependencySourceV3 = """
unit DependencyA;

interface

type
  TDependency = class
    procedure Baz;
  end;

implementation

procedure TDependency.Baz;
begin
end;

end.
""";

const string consumerSource = """
program DependencyB;

uses DependencyA;

var value: TDependency;

begin
  value.Foo;
end.
""";

Directory.CreateDirectory(dependencyDirectory);
File.WriteAllText(dependencyFileName, dependencySourceV1);
File.WriteAllText(consumerFileName, consumerSource);

var dependencyV1 = await languageService.OpenOrUpdateDocumentAsync(
    dependencyDocumentId,
    dependencyFileName,
    dependencySourceV1,
    version: 1);
Check(dependencyV1.IsCompiled, "Dependency A version 1 compiled");
var dependencyConverterBeforeBrokenUpdate =
    CodeCompletionController.comp_modules[dependencyFileName] as DomConverter;
Check(
    dependencyConverterBeforeBrokenUpdate is not null,
    "Dependency A version 1 is stored in the global semantic cache");

var consumerAnalysis = await languageService.OpenOrUpdateDocumentAsync(
    consumerDocumentId,
    consumerFileName,
    consumerSource,
    version: 1);
Check(consumerAnalysis.IsCompiled, "Consumer B compiled against dependency A");

var dependencyMemberOffset = consumerSource.IndexOf("value.", StringComparison.Ordinal) + "value.".Length;
var completionBeforeDependencyChange = await languageService.GetCompletionAfterDotAsync(
    consumerDocumentId,
    dependencyMemberOffset);
Check(
    completionBeforeDependencyChange.Any(item => item.Label == "Foo"),
    "Consumer completion initially contains Foo from dependency A");

File.WriteAllText(dependencyFileName, dependencyBrokenSource);
var brokenDependency = await languageService.OpenOrUpdateDocumentAsync(
    dependencyDocumentId,
    dependencyFileName,
    dependencyBrokenSource,
    version: 2);
Check(!brokenDependency.IsCompiled, "Broken dependency update is reported as failed");
Check(
    languageService.Documents.TryGet(dependencyDocumentId, out var brokenDependencySnapshot) &&
    brokenDependencySnapshot?.Version == 2 &&
    brokenDependencySnapshot.Text == dependencyBrokenSource,
    "Broken dependency snapshot remains the latest editor version");

var completionAfterBrokenDependency = await languageService.GetCompletionAfterDotAsync(
    consumerDocumentId,
    dependencyMemberOffset);
Check(
    completionAfterBrokenDependency.Any(item => item.Label == "Foo"),
    "Consumer keeps the last successful dependency model while A is broken");
Check(
    ReferenceEquals(
        dependencyConverterBeforeBrokenUpdate,
        CodeCompletionController.comp_modules[dependencyFileName] as DomConverter),
    "Broken A keeps its own last successful semantic model");
Console.WriteLine("PASS last successful semantic model survives a broken dependency update");

File.WriteAllText(dependencyFileName, dependencySourceV2);
var dependencyV2 = await languageService.OpenOrUpdateDocumentAsync(
    dependencyDocumentId,
    dependencyFileName,
    dependencySourceV2,
    version: 3);
Check(dependencyV2.IsCompiled, "Dependency A version 2 compiled");

var completionAfterDependencyChange = await languageService.GetCompletionAfterDotAsync(
    consumerDocumentId,
    dependencyMemberOffset);
var completionAfterDependencyChangeLabels = string.Join(
    ", ",
    completionAfterDependencyChange.Select(item => item.Label));
Check(
    completionAfterDependencyChange.Any(item => item.Label == "Bar"),
    $"Consumer completion refreshes Bar after dependency A changes (symbols: {completionAfterDependencyChangeLabels})");
Check(
    completionAfterDependencyChange.All(item => item.Label != "Foo"),
    "Consumer completion drops stale Foo after dependency A changes");
Console.WriteLine("PASS dependency-aware semantic refresh");

File.WriteAllText(dependencyFileName, dependencyBrokenSource);
await languageService.QueueDocumentUpdateAsync(
    dependencyDocumentId,
    dependencyFileName,
    dependencyBrokenSource,
    version: 4);
File.WriteAllText(dependencyFileName, dependencySourceV3);
await languageService.QueueDocumentUpdateAsync(
    dependencyDocumentId,
    dependencyFileName,
    dependencySourceV3,
    version: 5);

var completionAfterBurstUpdate = await languageService.GetCompletionAfterDotAsync(
    consumerDocumentId,
    dependencyMemberOffset);
Check(
    completionAfterBurstUpdate.Any(item => item.Label == "Baz"),
    "Semantic request compiles the latest queued dependency version");
Check(
    completionAfterBurstUpdate.All(item => item.Label != "Foo" && item.Label != "Bar"),
    "Superseded queued dependency versions are not published");
Check(
    languageService.Documents.TryGet(dependencyDocumentId, out var burstDependencySnapshot) &&
    burstDependencySnapshot?.Version == 5,
    "Document storage keeps the newest queued version");
await languageService.QueueDocumentUpdateAsync(
    dependencyDocumentId,
    dependencyFileName,
    dependencyBrokenSource,
    version: 4);
Check(
    languageService.Documents.TryGet(dependencyDocumentId, out var snapshotAfterStaleUpdate) &&
    snapshotAfterStaleUpdate?.Version == 5 &&
    snapshotAfterStaleUpdate.Text == dependencySourceV3,
    "A stale queued version cannot roll back the latest document snapshot");
Console.WriteLine("PASS queued updates coalesce to the latest semantic version");

File.WriteAllText(dependencyFileName, dependencySourceV1);
Check((await languageService.OpenOrUpdateDocumentAsync(
    dependencyDocumentId,
    dependencyFileName,
    dependencySourceV2,
    version: 6)).IsCompiled, "Unsaved dependency version compiled before close");
var completionBeforeDependencyClose = await languageService.GetCompletionAfterDotAsync(
    consumerDocumentId,
    dependencyMemberOffset);
Check(
    completionBeforeDependencyClose.Any(item => item.Label == "Bar") &&
    completionBeforeDependencyClose.All(item => item.Label != "Foo"),
    "Open consumer uses the unsaved dependency model before close");

Check(
    await languageService.CloseDocumentAsync(dependencyDocumentId),
    "Closing the unsaved dependency removes its open document");
Check(
    !languageService.Documents.TryGet(dependencyDocumentId, out _),
    "Closed dependency is removed from document storage");
var completionAfterDependencyClose = await languageService.GetCompletionAfterDotAsync(
    consumerDocumentId,
    dependencyMemberOffset);
Check(
    completionAfterDependencyClose.Any(item => item.Label == "Foo"),
    "Open consumer returns to the dependency model stored on disk after close");
Check(
    completionAfterDependencyClose.All(item => item.Label != "Bar"),
    "Open consumer drops the unsaved dependency model after close");
Console.WriteLine("PASS closing an unsaved dependency restores its disk model");

var unsavedOnlyFileName = Path.Combine(
    dependencyDirectory,
    $"UnsavedOnly_{Guid.NewGuid():N}.pas");
var unsavedOnlyDocumentId = new Uri(unsavedOnlyFileName).AbsoluteUri;
const string unsavedOnlySource = """
program UnsavedOnly;
begin
end.
""";
Check((await languageService.OpenOrUpdateDocumentAsync(
    unsavedOnlyDocumentId,
    unsavedOnlyFileName,
    unsavedOnlySource,
    version: 1)).IsCompiled, "Unsaved-only document compiled");
Check(
    CodeCompletionController.comp_modules[unsavedOnlyFileName] is DomConverter,
    "Unsaved-only document is present in the global semantic cache while open");
Check(
    await languageService.CloseDocumentAsync(unsavedOnlyDocumentId),
    "Unsaved-only document closed");
Check(
    CodeCompletionController.comp_modules[unsavedOnlyFileName] is null,
    "Closing a document without a disk file removes its global semantic cache entry");
Console.WriteLine("PASS closing an unsaved-only document clears its semantic cache");

var chainAFileName = Path.Combine(dependencyDirectory, "ChainA.pas");
var chainBFileName = Path.Combine(dependencyDirectory, "ChainB.pas");
var chainCFileName = Path.Combine(dependencyDirectory, "ChainC.pas");

const string chainASourceV1 = """
unit ChainA;
interface
type
  TBase = class
    procedure OldMember;
  end;
implementation
procedure TBase.OldMember;
begin
end;
end.
""";

const string chainASourceV2 = """
unit ChainA;
interface
type
  TBase = class
    procedure NewMember;
  end;
implementation
procedure TBase.NewMember;
begin
end;
end.
""";

const string chainBSource = """
unit ChainB;
interface
uses ChainA;
type
  TDerived = class(TBase)
  end;
implementation
end.
""";

const string chainCSource = """
program ChainC;
uses ChainB;
var value: TDerived;
begin
  value.OldMember;
end.
""";

File.WriteAllText(chainAFileName, chainASourceV1);
File.WriteAllText(chainBFileName, chainBSource);
File.WriteAllText(chainCFileName, chainCSource);

var chainADocumentId = new Uri(chainAFileName).AbsoluteUri;
var chainBDocumentId = new Uri(chainBFileName).AbsoluteUri;
var chainCDocumentId = new Uri(chainCFileName).AbsoluteUri;

Check((await languageService.OpenOrUpdateDocumentAsync(
    chainADocumentId,
    chainAFileName,
    chainASourceV1,
    version: 1)).IsCompiled, "Transitive dependency A compiled");
Check((await languageService.OpenOrUpdateDocumentAsync(
    chainBDocumentId,
    chainBFileName,
    chainBSource,
    version: 1)).IsCompiled, "Transitive dependency B compiled");
Check((await languageService.OpenOrUpdateDocumentAsync(
    chainCDocumentId,
    chainCFileName,
    chainCSource,
    version: 1)).IsCompiled, "Transitive consumer C compiled");

var chainMemberOffset = chainCSource.IndexOf("value.", StringComparison.Ordinal) + "value.".Length;
Check(
    (await languageService.GetCompletionAfterDotAsync(chainCDocumentId, chainMemberOffset))
        .Any(item => item.Label == "OldMember"),
    "Transitive consumer initially contains OldMember");

File.WriteAllText(chainAFileName, chainASourceV2);
Check((await languageService.OpenOrUpdateDocumentAsync(
    chainADocumentId,
    chainAFileName,
    chainASourceV2,
    version: 2)).IsCompiled, "Transitive dependency A version 2 compiled");

var chainCompletionAfterChange = await languageService.GetCompletionAfterDotAsync(
    chainCDocumentId,
    chainMemberOffset);
Check(
    chainCompletionAfterChange.Any(item => item.Label == "NewMember"),
    "Transitive consumer refreshes NewMember after dependency A changes");
Check(
    chainCompletionAfterChange.All(item => item.Label != "OldMember"),
    "Transitive consumer drops stale OldMember after dependency A changes");
Console.WriteLine("PASS transitive dependency-aware semantic refresh");

var implementationAFileName = Path.Combine(dependencyDirectory, "ImplementationA.pas");
var implementationBFileName = Path.Combine(dependencyDirectory, "ImplementationB.pas");
var implementationCFileName = Path.Combine(dependencyDirectory, "ImplementationC.pas");
var implementationADocumentId = new Uri(implementationAFileName).AbsoluteUri;
var implementationBDocumentId = new Uri(implementationBFileName).AbsoluteUri;
var implementationCDocumentId = new Uri(implementationCFileName).AbsoluteUri;

const string implementationASourceV1 = """
unit ImplementationA;
interface
type TImplementationDependency = class procedure OldMember; end;
implementation
procedure TImplementationDependency.OldMember;
begin
end;
end.
""";

const string implementationASourceV2 = """
unit ImplementationA;
interface
type TImplementationDependency = class procedure NewMember; end;
implementation
procedure TImplementationDependency.NewMember;
begin
end;
end.
""";

const string implementationBSource = """
unit ImplementationB;
interface
procedure Touch;
implementation
uses ImplementationA;
procedure Touch;
var value: TImplementationDependency;
begin
  value.OldMember;
end;
end.
""";

const string implementationCSource = """
program ImplementationC;
uses ImplementationB;
begin
  Touch;
end.
""";

File.WriteAllText(implementationAFileName, implementationASourceV1);
File.WriteAllText(implementationBFileName, implementationBSource);
File.WriteAllText(implementationCFileName, implementationCSource);

Check((await languageService.OpenOrUpdateDocumentAsync(
    implementationADocumentId,
    implementationAFileName,
    implementationASourceV1,
    version: 1)).IsCompiled, "Implementation dependency A compiled");
Check((await languageService.OpenOrUpdateDocumentAsync(
    implementationBDocumentId,
    implementationBFileName,
    implementationBSource,
    version: 1)).IsCompiled, "Implementation-only consumer B compiled");
Check((await languageService.OpenOrUpdateDocumentAsync(
    implementationCDocumentId,
    implementationCFileName,
    implementationCSource,
    version: 1)).IsCompiled, "Transitive implementation consumer C compiled");

var implementationMemberOffset =
    implementationBSource.IndexOf("value.", StringComparison.Ordinal) + "value.".Length;
Check(
    (await languageService.GetCompletionAfterDotAsync(
        implementationBDocumentId,
        implementationMemberOffset)).Any(item => item.Label == "OldMember"),
    "Implementation-only consumer initially contains OldMember");
var implementationCConverterBefore =
    CodeCompletionController.comp_modules[implementationCFileName] as DomConverter;
Check(implementationCConverterBefore is not null, "Transitive implementation consumer has a semantic model");

File.WriteAllText(implementationAFileName, implementationASourceV2);
Check((await languageService.OpenOrUpdateDocumentAsync(
    implementationADocumentId,
    implementationAFileName,
    implementationASourceV2,
    version: 2)).IsCompiled, "Implementation dependency A version 2 compiled");

var implementationCompletionAfterChange = await languageService.GetCompletionAfterDotAsync(
    implementationBDocumentId,
    implementationMemberOffset);
Check(
    implementationCompletionAfterChange.Any(item => item.Label == "NewMember"),
    "Implementation-only consumer refreshes NewMember after dependency A changes");
Check(
    implementationCompletionAfterChange.All(item => item.Label != "OldMember"),
    "Implementation-only consumer drops stale OldMember after dependency A changes");
var implementationCConverterAfter =
    CodeCompletionController.comp_modules[implementationCFileName] as DomConverter;
Check(
    implementationCConverterAfter is not null &&
    !ReferenceEquals(implementationCConverterBefore, implementationCConverterAfter),
    "Implementation-only dependency refresh propagates transitively to C");
Console.WriteLine("PASS implementation uses dependency refresh");
Console.WriteLine("PASS transitive implementation uses dependency refresh");

var invalidAnalysis = await languageService.OpenOrUpdateDocumentAsync(
    "file:///Broken.pas",
    "Broken.pas",
    invalidSource,
    version: 1);
Check(!invalidAnalysis.IsCompiled, "LanguageServices preserves failed conversion state");
Check(await languageService.CloseDocumentAsync(documentId), "LanguageServices closes an open document");
Console.WriteLine("PASS serialized PascalABCNet.LanguageServices round-trip");

Console.WriteLine("All headless IntelliSense smoke checks passed.");

static (int Line, int Column) GetPosition(string text, int offset)
{
    var line = 0;
    var column = 0;

    for (var index = 0; index < offset; index++)
    {
        if (text[index] == '\n')
        {
            line++;
            column = 0;
        }
        else
        {
            column++;
        }
    }

    return (line, column);
}

static void Check(bool condition, string message)
{
    if (!condition)
        throw new InvalidOperationException($"Smoke check failed: {message}");
}
