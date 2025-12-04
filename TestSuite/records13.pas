type
  TRec2 = record
    x := 2;
    constructor(xx: integer);
    begin
      x := xx;
    end;
  end;
  
  TRec = record
    x := 1;
    r: TRec2;
    constructor Create(xx: integer);
    begin
      x := xx;
      r := new TRec2(3);
    end;
  end;
  
  TClass = class
    r: TRec;
  end;
  
var gl_r1 := new TRec(10);
var gl_r2: TRec;
  
begin
  var r1 := new TRec(10);
  var r2: TRec;
  assert(r1.x = 10);
  assert(r1.r.x = 3);
  assert(r2.x = 1);
  assert(r2.r.x = 2);
  assert(gl_r1.x = 10);
  assert(gl_r1.r.x = 3);
  assert(gl_r2.x = 1);
  assert(gl_r2.r.x = 2);
  var o := new TClass;
  assert(o.r.x = 1);
  var a1: array of TRec;
  SetLength(a1, 3);
  assert(a1[0].x = 1);
end.