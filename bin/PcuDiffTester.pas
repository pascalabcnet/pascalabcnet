// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

// Тесты на совпадение pcu файлов модулей при различных сценариях компиляции
// Например, при полной пересборке всех стандартных модулей и при сборке их по отдельности

{$reference Compiler.dll}
{$reference LanguageIntegrator.dll}

uses System.IO, PascalABCCompiler;

const
  tempDirName = 'TempForPCUDiffTest';

/// Побайтовое сравнение файлов
procedure CompareBinaryFiles(filePath1, filePath2: string; var error: Exception);
begin
  var fileInfo1 := new FileInfo(filePath1);
  var fileInfo2 := new FileInfo(filePath2);
  
  if fileInfo1.Length <> fileInfo2.Length then
  begin
    error := new Exception($'File sizes differ: first - {fileInfo1.Length} vs second - {fileInfo2.Length}');
    exit;
  end;
  
  // Сравнение всех байтов пока что не следует запускать. Нужно решить многочисленные проблемы с несовпадением значений location в pcu
  // или исправить ошибки работы флага InternalDebug.IncludeDebugInfoInPCU
//  var fileBytes1 := &File.ReadAllBytes(filePath1);
//  var fileBytes2 := &File.ReadAllBytes(filePath2);
//  
//  for var i := 0 to fileInfo1.Length - 1 do
//  begin
//    if fileBytes1[i] <> fileBytes2[i] then
//    begin
//      error := new Exception($'Mismatch of files in position {i}: {fileBytes1[i]} vs {fileBytes2[i]}');
//      exit;
//    end;
//  end;
end;

begin
  Languages.Integration.LanguageIntegrator.LoadAllLanguages();
  
  var comp := new Compiler();
  comp.InternalDebug.SkipPCUErrors := False;
  // TODO: Флаг ниже вызывает ошибку при компиляции School.pas 
  // comp.InternalDebug.IncludeDebugInfoInPCU := False;
  comp.InternalDebug.CodeGeneration := False;
  
  var co := new CompilerOptions();
  co.Debug := True;
  co.UseDllForSystemUnits := False;
  co.Rebuild := False;
  
  Println('----- Recompiling PCU tests -----');
  
  Directory.CreateDirectory(tempDirName);
  
  // Скомпилированные уже с помощью RebuildStandartModules.pas модули переносим во временную папку.
  // ВАЖНО: Компилировать модули заново обязательно в папке Lib, потому что при сериализации вызова функции Assert в pcu файле модуля
  // сохраняется полный путь к файлу модуля, а значит этот путь должен быть всегда одинаковым для последующего сравнения pcu
  foreach var filePath in Directory.GetFiles('Lib', '*.pcu') do
    &File.Move(filePath, Path.Combine(tempDirName, Path.GetFileName(filePath)));
  
  var error: Exception;
  
  var standardModuleFileNames := Directory.GetFiles(tempDirName, '*.pcu')
                                         .Select(filePath -> Path.GetFileNameWithoutExtension(filePath));
  
  Println('Recompiling standard modules...');
  
  // TODO: При компиляции PABCSystem отдельно от других модулей ее pcu все еще отличается, поэтому тут он фильтруется
  foreach var fileName in standardModuleFileNames.Where(name -> name <> 'PABCSystem').Select(name -> name + '.pas') do
  begin
    co.SourceFileName := Path.GetFullPath(Path.Combine('Lib', fileName));
    
    // Отдельная компиляция каждого модуля без флага /rebuild
    comp.Compile(co);
    
    if comp.ErrorsList.Count > 0 then
    begin
      error := new Exception($'Compilation of {fileName} failed{NewLine}{comp.ErrorsList[0].ToString()}');
      break;
    end;
  end;
  
  if error = nil then
  begin
    Println('Comparing PCU file versions from before and after recompilation...');
    
    foreach var fileName in standardModuleFileNames.Select(name -> name + '.pcu') do
    begin
      // Сравнение версий pcu, полученных до и после перекомпиляции
      CompareBinaryFiles(Path.Combine('Lib', fileName), Path.Combine(tempDirName, fileName), error);
      if error <> nil then
        break;
    end;
  end;
  
  // возврат исходных pcu в папку Lib
  foreach var filePath in Directory.GetFiles(tempDirName, '*.pcu') do
  begin
    &File.Delete(Path.Combine('Lib', Path.GetFileName(filePath)));
    &File.Move(filePath, Path.Combine('Lib', Path.GetFileName(filePath)));
  end;
  
  Directory.Delete(tempDirName, True);
  
  if error <> nil then
    raise error
  else
    Println('Success. No differences found.');

end.
