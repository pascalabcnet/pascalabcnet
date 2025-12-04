type TClass = class
a : integer;
constructor(a : integer);
begin
  self.a := a;
end;

class function operator implicit(t: TClass):TClass;
begin
  Result := t.a;  
end;

end;

begin
  
end.