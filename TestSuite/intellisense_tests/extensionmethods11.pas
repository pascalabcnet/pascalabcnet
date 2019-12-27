begin
  var l := Lst(1,2,3,4);
  l.Count{@(расширение sequence of T) function Count<integer>(predicate: integer->boolean): integer;@}(i->i>2).Print;
  writeln(l.Count{@property List<>.Count: integer; virtual; readonly;@});
end.