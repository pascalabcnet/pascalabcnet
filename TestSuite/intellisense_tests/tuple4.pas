begin
  var t{@var t: (integer,real,array of integer);@} := (1, 2.3, [2,3,4]);
  var s{@var s: array of integer;@} := t.Item3;
  writeln(t.Item3{@property Tuple<>.Item3: array of integer; readonly;@});
end.
