procedure p2<T>(params a: array of T);
begin
  assert(a.Length = 3);
  assert(a[0].ToString() = '1');
  assert(a[1].ToString() = '2');
  assert(a[2].ToString() = '3');
end;

procedure p<T>(params a: array of T);
begin
  p2(a[0], a[1], a[2]);
end;

type TRec = record
  x: integer;
end;

type digits = (one, two, three);

procedure p3(params a: array of TRec);
begin
  assert(a[0].x = 1);
  assert(a[1].x = 2);
end;

procedure p4(params a: array of digits);
begin
  assert(a[0] = one);
  assert(a[1] = two);
end;

procedure p5(params a: array of object);
begin
  assert(a[0] = nil);
  assert(a[1] = nil);
end;

procedure p6(params a: array of integer?);
begin
  assert(a[0] = nil);
  assert(a[1] = nil);
end;

procedure p7(params a: array of integer?);
begin
  assert(a[0] = 1);
  assert(a[1] = 2);
end;

procedure p8(params a: array of integer);
procedure Nested(params a: array of integer);
begin
  assert(a[0] = 1);
  assert(a[1] = 2);
end;
begin
  Nested(a[0], a[1]);  
end;

type CharSet = set of char;

procedure p9(params a: array of CharSet);
begin
  assert('d' in a[0]);
  assert('k' in a[1]);
end;

begin
  p(1,2,3);
  var r1: TRec;
  var r2: TRec;
  r1.x := 1;
  r2.x := 2;
  p3(r1, r2);
  p4(one, two);
  p5(nil, nil);
  p6(nil, nil);
  p7(1, 2);
  p8(1, 2);
  p9(['d','e'], ['k']);
end.