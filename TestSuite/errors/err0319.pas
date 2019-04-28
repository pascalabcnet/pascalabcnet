type AAA = record s: string[4] end;

begin
  var f := OpenFile&<AAA>('a.dat');
  //f.Read;
end.