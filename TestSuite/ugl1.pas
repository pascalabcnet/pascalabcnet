// #2262
type t1 = class 
end;
  
function f1<T>(self: t1): t1; extensionmethod;
begin
  Result := self;
end;
  

begin
  var a := new t1;
  var b := a.f1&<byte>;
  Assert(a=b);
end.