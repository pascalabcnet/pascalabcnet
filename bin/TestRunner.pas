// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
{$reference Compiler.dll}
{$reference CodeCompletion.dll}
{$reference Errors.dll}
{$reference CompilerTools.dll}
{$reference Localization.dll}
{$reference System.Windows.Forms.dll}
{$reference LanguageIntegrator.dll}

uses PascalABCCompiler, System.IO, System.Diagnostics;

type
  LanguageTestsInfo = auto class
 
    languageName: string;
    languageExtensions: array of string;
    commentSymbol: string;
end;

var
  TestSuiteDir: string;
  CurrentLanguageInfo: LanguageTestsInfo;
  nogui: boolean;

var
  PathSeparator: string := Path.DirectorySeparatorChar;

function IsUnix: boolean;
begin
  Result := (System.Environment.OSVersion.Platform = System.PlatformID.Unix) or (System.Environment.OSVersion.Platform = System.PlatformID.MacOSX);  
end;

function GetCurrentLanguageInfo(dir: string): LanguageTestsInfo;
begin
  var configDict := &File.ReadLines(dir + PathSeparator + 'testsettings.config')
                           .Select(line -> line.Split([':', ' '], System.StringSplitOptions.RemoveEmptyEntries))
                           .ToDictionary(arr -> arr[0], arr -> arr[1]);
    
  var languageInformation := Languages.Facade.LanguageProvider.Instance.SelectLanguageByName(configDict['languageName']).LanguageInformation;
    
  Result := new LanguageTestsInfo(languageInformation.Name, languageInformation.FilesExtensions, languageInformation.CommentSymbol);
end;

function GetCurrentTestSuiteDir(): string;
begin
  Result := System.Environment.CurrentDirectory;
end;

function GetLibDir: string;
begin
  var dir := Path.GetDirectoryName(GetEXEFileName());
  Result := dir + PathSeparator + 'Lib';
end;

function GetFilesByExtensions(path: string; extensions: array of string; searchOption: SearchOption := System.IO.SearchOption.TopDirectoryOnly): array of string;
begin
  Result := extensions.SelectMany(ext -> Directory.GetFiles(path, $'*{ext}', searchOption)).ToArray();
end;

function GetCodeExamplesDir: string;
begin
  var dir := Path.GetDirectoryName(GetEXEFileName());
  Result := (new DirectoryInfo(dir)).Parent.FullName + PathSeparator + 'CodeExamples' + PathSeparator + CurrentLanguageInfo.languageName;
end;

procedure CompileErrorTests(withide: boolean);
begin
  
  var dir := TestSuiteDir + PathSeparator + 'errors';
  if not Directory.Exists(dir) then exit;
  
  var commentSymbol := CurrentLanguageInfo.commentSymbol;
  var files := GetFilesByExtensions(dir, CurrentLanguageInfo.languageExtensions);
  for var i := 0 to files.Length - 1 do
  begin
    var comp := new Compiler();
    var content := &File.ReadAllText(files[i]);
    if content.StartsWith(commentSymbol + 'winonly') and IsUnix then
      continue;
    if content.StartsWith(commentSymbol + 'exclude') then
      continue;
    var errorMessage := '';
    if content.StartsWith(commentSymbol + '!') then
    begin
      errorMessage := content.Substring(commentSymbol.Length + 1, content.IndexOf(System.Environment.NewLine) - commentSymbol.Length + 1).Trim;
    end;
    var co: CompilerOptions := new CompilerOptions(files[i], CompilerOptions.OutputType.ConsoleApplicaton);
    co.Debug := true;
    co.OutputDirectory := TestSuiteDir + PathSeparator + 'errors';
    co.UseDllForSystemUnits := false;
    co.RunWithEnvironment := withide;
    comp.ErrorsList.Clear();
    comp.Warnings.Clear();
    
    comp.Compile(co);
    if comp.ErrorsList.Count = 0 then
    begin
      if nogui then
        raise new Exception('Compilation of error sample ' + files[i] + ' was successfull');
      System.Windows.Forms.MessageBox.Show('Compilation of error sample ' + files[i] + ' was successfull' + System.Environment.NewLine);
      Halt();
    end
    else if comp.ErrorsList.Count = 1 then
    begin
      if comp.ErrorsList[0].GetType() = typeof(PascalABCCompiler.Errors.CompilerInternalError) then
      begin
        if nogui then
          raise new Exception('Compilation of ' + files[i] + ' failed' + System.Environment.NewLine + comp.ErrorsList[0].ToString());
        System.Windows.Forms.MessageBox.Show('Compilation of ' + files[i] + ' failed' + System.Environment.NewLine + comp.ErrorsList[0].ToString());
      end;
      if (errorMessage <> '') and (comp.ErrorsList[comp.ErrorsList.Count - 1].Message.Trim <> errorMessage) then
      begin
        if nogui then
          raise new Exception('Wrong error message in file ' + files[i] + ', should ' + errorMessage + ', is ' + comp.ErrorsList[comp.ErrorsList.Count - 1].Message);
        System.Windows.Forms.MessageBox.Show('Wrong error message in file ' + files[i] + ', should ' + errorMessage + ', is ' + comp.ErrorsList[comp.ErrorsList.Count - 1].Message);
      end;
    end;
    if i mod 50 = 0 then
      System.GC.Collect();
  end;
  
end;

procedure CompileAllRunTests(withdll: boolean; only32bit: boolean := false);
begin
  
  var comp := new Compiler();
  
  var commentSymbol := CurrentLanguageInfo.commentSymbol;
  var files := GetFilesByExtensions(TestSuiteDir, CurrentLanguageInfo.languageExtensions);

  for var i := 0 to files.Length - 1 do
  begin
    if IsUnix then
      Println('Compile file ' + files[i]);
    var content := &File.ReadAllText(files[i]);
    if content.StartsWith(commentSymbol + 'winonly') and IsUnix then
      continue;
    if content.StartsWith(commentSymbol + 'nopabcrtl') and withdll then
      continue;
    var co: CompilerOptions := new CompilerOptions(files[i], CompilerOptions.OutputType.ConsoleApplicaton);
    co.Debug := true;
    co.OutputDirectory := TestSuiteDir + PathSeparator + 'exe';
    Directory.CreateDirectory(co.OutputDirectory);
    co.UseDllForSystemUnits := withdll;
    co.RunWithEnvironment := false;
    co.IgnoreRtlErrors := false;
    co.Only32Bit := only32bit;
    comp.ErrorsList.Clear();
    comp.Warnings.Clear();
    
    comp.Compile(co);
    if comp.ErrorsList.Count > 0 then
    begin
      if nogui then
        raise new Exception('Compilation of ' + files[i] + ' failed' + System.Environment.NewLine + comp.ErrorsList[0].ToString());
      System.Windows.Forms.MessageBox.Show('Compilation of ' + files[i] + ' failed' + System.Environment.NewLine + comp.ErrorsList[0].ToString());
      Halt();
    end;
    if content.StartsWith(commentSymbol + '!') then
    begin
      var warning := content.Substring(commentSymbol.Length + 1, content.IndexOf(System.Environment.NewLine) - commentSymbol.Length + 1).Trim;
      if comp.Warnings.Count = 0 then
      begin
        if nogui then
          raise new Exception('Missing warning in ' + files[i]);
        System.Windows.Forms.MessageBox.Show('Missing warning in ' + files[i]);
        Halt();
      end;
      if comp.Warnings[0].Message <> warning then
      begin
        if nogui then
          raise new Exception('Wrong warning for ' + files[i] + ': ' + comp.Warnings[0].Message);
        System.Windows.Forms.MessageBox.Show('Wrong warning for ' + files[i] + ': ' + comp.Warnings[0].Message);
        Halt();
      end;
    end;
    if i mod 50 = 0 then
    begin
      System.GC.Collect();
    end;  
  end;
  //Println;
end;

procedure CompileAllCompilationTests(dir: string; withdll: boolean);
begin
  
  var comp := new Compiler();
  
  var fullDirName := TestSuiteDir + PathSeparator + dir;
  if not Directory.Exists(fullDirName) then exit;
  
  var commentSymbol := CurrentLanguageInfo.commentSymbol;
  var files := GetFilesByExtensions(fullDirName, CurrentLanguageInfo.languageExtensions);
  for var i := 0 to files.Length - 1 do
  begin
    var content := &File.ReadAllText(files[i]);
    if content.StartsWith(commentSymbol + 'winonly') and IsUnix then
      continue;
    var co: CompilerOptions := new CompilerOptions(files[i], CompilerOptions.OutputType.ConsoleApplicaton);
    co.Debug := true;
    co.OutputDirectory := TestSuiteDir + PathSeparator + dir;
    co.UseDllForSystemUnits := withdll;
    co.RunWithEnvironment := false;
    co.IgnoreRtlErrors := false;
    comp.ErrorsList.Clear();
    comp.Warnings.Clear();
    
    comp.Compile(co);
    if comp.ErrorsList.Count > 0 then
    begin
      if nogui then
        raise new Exception('Compilation of ' + files[i] + ' failed' + System.Environment.NewLine + comp.ErrorsList[0].ToString());
      System.Windows.Forms.MessageBox.Show('Compilation of ' + files[i] + ' failed' + System.Environment.NewLine + comp.ErrorsList[0].ToString());
      Halt();
    end;
    if i mod 50 = 0 then
      System.GC.Collect();
  end;
  
end;

procedure CompileAllUnits;
begin
  var comp := new Compiler();
  // Не пропускать ошибки сохранения PCU, в тесте создания PCU
  comp.InternalDebug.SkipPCUErrors := false;
  
  var dir := TestSuiteDir + PathSeparator + 'units' + PathSeparator;
  if not Directory.Exists(dir) then exit;
  
  var commentSymbol := CurrentLanguageInfo.commentSymbol;
  var files := GetFilesByExtensions(TestSuiteDir + PathSeparator + 'units', CurrentLanguageInfo.languageExtensions);
  for var i := 0 to files.Length - 1 do
  begin
    var content := &File.ReadAllText(files[i]);
    if content.StartsWith(commentSymbol + 'winonly') and IsUnix then
      continue;
    var co: CompilerOptions := new CompilerOptions(files[i], CompilerOptions.OutputType.ConsoleApplicaton);
    co.Debug := true;
    co.OutputDirectory := dir;
    co.UseDllForSystemUnits := false;
    comp.ErrorsList.Clear();
    comp.Warnings.Clear();
    
    comp.Compile(co);
    if comp.ErrorsList.Count > 0 then
    begin
      if nogui then
        raise new Exception('Compilation of ' + files[i] + ' failed' + System.Environment.NewLine + comp.ErrorsList[0].ToString());
      System.Windows.Forms.MessageBox.Show('Compilation of ' + files[i] + ' failed' + System.Environment.NewLine + comp.ErrorsList[0].ToString());
      Halt();
    end;
  end;
  System.GC.Collect;
end;

procedure CompileAllUsesUnits;
begin
  var comp := new Compiler();
  
  var dir := TestSuiteDir + PathSeparator + 'usesunits';
  
  if not Directory.Exists(dir) then exit;
  
  var commentSymbol := CurrentLanguageInfo.commentSymbol;
  var files := GetFilesByExtensions(dir, CurrentLanguageInfo.languageExtensions);
  for var i := 0 to files.Length - 1 do
  begin
    var content := &File.ReadAllText(files[i]);
    if content.StartsWith(commentSymbol + 'winonly') and IsUnix then
      continue;
    var co: CompilerOptions := new CompilerOptions(files[i], CompilerOptions.OutputType.ConsoleApplicaton);
    co.Debug := true;
    co.OutputDirectory := TestSuiteDir + PathSeparator + 'exe';
    co.UseDllForSystemUnits := false;
    comp.ErrorsList.Clear();
    comp.Warnings.Clear();
    comp.Compile(co);
    if comp.ErrorsList.Count > 0 then
    begin
      if nogui then
        raise new Exception('Compilation of ' + files[i] + ' failed' + System.Environment.NewLine + comp.ErrorsList[0].ToString());
      System.Windows.Forms.MessageBox.Show('Compilation of ' + files[i] + ' failed' + System.Environment.NewLine + comp.ErrorsList[0].ToString());
      Halt();
    end;
  end;
  System.GC.Collect;
end;

procedure CopyPCUFiles;
begin  
  var dir := TestSuiteDir + PathSeparator + 'units';
  if not Directory.Exists(dir) then exit;
  
  var files := Directory.GetFiles(dir, '*.pcu');
  
  foreach fname: string in files do
  begin
    &File.Move(fname, TestSuiteDir + PathSeparator + 'usesunits' + PathSeparator + Path.GetFileName(fname));
  end;
end;

procedure RunAllTests(redirectIO: boolean);
begin
  var dlls := Directory.GetFiles(TestSuiteDir, '*.dll');
  foreach var dll in dlls do
  begin
    System.IO.File.Copy(dll, TestSuiteDir + PathSeparator + 'exe' + PathSeparator + Path.GetFileName(dll), true);
  end;
  
  var dir := TestSuiteDir + PathSeparator + 'exe';
  if not Directory.Exists(dir) then exit;
  
  var files := Directory.GetFiles(dir, '*.exe');
  for var i := 0 to files.Length - 1 do
  begin
    //Println(files[i]);
    var psi := new System.Diagnostics.ProcessStartInfo(files[i]);
    psi.CreateNoWindow := true;
    psi.UseShellExecute := false;
    
    psi.WorkingDirectory := TestSuiteDir + PathSeparator + 'exe';
		  {psi.RedirectStandardInput := true;
		  psi.RedirectStandardOutput := true;
		  psi.RedirectStandardError := true;}
    var p: Process := new Process();
    p.StartInfo := psi;
    p.Start();
    if redirectIO then
      p.StandardInput.WriteLine('GO');
		  //p.StandardInput.AutoFlush := true;
		  //var p := System.Diagnostics.Process.Start(psi);
    if nogui then
    begin
      var success := p.WaitForExit(60000);
      if not success then
      begin
        raise new Exception('Running of ' + files[i] + ' failed.');
      end;
    end
    else
      p.WaitForExit();
    //while not p.HasExited do
    //  Sleep(5);
    if p.ExitCode <> 0 then
    begin
      if nogui then
        raise new Exception('Running of ' + files[i] + ' failed. Exit code is not 0');
      System.Windows.Forms.MessageBox.Show('Running of ' + files[i] + ' failed. Exit code is not 0');
      Halt;
    end;
  end;
end;

procedure RunExpressionsExtractTests;
begin
  // Пока для других языков не поддерживается
  if CurrentLanguageInfo.languageName <> 'PascalABC.NET' then exit;
  CodeCompletion.CodeCompletionTester.Test();
end;

function GetLineByPos(lines: array of string; pos: integer): integer;
begin
  var cum_pos := 0;
  for var i := 0 to lines.Length - 1 do
    for var j := 0 to lines[j].Length - 1 do
    begin
      if cum_pos = pos then
      begin
        Result := i + 1;
        exit;
      end;
      Inc(cum_pos);  
    end;
end;

function GetColByPos(lines: array of string; pos: integer): integer;
begin
  var cum_pos := 0;
  for var i := 0 to lines.Length - 1 do
    for var j := 0 to lines[j].Length - 1 do
    begin
      if cum_pos = pos then
      begin
        Result := j + 1;
        exit;
      end;
      Inc(cum_pos);  
    end;
end;

procedure RunIntellisenseTests;
begin
  var dir := TestSuiteDir + PathSeparator + 'intellisense_tests';
  if not Directory.Exists(dir) then exit;
  
  PascalABCCompiler.StringResourcesLanguage.CurrentTwoLetterISO := 'ru';
  CodeCompletion.CodeCompletionTester.TestIntellisense(dir);
end;

procedure RunFormatterTests;
begin
  var dir := TestSuiteDir + PathSeparator + 'formatter_tests';
  if not Directory.Exists(dir) then exit;
  
  CodeCompletion.FormatterTester.Test();
  var errors := &File.ReadAllText(dir + PathSeparator + 'output' + PathSeparator + 'log.txt');
  if not string.IsNullOrEmpty(errors) then
  begin
    var dirInfo := new DirectoryInfo(TestSuiteDir);
    
    System.Windows.Forms.MessageBox.Show(errors + System.Environment.NewLine + $'more info at {dirInfo.Parent.Name}/{dirInfo.Name}/formatter_tests/output/log.txt');
    Halt;
  end;
end;

procedure ClearDirByPattern(dir, pattern: string);
begin
  if not Directory.Exists(dir) then exit;
  var files := Directory.GetFiles(dir, pattern);
  for var i := 0 to files.Length - 1 do
  begin
    try
      if Path.GetFileName(files[i]) <> '.gitignore' then
        &File.Delete(files[i]);
    except
    end;
  end;
end;

procedure ClearExeDir;
begin
  ClearDirByPattern(TestSuiteDir + PathSeparator + 'exe', '*.*');
  ClearDirByPattern(TestSuiteDir + PathSeparator + 'CompilationSamples', '*.exe');
  ClearDirByPattern(TestSuiteDir + PathSeparator + 'CompilationSamples', '*.mdb');
  ClearDirByPattern(TestSuiteDir + PathSeparator + 'CompilationSamples', '*.pdb');
  ClearDirByPattern(TestSuiteDir + PathSeparator + 'CompilationSamples', '*.pcu');
  ClearDirByPattern(TestSuiteDir + PathSeparator + 'pabcrtl_tests', '*.exe');
  ClearDirByPattern(TestSuiteDir + PathSeparator + 'pabcrtl_tests', '*.pdb');
  ClearDirByPattern(TestSuiteDir + PathSeparator + 'pabcrtl_tests', '*.mdb');
  ClearDirByPattern(TestSuiteDir + PathSeparator + 'pabcrtl_tests', '*.pcu');
end;

procedure DeletePCUFiles;
begin
  ClearDirByPattern(TestSuiteDir + PathSeparator + 'usesunits', '*.pcu');
end;

//procedure DeletePABCSystemPCU;
//begin
//  var dir := Path.Combine(Path.GetDirectoryName(GetEXEFileName()), 'Lib');
//  var pcu := Path.Combine(dir, 'PABCSystem.pcu');
//end;

procedure CopyLibFiles;
begin
  var files := GetFilesByExtensions(GetLibDir(), CurrentLanguageInfo.languageExtensions);
  foreach f: string in files do
  begin
    &File.Copy(f, TestSuiteDir + PathSeparator + 'CompilationSamples' + PathSeparator + Path.GetFileName(f), true);
  end;
end;

procedure CopyCodeExamples;
begin
  var dir := GetCodeExamplesDir();
  if not Directory.Exists(dir) then exit;
  
  var files := GetFilesByExtensions(dir, CurrentLanguageInfo.languageExtensions, SearchOption.AllDirectories);
  foreach f: string in files do
  begin
    &File.Copy(f, TestSuiteDir + PathSeparator + 'CompilationSamples' + PathSeparator + Path.GetFileName(f), true);
  end;
end;

function MsToMinutes(ms: integer) : string;
begin
  var span := System.TimeSpan.FromMilliseconds(ms);
  Result := span.Minutes > 0 ? $'{span.Minutes}m {span.Seconds}s' : $'{span.Seconds}s';
end;

begin
  //DeletePABCSystemPCU;
  try
    Languages.Integration.LanguageIntegrator.LoadStandardLanguages();
    
    TestSuiteDir := GetCurrentTestSuiteDir();
    
    CurrentLanguageInfo := GetCurrentLanguageInfo(TestSuiteDir);
    
    Println($'----- {CurrentLanguageInfo.languageName} тесты -----');
    if (ParamCount = 2) and (ParamStr(2) = '1') then
      nogui := true;
    if (ParamCount = 0) or (ParamStr(1) = '1') then
    begin
      Println('Compiling tests...');
      DeletePCUFiles;
      ClearExeDir;
      CompileAllRunTests(false);
      Println('Success. Time elapsed: ' + MsToMinutes(MillisecondsDelta()));
    end;
    
    if (ParamCount = 0) or (ParamStr(1) = '2') then
    begin
      Println('Compiling compilation samples...');        
      CopyLibFiles;
      CopyCodeExamples;
      CompileAllCompilationTests('CompilationSamples', false);
      Println('Success. Time elapsed: ' + MsToMinutes(MillisecondsDelta()));
    end;
    if (ParamCount = 0) or (ParamStr(1) = '3') then
    begin
      Println('Compiling tests with multiple units and error throwing tests...');
      CompileAllUnits;
      CopyPCUFiles;
      CompileAllUsesUnits;
      CompileErrorTests(false);
      Println('Success. Time elapsed: ' + MsToMinutes(MillisecondsDelta()));
    end;
    if (ParamCount = 0) or (ParamStr(1) = '4') then
    begin
      Println('Running tests...');
      RunAllTests(false);
      Println('Success. Time elapsed: ' + MsToMinutes(MillisecondsDelta()));
      ClearExeDir;
      DeletePCUFiles;
    end;
    if (ParamCount = 0) or (ParamStr(1) = '5') then
    begin
      Println('Compiling tests in 32bit mode...');
      CompileAllRunTests(false, true);
      Println('Success. Time elapsed: ' + MsToMinutes(MillisecondsDelta()));
      Println('Running tests in 32bit mode...');
      RunAllTests(false);
      Println('Success. Time elapsed: ' + MsToMinutes(MillisecondsDelta()));
      ClearExeDir;
      Println('Compiling tests with PABCRtl.dll...');
      CompileAllRunTests(true);
      Println('Success. Time elapsed: ' + MsToMinutes(MillisecondsDelta()));
      Println('Compiling compilation samples with PABCRtl.dll...');
      CompileAllCompilationTests('pabcrtl_tests', true);
      Println('Success. Time elapsed: ' + MsToMinutes(MillisecondsDelta()));
    end;
    if (ParamCount = 0) or (ParamStr(1) = '6') then
    begin
      Println('Running tests with PABCRtl.dll...');
      RunAllTests(false);
      Println('Success. Time elapsed: ' + MsToMinutes(MillisecondsDelta()));
      Println('Running intellisense expression tests...');
      RunExpressionsExtractTests;
      Println('Success. Time elapsed: ' + MsToMinutes(MillisecondsDelta()));
      Println('Running basic intellisense tests...');
      RunIntellisenseTests;
      Println('Success. Time elapsed: ' + MsToMinutes(MillisecondsDelta()));
      Println('Running formatter tests...');
      RunFormatterTests;
      Println('Success. Time elapsed: ' + MsToMinutes(MillisecondsDelta()));
    end;
  except
    on e: Exception do
    begin
      if nogui then
        raise new Exception(e.ToString());
      assert(false, e.ToString());
    end;
  
  end;
end.