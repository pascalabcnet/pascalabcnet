uses System.Collections;

var
  ht:hashtable;

begin
  ht:=hashtable.Create(10);
  ht['Hello']:='world!';
  writeln('Hello '+ht['Hello'].tostring);
  readln;
end.
