type TClass = class
a : integer;
constructor(a : integer);
begin
  self.a := a;
end;

class function operator implicit(a : integer):TClass;
begin
  Result := new TClass(a);  
end;

class function operator implicit(a : real):TClass;
begin
  Result := new TClass(round(a));  
end;

class function operator implicit(t : TClass):integer;
begin
  Result := t.a;  
end;

class function operator explicit(t : TClass):char;
begin
Result := System.Convert.ToChar(t.a);  
end;
end;

var t : TClass;
    i : integer;
    r : real;
    
begin
t := 202;
assert(t.a = 202);
i := t;//integer(t);
assert(i=202);
var c : char := char(t);
assert(c=char(202));
t := 112.23;
assert(t.a = 112);
end.