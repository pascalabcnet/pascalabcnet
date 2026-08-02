// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
{$reference Compiler.dll}
{$reference Errors.dll}
{$reference CompilerTools.dll}
{$reference Localization.dll}
{$reference LanguageIntegrator.dll}

uses PascalABCCompiler, System.IO, System.Diagnostics;

type
  LanguageTestsInfo = auto class
    languageName: string;
    languageExtensions: array of string;
    commentSymbol: string;
  end;

const
  TestTimeoutMilliseconds = 60000;

var
  TestSuiteDir: string;
  TestWorkDir: string;
  ResultsRoot: string;
  OutputRoot: string;
  CoreOutputDir: string;
  UnitsOutputDir: string;
  UsesUnitsOutputDir: string;
  ErrorsOutputDir: string;
  TargetName: string;
  IsModernTarget: boolean;
  ModernExcludedTests := new System.Collections.Generic.List<string>;
  CurrentLanguageInfo: LanguageTestsInfo;
  TestFilter := '';
  PassedCount := 0;
  FailedCount := 0;
  SkippedCount := 0;
  ModernSkippedCount := 0;
  FailureMessages := new System.Collections.Generic.List<string>;
  ProgressColumn := 0;
  ProgressPending := 0;

procedure EmitProgressDot;
begin
  Write('.');
  Inc(ProgressColumn);
  if ProgressColumn = 80 then
  begin
    Println;
    ProgressColumn := 0;
  end;
end;

procedure FinishProgressLine;
begin
  if ProgressPending > 0 then
  begin
    EmitProgressDot;
    ProgressPending := 0;
  end;
  if ProgressColumn > 0 then
    Println;
  ProgressColumn := 0;
end;

procedure BeginSection(name: string);
begin
  FinishProgressLine;
  Println('----- ' + name + ' -----');
end;

procedure EndSection(startedAt: integer);
begin
  FinishProgressLine;
  Println('Время этапа: ' + System.TimeSpan.FromMilliseconds(Milliseconds - startedAt).ToString());
end;

procedure PrintSuccessDot;
begin
  Inc(ProgressPending);
  if ProgressPending = 10 then
  begin
    EmitProgressDot;
    ProgressPending := 0;
  end;
end;

function IsUnix: boolean;
begin
  Result := (System.Environment.OSVersion.Platform = System.PlatformID.Unix) or
            (System.Environment.OSVersion.Platform = System.PlatformID.MacOSX);
end;

function GetCurrentLanguageInfo(dir: string): LanguageTestsInfo;
begin
  var configDict := &File.ReadLines(Path.Combine(dir, 'testsettings.config'))
                           .Select(line -> line.Split([':', ' '], System.StringSplitOptions.RemoveEmptyEntries))
                           .ToDictionary(arr -> arr[0], arr -> arr[1]);

  var languageInformation := Languages.Facade.LanguageProvider.Instance
                                    .SelectLanguageByName(configDict['languageName'])
                                    .LanguageInformation;

  Result := new LanguageTestsInfo(languageInformation.Name,
                                  languageInformation.FilesExtensions,
                                  languageInformation.CommentSymbol);
end;

function GetFilesByExtensions(path: string; extensions: array of string): array of string;
begin
  if not Directory.Exists(path) then
  begin
    Result := new string[0];
    exit;
  end;

  Result := extensions.SelectMany(ext -> Directory.GetFiles(path, $'*{ext}'))
                      .OrderBy(fileName -> fileName)
                      .ToArray();
end;

function MatchesFilter(fileName: string): boolean;
begin
  Result := (TestFilter = '') or
            Path.GetFileName(fileName).ToLower().Contains(TestFilter.ToLower());
end;

function FirstLine(content: string): string;
begin
  var lines := content.Split([#13, #10], System.StringSplitOptions.RemoveEmptyEntries);
  Result := lines.Length = 0 ? '' : lines[0];
end;

function ShouldSkip(content: string): boolean;
begin
  var first := FirstLine(content);
  Result := first.StartsWith(CurrentLanguageInfo.commentSymbol + 'exclude') or
            (IsUnix and first.StartsWith(CurrentLanguageInfo.commentSymbol + 'winonly'));
end;

function ExpectedMessage(content: string): string;
begin
  var first := FirstLine(content);
  var prefix := CurrentLanguageInfo.commentSymbol + '!';
  Result := first.StartsWith(prefix) ? first.Substring(prefix.Length).Trim() : '';
end;

procedure RecordFailure(phase, fileName, message: string);
begin
  Inc(FailedCount);
  var displayName := string.IsNullOrEmpty(fileName) ? '<runner>' : Path.GetFileName(fileName);
  var text := $'[{phase}] {displayName}: {message}';
  FailureMessages.Add(text);
  FinishProgressLine;
  Println('FAIL ' + text);
end;

procedure RecordPassed;
begin
  Inc(PassedCount);
  PrintSuccessDot;
end;

procedure RecordSkipped;
begin
  Inc(SkippedCount);
end;

procedure RecordModernSkipped;
begin
  Inc(SkippedCount);
  Inc(ModernSkippedCount);
end;

function CompilerErrorsToString(comp: Compiler): string;
begin
  Result := '';
  for var i := 0 to comp.ErrorsList.Count - 1 do
  begin
    if Result <> '' then
      Result += System.Environment.NewLine;
    Result += comp.ErrorsList[i].ToString();
  end;
end;

procedure RecreateDirectory(dir: string);
begin
  if Directory.Exists(dir) then
    Directory.Delete(dir, true);
  Directory.CreateDirectory(dir);
end;

function RelativeWorkPath(fileName: string): string;
begin
  var prefix := TestWorkDir + Path.DirectorySeparatorChar;
  Result := fileName.StartsWith(prefix, System.StringComparison.OrdinalIgnoreCase) ?
            fileName.Substring(prefix.Length) : Path.GetFileName(fileName);
end;

function IsModernExcluded(fileName: string): boolean;
begin
  Result := false;
  if not IsModernTarget then
    exit;

  var relativeName := RelativeWorkPath(fileName).Replace(Path.AltDirectorySeparatorChar,
                                                         Path.DirectorySeparatorChar);
  Result := ModernExcludedTests.Contains(relativeName.ToLower());
end;

procedure LoadModernExclusions;
begin
  var exclusionsFile := Path.Combine(TestSuiteDir, 'modern-excluded.txt');
  if not &File.Exists(exclusionsFile) then
    exit;

  foreach var line in &File.ReadAllLines(exclusionsFile) do
  begin
    var value := line.Trim();
    if (value = '') or value.StartsWith('#') then
      continue;
    ModernExcludedTests.Add(value.Replace(Path.AltDirectorySeparatorChar,
                                          Path.DirectorySeparatorChar).ToLower());
  end;
end;

function ShouldCopyToWork(fileName: string): boolean;
begin
  var extension := Path.GetExtension(fileName).ToLower();
  Result := (extension <> '.pcu') and (extension <> '.exe') and (extension <> '.pdb');
  if IsModernTarget and (extension = '.dll') then
    Result := false;
end;

procedure CopyDirectoryToWork(sourceDir, targetDir: string);
begin
  Directory.CreateDirectory(targetDir);
  foreach var fileName in Directory.GetFiles(sourceDir) do
    if ShouldCopyToWork(fileName) then
      &File.Copy(fileName, Path.Combine(targetDir, Path.GetFileName(fileName)), true);
end;

procedure CopyDirectoryTreeToWork(sourceDir, targetDir: string);
begin
  CopyDirectoryToWork(sourceDir, targetDir);
  foreach var childDir in Directory.GetDirectories(sourceDir) do
    CopyDirectoryTreeToWork(childDir,
                            Path.Combine(targetDir, Path.GetFileName(childDir)));
end;

procedure PrepareTargetDirectories;
begin
  RecreateDirectory(ResultsRoot);
  Directory.CreateDirectory(TestWorkDir);
  Directory.CreateDirectory(OutputRoot);

  CopyDirectoryToWork(TestSuiteDir, TestWorkDir);
  CopyDirectoryTreeToWork(Path.Combine(TestSuiteDir, 'namespaces'),
                          Path.Combine(TestWorkDir, 'namespaces'));
  CopyDirectoryTreeToWork(Path.Combine(TestSuiteDir, 'units'),
                          Path.Combine(TestWorkDir, 'units'));
  CopyDirectoryTreeToWork(Path.Combine(TestSuiteDir, 'usesunits'),
                          Path.Combine(TestWorkDir, 'usesunits'));
  CopyDirectoryTreeToWork(Path.Combine(TestSuiteDir, 'errors'),
                          Path.Combine(TestWorkDir, 'errors'));
end;

procedure CopyRuntimeDependencies(outputDir: string);
begin
  foreach var dllName in Directory.GetFiles(TestWorkDir, '*.dll') do
    &File.Copy(dllName, Path.Combine(outputDir, Path.GetFileName(dllName)), true);
end;

function CompileSource(var comp: Compiler;
                       fileName, outputDir, phase: string;
                       validateWarning: boolean;
                       searchDirectory: string := ''): boolean;
begin
  Result := false;
  try
    var content := &File.ReadAllText(fileName);
    var options := new CompilerOptions(fileName, CompilerOptions.OutputType.ConsoleApplicaton);
    options.Debug := true;
    options.OutputDirectory := outputDir;
    options.UseDllForSystemUnits := false;
    options.RunWithEnvironment := false;
    options.IgnoreRtlErrors := false;
    if searchDirectory <> '' then
      options.SearchDirectories.Add(searchDirectory);

    comp.ErrorsList.Clear();
    comp.Warnings.Clear();
    comp.Compile(options);
    if comp.ErrorsList.Count > 0 then
    begin
      RecordFailure(phase, fileName, CompilerErrorsToString(comp));
      exit;
    end;

    if validateWarning then
    begin
      var expectedWarning := ExpectedMessage(content);
      if expectedWarning <> '' then
      begin
        if comp.Warnings.Count = 0 then
        begin
          RecordFailure(phase, fileName, 'Ожидалось предупреждение: ' + expectedWarning);
          exit;
        end;
        if comp.Warnings[0].Message.Trim() <> expectedWarning then
        begin
          RecordFailure(phase, fileName,
                        'Неверное предупреждение. Ожидалось: ' + expectedWarning +
                        '. Получено: ' + comp.Warnings[0].Message.Trim());
          exit;
        end;
      end;
    end;

    Result := true;
  except
    on e: Exception do
    begin
      RecordFailure(phase, fileName, e.ToString());
      comp := new Compiler();
    end;
  end;
end;

function RunProgram(exeName, sourceFileName: string): boolean;
begin
  Result := false;
  try
    if not &File.Exists(exeName) then
    begin
      RecordFailure('run', sourceFileName, 'Исполняемый файл не создан: ' + exeName);
      exit;
    end;

    var startInfo := new ProcessStartInfo(IsModernTarget ? 'dotnet' : exeName);
    if IsModernTarget then
      startInfo.Arguments := '"' + exeName + '"';
    startInfo.CreateNoWindow := true;
    startInfo.UseShellExecute := false;
    startInfo.RedirectStandardError := IsModernTarget;
    startInfo.WorkingDirectory := Path.GetDirectoryName(exeName);

    var process := new Process();
    process.StartInfo := startInfo;
    process.Start();

    if not process.WaitForExit(TestTimeoutMilliseconds) then
    begin
      try
        process.Kill();
        process.WaitForExit();
      except
      end;
      RecordFailure('run', sourceFileName,
                    $'Превышен таймаут {TestTimeoutMilliseconds div 1000} секунд');
      exit;
    end;

    var errorOutput := '';
    if IsModernTarget then
      errorOutput := process.StandardError.ReadToEnd().Trim();

    if process.ExitCode <> 0 then
    begin
      var details := 'Код завершения: ' + process.ExitCode.ToString();
      if errorOutput <> '' then
        details += NewLine + errorOutput;
      RecordFailure('run', sourceFileName,
                    details);
      exit;
    end;

    Result := true;
  except
    on e: Exception do
      RecordFailure('run', sourceFileName, e.ToString());
  end;
end;

function CollectGeneratedPcu(sourceFileName, outputDir, phase: string): boolean; forward;

procedure RunCoreTests;
begin
  RecreateDirectory(CoreOutputDir);

  var executables := new System.Collections.Generic.List<string>;
  var sourceFiles := new System.Collections.Generic.List<string>;
  var files := GetFilesByExtensions(TestWorkDir, CurrentLanguageInfo.languageExtensions);
  var comp := new Compiler();

  BeginSection('core: compile');
  var compileStarted := Milliseconds;

  foreach var fileName in files do
  begin
    if not MatchesFilter(fileName) then
      continue;

    if IsModernExcluded(fileName) then
    begin
      RecordModernSkipped;
      continue;
    end;

    var content := &File.ReadAllText(fileName);
    if ShouldSkip(content) then
    begin
      RecordSkipped;
      continue;
    end;

    if CompileSource(comp, fileName, CoreOutputDir, 'compile', true) then
    begin
      var baseName := Path.GetFileNameWithoutExtension(fileName);
      var exeName := Path.Combine(CoreOutputDir, baseName + '.exe');
      var dllName := Path.Combine(CoreOutputDir, baseName + '.dll');
      var sourcePcu := Path.ChangeExtension(fileName, '.pcu');

      if &File.Exists(exeName) then
      begin
        executables.Add(exeName);
        sourceFiles.Add(fileName);
        PrintSuccessDot;
      end
      else if &File.Exists(sourcePcu) then
      begin
        if CollectGeneratedPcu(fileName, CoreOutputDir, 'compile') then
          RecordPassed;
      end
      else if &File.Exists(dllName) then
        RecordPassed
      else
        RecordFailure('compile', fileName, 'Компилятор не создал EXE, DLL или PCU');
    end;
  end;

  CopyRuntimeDependencies(CoreOutputDir);
  EndSection(compileStarted);

  BeginSection('core: run');
  var runStarted := Milliseconds;
  for var i := 0 to executables.Count - 1 do
  begin
    if RunProgram(executables[i], sourceFiles[i]) then
      RecordPassed;
  end;
  EndSection(runStarted);
end;

function CollectGeneratedPcu(sourceFileName, outputDir, phase: string): boolean;
begin
  Result := false;
  try
    var sourcePcu := Path.ChangeExtension(sourceFileName, '.pcu');
    if not &File.Exists(sourcePcu) then
    begin
      RecordFailure(phase, sourceFileName, 'Компилятор не создал PCU');
      exit;
    end;

    Result := true;
  except
    on e: Exception do
      RecordFailure(phase, sourceFileName, 'Не удалось проверить PCU: ' + e.ToString());
  end;
end;

procedure CompileDirectory(sourceDir, outputDir, phase: string;
                           searchDirectory: string := '';
                           collectPcu: boolean := false);
begin
  var files := GetFilesByExtensions(sourceDir, CurrentLanguageInfo.languageExtensions);
  var comp := new Compiler();
  foreach var fileName in files do
  begin
    if not MatchesFilter(fileName) then
      continue;

    if IsModernExcluded(fileName) then
    begin
      RecordModernSkipped;
      continue;
    end;

    var content := &File.ReadAllText(fileName);
    if ShouldSkip(content) then
    begin
      RecordSkipped;
      continue;
    end;

    if CompileSource(comp, fileName, outputDir, phase, false, searchDirectory) then
    begin
      if (not collectPcu) or CollectGeneratedPcu(fileName, outputDir, phase) then
        RecordPassed;
    end;
  end;
end;

procedure RunUnitTests;
begin
  RecreateDirectory(UnitsOutputDir);
  RecreateDirectory(UsesUnitsOutputDir);

  BeginSection('units: compile');
  var unitsStarted := Milliseconds;
  var workUnitsDir := Path.Combine(TestWorkDir, 'units');
  CompileDirectory(workUnitsDir, UnitsOutputDir, 'units', '', true);
  EndSection(unitsStarted);

  BeginSection('usesunits: compile');
  var usesUnitsStarted := Milliseconds;
  CompileDirectory(Path.Combine(TestWorkDir, 'usesunits'), UsesUnitsOutputDir,
                   'usesunits', workUnitsDir);
  EndSection(usesUnitsStarted);
end;

procedure RunErrorTests;
begin
  RecreateDirectory(ErrorsOutputDir);

  var sourceDir := Path.Combine(TestWorkDir, 'errors');
  var files := GetFilesByExtensions(sourceDir, CurrentLanguageInfo.languageExtensions);

  BeginSection('errors');
  var errorsStarted := Milliseconds;

  foreach var fileName in files do
  begin
    if not MatchesFilter(fileName) then
      continue;

    if IsModernExcluded(fileName) then
    begin
      RecordModernSkipped;
      continue;
    end;

    var content := &File.ReadAllText(fileName);
    if ShouldSkip(content) then
    begin
      RecordSkipped;
      continue;
    end;

    try
      var comp := new Compiler();
      var options := new CompilerOptions(fileName, CompilerOptions.OutputType.ConsoleApplicaton);
      options.Debug := true;
      options.OutputDirectory := ErrorsOutputDir;
      options.UseDllForSystemUnits := false;
      options.RunWithEnvironment := false;

      comp.Compile(options);
      if comp.ErrorsList.Count = 0 then
      begin
        RecordFailure('errors', fileName, 'Компиляция ошибочного примера завершилась успешно');
        continue;
      end;

      var hasInternalError := false;
      for var i := 0 to comp.ErrorsList.Count - 1 do
        if comp.ErrorsList[i].GetType() = typeof(PascalABCCompiler.Errors.CompilerInternalError) then
          hasInternalError := true;

      if hasInternalError then
      begin
        RecordFailure('errors', fileName, CompilerErrorsToString(comp));
        continue;
      end;

      var expectedError := ExpectedMessage(content);
      var expectedErrorFound := expectedError = '';
      if not expectedErrorFound then
        for var i := 0 to comp.ErrorsList.Count - 1 do
          if comp.ErrorsList[i].Message.Trim() = expectedError then
            expectedErrorFound := true;

      if not expectedErrorFound then
      begin
        RecordFailure('errors', fileName,
                      'Неверный текст ошибки. Ожидалось: ' + expectedError +
                      System.Environment.NewLine + 'Все ошибки:' + System.Environment.NewLine +
                      CompilerErrorsToString(comp));
        continue;
      end;

      RecordPassed;
    except
      on e: Exception do
        RecordFailure('errors', fileName, e.ToString());
    end;
  end;
  EndSection(errorsStarted);
end;

procedure PrintUsage;
begin
  Println('TestRunner [all|core|units|errors] [часть имени файла]');
  Println('Старые номера проходов временно поддерживаются: 1 = core, 3 = units+errors.');
end;

procedure PrintSummary(elapsedMilliseconds: integer);
begin
  FinishProgressLine;
  Println;
  Println('========== ИТОГ ==========');
  Println('Пройдено:  ' + PassedCount);
  Println('Ошибок:    ' + FailedCount);
  Println('Пропущено: ' + SkippedCount);
  if ModernSkippedCount > 0 then
    Println('  из них modern-исключений: ' + ModernSkippedCount);
  Println('Время:     ' + System.TimeSpan.FromMilliseconds(elapsedMilliseconds).ToString());

  if FailureMessages.Count > 0 then
  begin
    Println;
    Println('Список ошибок:');
    for var i := 0 to FailureMessages.Count - 1 do
      Println($'{i + 1}. {FailureMessages[i]}');
  end;
end;

begin
  var started := Milliseconds;
  try
    TestSuiteDir := Path.GetFullPath(System.Environment.CurrentDirectory);
    var runnerDir := Path.GetDirectoryName(GetEXEFileName());
    IsModernTarget := Path.GetFileName(runnerDir).ToLower() = 'bin-net10';
    TargetName := IsModernTarget ? 'net10' : 'net40';

    ResultsRoot := Path.Combine(TestSuiteDir, 'TestResults', TargetName);
    TestWorkDir := Path.Combine(ResultsRoot, 'work');
    OutputRoot := Path.Combine(ResultsRoot, 'output');
    CoreOutputDir := Path.Combine(OutputRoot, 'core');
    UnitsOutputDir := Path.Combine(OutputRoot, 'units');
    UsesUnitsOutputDir := Path.Combine(OutputRoot, 'usesunits');
    ErrorsOutputDir := Path.Combine(OutputRoot, 'errors');

    if not &File.Exists(Path.Combine(TestSuiteDir, 'testsettings.config')) then
      raise new Exception('TestRunner необходимо запускать из каталога TestSuite');

    Languages.Integration.LanguageIntegrator.LoadAllLanguages();
    System.Environment.CurrentDirectory := runnerDir;
    CurrentLanguageInfo := GetCurrentLanguageInfo(TestSuiteDir);
    LoadModernExclusions;
    PrepareTargetDirectories;

    var mode := ParamCount = 0 ? 'all' : ParamStr(1).ToLower();
    var legacyInvocation := (mode = '1') or (mode = '2') or (mode = '3') or
                            (mode = '4') or (mode = '5') or (mode = '6');

    if mode = '1' then
      mode := 'core'
    else if mode = '3' then
      mode := 'units-errors'
    else if legacyInvocation then
      mode := 'legacy-skip';

    if (ParamCount > 1) and not legacyInvocation then
      TestFilter := ParamStr(2);

    if (mode <> 'all') and (mode <> 'core') and
       (mode <> 'units') and (mode <> 'errors') and
       (mode <> 'units-errors') and (mode <> 'legacy-skip') then
    begin
      PrintUsage;
      RecordFailure('runner', '', 'Неизвестный режим: ' + mode);
    end
    else if mode = 'legacy-skip' then
    begin
      Println('Этот старый проход исключён из сокращённого консольного TestRunner.');
      RecordSkipped;
    end
    else
    begin
      if (mode = 'all') or (mode = 'core') then
        RunCoreTests;
      if (mode = 'all') or (mode = 'units') or (mode = 'units-errors') then
        RunUnitTests;
      if (mode = 'all') or (mode = 'errors') or (mode = 'units-errors') then
        RunErrorTests;
    end;
  except
    on e: Exception do
      RecordFailure('runner', '', e.ToString());
  end;

  PrintSummary(Milliseconds - started);
  if FailedCount > 0 then
    Halt(1);
end.
