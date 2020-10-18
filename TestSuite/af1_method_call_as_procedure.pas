type
  TBase = class
    function f1: real;
    begin
      Result := 3.5;
      Assert(False);
    end;
  end;
  
  TErr = class(TBase)
    function f1: integer;
    begin
      Result := 2
    end;
  end;
  
begin
  var a := new TErr;
  Assert(a.f1=2);
  a.f1;
end.