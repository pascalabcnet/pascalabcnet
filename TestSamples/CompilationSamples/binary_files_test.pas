type pointrec = record
    x: integer;
    y: integer;
    line: string;
  end;
   
var f: file;
    x: real;
    i: integer;
    s: string;
    r1, r2: pointrec;

begin
  f := new BinaryFile;
  assign(f, 'binary_file_test.dat');
  rewrite(f);
  write(f, 3.14, 7);
  r1.x := 14;
  r1.y := 21;
  r1.line := 'It is record!';
  write(f, r1);
  write(f, 'Hi!');
  close(f);
  reset(f);
  read(f, x, i);
  writeln(eof(f));
  read(f, r2, s);
  writeln(eof(f));
  writeln(x);
  writeln(i);
  writeln(r2.x, ' ', r2.y, ' ', r2.line);
  writeln(s);
  close(f);
end.