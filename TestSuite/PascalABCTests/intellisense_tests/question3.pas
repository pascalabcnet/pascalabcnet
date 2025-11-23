function f1<T>(self: sequence of T; f: T->T): sequence of T; extensionmethod := self;

begin
  Arr(0)
  .f1(i -> 0=0?0:0 )
  .f1{@(расширение sequence of T) function f1<T>(f: T->T): sequence of T;@}(i->i)
end.