//ä
begin
  var l{@var l: sequence of integer;@} := Seq(2,1,4);
  l.OfType&<integer>().ForEach{@(расширение sequence of T) procedure ForEach<T>(action: T->());@}(x->Print(x));
  var l1 := new List<integer>;
  var seq1{@var seq1: sequence of integer;@} := l1.Where{@(расширение sequence of T) function Where<integer>(predicate: integer->boolean): sequence of integer;@}(i->true);
  var l2 := l1.ConvertAll&{@function List<>.ConvertAll<TOutput>(converter: Converter<integer,TOutput>): List<TOutput>;@}<real>();
  l2{@var l2: List<real>;@}.Add(23);
  var seq2{@var seq2: sequence of integer;@} := l1.Skip{@(расширение sequence of T) function Skip<integer>(count: integer): sequence of integer;@}(3);
  var seq3 := l1.OrderBy(i->i).Skip{@(расширение sequence of T) function Skip<integer>(count: integer): sequence of integer;@}(3);
  seq3{@var seq3: sequence of integer;@}.ToString();
end.