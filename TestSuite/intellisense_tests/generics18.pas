function f1<T> := default(T);
function f2<T>: List<T> := nil;

begin
  var f{@var f: function(x: integer): integer;@} := f1&<function(x: integer): integer>;
  var ff{@var ff: List<function(x: integer): integer>;@} := f2&<function(x: integer): integer>;
end.