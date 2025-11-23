type
  A = class end;
  
var i: integer;

procedure D<T>(self: T; var a: T); extensionmethod;begin i := 1 end;

procedure D(self: A; var b: A; var c: A); extensionmethod;begin i := 2 end;

begin
  var t := new A;
  var b, c: A;
  t.D(b);
  assert(i = 1);
  t.D(b, c);
  assert(i = 2);
end.