begin
var a:word:=1;
inc{@procedure Inc(var i: word);@}(a);
var smi:smallint:=1;
inc{@procedure Inc(var i: smallint);@}(smi);
var i: integer;
Inc{@procedure Inc(var i: integer);@}(i);
var si: shortint;
Inc{@procedure Inc(var i: shortint);@}(si);
var lw: longword;
Inc{@procedure Inc(var i: longword);@}(lw);
var i64: int64;
Inc{@procedure Inc(var i: int64);@}(i64);
var ui64: uint64;
Inc{@procedure Inc(var i: uint64);@}(ui64);

end.