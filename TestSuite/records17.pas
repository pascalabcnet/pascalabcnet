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
    
    {static function operator=(a, b: r1): boolean;
    begin
      Result := a.i = b.i;
    end;
    
    static function operator<>(a, b: r1): boolean;
    begin
      Result := a.i <> b.i;
    end;}
  end;
  
begin
  assert(r1.f(new r1));
  assert(not r1.f2(new r1));
end.