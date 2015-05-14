uses System;

var i:integer;
    t:System.DateTime;
    s:string;
begin
  write('test1');
  write('test2');
  writeln('test3');
  writeln('test4');
  writeln('test5');
  t := DateTime.Now;
  while i<200 do begin
    write(i,' ');
    i:=i+1;
  end;
  Writeln;
  Writeln((DateTime.Now-t).TotalMilliseconds);
  readln(s);
  Writeln(uppercase(s));
  readln;
  //readln;
end.