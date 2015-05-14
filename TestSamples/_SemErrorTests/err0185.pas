type TClass = class
a : integer;
constructor(a : integer);
begin
  self.a := a;
end;

class function operator explicit(t: TClass):TClass;
begin
  Result := t.a;  
end;

end;

begin
  
end.