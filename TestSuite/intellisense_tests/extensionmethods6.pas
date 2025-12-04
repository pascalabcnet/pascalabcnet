type t1<T> = class end;

procedure p1(self:t1<T>); extensionmethod := exit;

begin
  var a := new t1<byte>;
  a.p1{@(расширение) procedure t1<T>.p1();@};
  a.p1{@(расширение) procedure t1<T>.p1();@}();
end.