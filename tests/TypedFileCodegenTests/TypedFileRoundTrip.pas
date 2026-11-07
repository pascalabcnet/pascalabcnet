var intFile: file of integer;
    realFile: file of real;

begin
  Assign(intFile, 'integers.dat');
  Rewrite(intFile);
  Write(intFile, 123456);
  Close(intFile);
  var intInfo := new System.IO.FileInfo('integers.dat');
  assert(intInfo.Length = 4);
  Reset(intFile);
  var i: integer;
  Read(intFile, i);
  Close(intFile);
  assert(i = 123456);

  Assign(realFile, 'reals.dat');
  Rewrite(realFile);
  Write(realFile, 1.25);
  Close(realFile);
  var realInfo := new System.IO.FileInfo('reals.dat');
  assert(realInfo.Length = 8);
  Reset(realFile);
  var r: real;
  Read(realFile, r);
  Close(realFile);
  assert(r = 1.25);
end.
