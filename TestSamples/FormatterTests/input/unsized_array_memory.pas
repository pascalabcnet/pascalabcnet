uses PointerTools;

type PPointer = ^pointer;

var a:array of byte;
    j:integer;
    p:pointer;
    i:=0;
begin
  SetLength(a,3);
  a[0]:=111;
  a[1]:=222;
  p:=pointer(@a);
  WriteMemoryToScreen(PPointer(p)^, 20);

  //writeln(pbyte(pointer(@a))^);//error!!!!!!1
  readln;
end.