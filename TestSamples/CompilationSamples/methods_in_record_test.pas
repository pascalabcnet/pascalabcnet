type
  rec = record
    x:integer;
    function ToString: String; override;
    begin
      result := 'Работает! ' + x.tostring;
    end;
  end;
  
  
procedure Test;
var 
  x: real;
  rec2: record 
    x : real;
    procedure Test;
    var rec2 : 
      record 
        x : real;
        procedure Test;
        begin
          x:=3.3;
          writeln(x);
        end;
      end;  
    begin
      x := 2.2;
      rec2.test;
      writeln(x);
    end;
  end;  
begin
  x := 1.1;
  rec2.test;
  writeln(x);
end;

var r:rec;

begin
  writeln(r);
  writeln(r.x);
  test;
  readln;
end.