type
  TClass = class
  end;
  
begin
  var x{@var x: (integer,List<byte>);@} := (0, new List<byte>());
  var y{@var y: (integer,TClass);@} := (0, new TClass());
  writeln(y.Item2{@property Tuple<>.Item2: TClass; readonly;@});
end.