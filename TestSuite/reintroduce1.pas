type
  t1 = class
    
    property p1: integer read 3; virtual;
    property p2: integer read 3; virtual;
    function f1: integer; virtual;
    begin
      Result := 3;
    end;
    function f2: integer; virtual;
    begin
      Result := 3;
    end;
  end;
  t2 = class(t1)
    
    property p1: integer read 5; reintroduce;
    property p2: integer read 5; override;
    function f1: integer; reintroduce; virtual;
    begin
      Result := 5;
    end;
    function f2: integer; override;
    begin
      Result := 5;
    end;
  end;
  t3 = class(t2)
  
    function f2: integer; override;
    begin
      Result := 5;
    end;
  end;
begin 
  var o: t1 := new t2;
  assert(o.p1 = 3);
  assert(o.p2 = 5);
  assert(o.f1 = 3);
  assert(o.f2 = 5);
  var o2: t2 := new t3;
  assert(o2.f1 = 5);
end.