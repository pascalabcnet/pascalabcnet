// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
uses 
  FilesOperations,
  System.IO,
  System.Resources;

begin
  var Files := DirectoryInfo.Create(Path.Combine('..', '..', 'bin', 'Lng', 'Rus')).GetFiles('*.*');
  Writeln('Найдено ' + Files.Length.tostring + ' файлов, обработка...');
  var res := '';
  for var i := 0 to Files.Length - 1 do
    res := res + #13#10#13#10'//' + Files[i].Name + #13#10'%PREFIX%='#13#10 +
    	ReadFileToEnd(Files[i].FullName);
  Writeln('Сохранение собраного файла в ресурс...');
  var ResWriter := ResourceWriter.Create(Path.Combine('..', '..', 'Localization', 'DefaultLang.resources'));
  ResWriter.AddResource('DefaultLanguage', System.Text.Encoding.GetEncoding(1251).GetBytes(res));
  ResWriter.Generate;
  ResWriter.Close;
  Writeln('OK');
end.