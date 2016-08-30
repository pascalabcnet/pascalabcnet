//ä
begin
  var l := Seq(2,1,4);
  l.OfType&<integer>().ForEach{@(расширение) procedure ForEach<T>(Self: sequence of T; action: T->());@}(x->Print(x));
  var l1 := new List<integer>;
  var seq1{@var seq1: sequence of integer;@} := l1.Where{@(расширение) function IEnumerable<>.Where<>(predicate: integer->boolean): sequence of integer;@}(i->true);
  var l2 := l1.ConvertAll&{@function List<>.ConvertAll<TOutput>(converter: Converter<integer,TOutput>): List<TOutput>;@}<real>();
  var seq2{@var seq2: sequence of integer;@} := l1.Skip{@(расширение) function IEnumerable<>.Skip<>(count: integer): sequence of integer;@}(3);
  l1.OrderBy(i->i).Skip{@(расширение) function IEnumerable<>.Skip<>(count: integer): sequence of integer;@}(3);
end.