begin
  var t{@var t: (integer,real,set of integer);@} := (1, 2.3, [2,3,4]);
  var s{@var s: set of integer;@} := t.Item3;
  writeln(t.Item3{@property Tuple<>.Item3: set of integer; readonly;@});
end.