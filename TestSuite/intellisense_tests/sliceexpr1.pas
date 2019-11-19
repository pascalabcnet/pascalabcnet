begin
  var a: array of integer;
  writeln(a[1:].Length{@property Array.Length: integer; readonly;@});
  writeln(a[:1].Length{@property Array.Length: integer; readonly;@});
  writeln(a[:].Length{@property Array.Length: integer; readonly;@});
  writeln(a[1:2:4].Length{@property Array.Length: integer; readonly;@});
end.