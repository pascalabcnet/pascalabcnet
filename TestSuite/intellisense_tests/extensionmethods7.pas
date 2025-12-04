function f1<T>(self:sequence of T); extensionmethod := self;

begin
  var seq1 := Seq(0)
    .f1&<integer>.Distinct{@(расширение sequence of T) function Distinct<integer>(): sequence of integer;@};
  ;
  seq1{@var seq1: sequence of integer;@}.ToString();
end.