var flag: boolean;

type
  r1 = record
    i := 1;
    static function f(a: r1): boolean;
    begin
      Result := a = a;
    end;
    static function f2(a: r1): boolean;
    begin
      Result := a <> a;
    end;
    
    static function operator=(a, b: r1): boolean;
    begin
      flag := true;
      Result := a.i = b.i;
    end;
    
    static function operator<>(a, b: r1): boolean;
    begin
      flag := true;
      Result := a.i <> b.i;
    end;
  end;
  
begin
  assert(r1.f(new r1));
  assert(flag);
  flag := false;
  assert(not r1.f2(new r1));
  assert(flag);
end.