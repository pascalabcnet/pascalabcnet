{$reference TypedFileLibrary.dll}

begin
  Assign(ImportedIntFile, 'integers.dat');
  Rewrite(ImportedIntFile);
  Write(ImportedIntFile, 123456);
  Close(ImportedIntFile);
  var intInfo := new System.IO.FileInfo('integers.dat');
  assert(intInfo.Length = 4);
  Reset(ImportedIntFile);
  var i: integer;
  Read(ImportedIntFile, i);
  Close(ImportedIntFile);
  assert(i = 123456);

  Assign(ImportedRealFile, 'reals.dat');
  Rewrite(ImportedRealFile);
  Write(ImportedRealFile, 1.25);
  Close(ImportedRealFile);
  var realInfo := new System.IO.FileInfo('reals.dat');
  assert(realInfo.Length = 8);
  Reset(ImportedRealFile);
  var r: real;
  Read(ImportedRealFile, r);
  Close(ImportedRealFile);
  assert(r = 1.25);
end.
