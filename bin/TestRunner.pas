﻿// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
{$reference Compiler.dll}
{$reference CodeCompletion.dll}
{$reference Errors.dll}
{$reference CompilerTools.dll}
{$reference Localization.dll}
{$reference System.Windows.Forms.dll}

uses PascalABCCompiler, System.IO, System.Diagnostics;

var
  TestSuiteDir: string;
  nogui: boolean;
  
var
  PathSeparator: string := Path.DirectorySeparatorChar;

function IsUnix: boolean;
begin
  Result := (System.Environment.OSVersion.Platform = System.PlatformID.Unix) or (System.Environment.OSVersion.Platform = System.PlatformID.MacOSX);  
end;

function GetTestSuiteDir: string;
begin
  var dir := Path.GetDirectoryName(GetEXEFileName());
  var ind := dir.LastIndexOf('bin');
  Result := dir.Substring(0, ind) + 'TestSuite';
end;

function GetLibDir: string;
begin
  var dir := Path.GetDirectoryName(GetEXEFileName());
  Result := dir + PathSeparator + 'Lib';
end;

procedure CompileErrorTests(withide: boolean);
begin
  
  var comp := new Compiler();
  
  var files := Directory.GetFiles(TestSuiteDir + PathSeparator + 'errors', '*.pas');
  for var i := 0 to files.Length - 1 do
  begin
    var content := &File.ReadAllText(files[i]);
    if content.StartsWith('//winonly') and IsUnix then
      continue;
    
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
    end;
    if i mod 50 = 0 then
      System.GC.Collect();
  end;
  
end;

procedure CompileAllRunTests(withdll: boolean; only32bit: boolean := false);
begin
  
  var comp := new Compiler();
  
  var files := Directory.GetFiles(TestSuiteDir, '*.pas');
  for var i := 0 to files.Length - 1 do
  begin
    if IsUnix then
      writeln('Compile file '+files[i]);
    var content := &File.ReadAllText(files[i]);
    if content.StartsWith('//winonly') and IsUnix then
      continue;
    if content.StartsWith('//nopabcrtl') and withdll then
      continue;
    var co: CompilerOptions := new CompilerOptions(files[i], CompilerOptions.OutputType.ConsoleApplicaton);
    co.Debug := true;
    co.OutputDirectory := TestSuiteDir + PathSeparator + 'exe';
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
  
  var files := Directory.GetFiles(TestSuiteDir + PathSeparator + dir, '*.pas');
  for var i := 0 to files.Length - 1 do
  begin
    var content := &File.ReadAllText(files[i]);
    if content.StartsWith('//winonly') and IsUnix then
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
  var files := Directory.GetFiles(TestSuiteDir + PathSeparator + 'units', '*.pas');
  var dir := TestSuiteDir + PathSeparator + 'units' + PathSeparator;
  for var i := 0 to files.Length - 1 do
  begin
    var content := &File.ReadAllText(files[i]);
    if content.StartsWith('//winonly') and IsUnix then
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
  System.Environment.CurrentDirectory := Path.GetDirectoryName(GetEXEFileName());
  var comp := new Compiler();
  
  var files := Directory.GetFiles(TestSuiteDir + PathSeparator + 'usesunits', '*.pas');
  for var i := 0 to files.Length - 1 do
  begin
    var content := &File.ReadAllText(files[i]);
    if content.StartsWith('//winonly') and IsUnix then
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
  System.Environment.CurrentDirectory := Path.GetDirectoryName(GetEXEFileName());
  var files := Directory.GetFiles(TestSuiteDir + PathSeparator + 'units', '*.pcu');
  
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
  var files := Directory.GetFiles(TestSuiteDir + PathSeparator + 'exe', '*.exe');
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
  PascalABCCompiler.StringResourcesLanguage.CurrentTwoLetterISO := 'ru';
  CodeCompletion.CodeCompletionTester.TestIntellisense(TestSuiteDir + PathSeparator + 'intellisense_tests');
end;

procedure RunFormatterTests;
begin
  CodeCompletion.FormatterTester.Test();
  var errors := &File.ReadAllText(TestSuiteDir + PathSeparator + 'formatter_tests' + PathSeparator + 'output' + PathSeparator + 'log.txt');
  if not string.IsNullOrEmpty(errors) then
  begin
    System.Windows.Forms.MessageBox.Show(errors + System.Environment.NewLine + 'more info at TestSuite/formatter_tests/output/log.txt');
    Halt;
  end;
end;

procedure ClearDirByPattern(dir, pattern: string);
begin
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

procedure DeletePABCSystemPCU;
begin
  var dir := Path.Combine(Path.GetDirectoryName(GetEXEFileName()), 'Lib');
  var pcu := Path.Combine(dir, 'PABCSystem.pcu');
end;

procedure CopyLibFiles;
begin
  var files := Directory.GetFiles(GetLibDir(), '*.pas');
  foreach f: string in files do
  begin
    &File.Copy(f, TestSuiteDir + PathSeparator + 'CompilationSamples' + PathSeparator + Path.GetFileName(f), true);
  end;
end;

begin
  //DeletePABCSystemPCU;
  try
    TestSuiteDir := GetTestSuiteDir;
    System.Environment.CurrentDirectory := Path.GetDirectoryName(GetEXEFileName());
    if (ParamCount = 2) and (ParamStr(2) = '1') then
      nogui := true;
    if (ParamCount = 0) or (ParamStr(1) = '1') then
    begin
      DeletePCUFiles;
      ClearExeDir;
      CompileAllRunTests(false);
      writeln('Tests to run: '+Milliseconds()+'ms');
    end;
    
    if (ParamCount = 0) or (ParamStr(1) = '2') then
    begin
      CopyLibFiles;
      CompileAllCompilationTests('CompilationSamples', false);
      writeln('CompilationSamples: '+Milliseconds()+'ms');
    end;
    if (ParamCount = 0) or (ParamStr(1) = '3') then
    begin
      CompileAllUnits;
      CopyPCUFiles;
      CompileAllUsesUnits;
      CompileErrorTests(false);
      writeln('Other tests '+Milliseconds()+'ms'+'. Tests compiled successfully ');
    end;
    if (ParamCount = 0) or (ParamStr(1) = '4') then
    begin
      RunAllTests(false);
      writeln('Tests run successfully: '+Milliseconds()+'ms');
      ClearExeDir;
      DeletePCUFiles;
    end;
    if (ParamCount = 0) or (ParamStr(1) = '5') then
    begin
      
        CompileAllRunTests(false, true);
        writeln('Tests in 32bit mode compiled successfully');
        RunAllTests(false);
        writeln('Tests in 32bit run successfully');
        ClearExeDir;
        CompileAllRunTests(true);
        writeln('Tests with pabcrtl compiled successfully');
        CompileAllCompilationTests('pabcrtl_tests', true);
    end;
    if (ParamCount = 0) or (ParamStr(1) = '6') then
    begin
      RunAllTests(false);
      writeln('Tests with pabcrtl run successfully');
      System.Environment.CurrentDirectory := Path.GetDirectoryName(GetEXEFileName());
      RunExpressionsExtractTests;
      writeln('Intellisense expression tests run successfully');
      RunIntellisenseTests;
      writeln('Intellisense tests run successfully');
      RunFormatterTests;
      writeln('Formatter tests run successfully');
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