
unit arr_unit;

interface

procedure write_array(params arr:array of object);overload;

procedure write_array(arr:array of integer);overload;

implementation

procedure write_array(params arr:array of object);
var
  i:integer;
begin
 writeln;
 writeln('Object array:');
 for i:=0 to arr.length-1 do
  begin
   writeln(arr[i].tostring);
  end;
  writeln('End of object array.');
  writeln;
end;

procedure write_array(arr:array of integer);
var
  i:integer;
begin
writeln;
 writeln('Integer array:');
 for i:=0 to arr.length-1 do
  begin
   writeln(arr[i].tostring);
  end;
  writeln('End of integer array.');
  writeln;
end;


end.