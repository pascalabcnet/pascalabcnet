function f1<T>(self:sequence of T); extensionmethod := self;

begin
  Seq(0)
    .f1&<integer>.Distinct{@(расширение sequence of T) function Distinct<integer>(): sequence of integer;@};
  ;
end.