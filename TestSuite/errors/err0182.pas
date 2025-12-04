type TClass = class
a : integer;
constructor(a : integer);
begin
  self.a := a;
end;

class function operator explicit(t : TClass):integer;
begin
  Result := t.a;  
end;

class function operator explicit(t : TClass):integer;
begin
Result := System.Convert.ToChar(t.a);  
end;

end;

begin
  
end.