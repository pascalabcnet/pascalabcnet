type TRec = record end;

type myproc = integer -> TRec;

begin
  var f: Func0<byte> := () -> begin Result{@var Result: byte;@} := 0; end;
  var f2: char->string := c -> begin Result{@var Result: string;@} := nil; end;
  var f3: myproc := i -> begin Result{@var Result: TRec;@} := default(TRec); end;
end.