type
  A = class
    procedure Deconstruct(var i: integer; var j: integer);
    begin
      i := 1;
      j := 1;
    end;
  end;
  
begin
  var a1 := new A;
  var a2: A;
  a1.Deconstruct(a2);
  var i,j: integer;
  a1.Deconstruct(i,j);
  Assert((i,j)=(1,1));
end.  