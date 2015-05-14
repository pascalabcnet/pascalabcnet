type
  rec = record
    x: real;
    procedure test;
    begin
      x:=1.1;
      writeln(x);
      writeln(self.x);
      
    end;
  end;

var r: rec;

begin
  r.test;
  readln;
end.