type
  t1<T>=class
    
    b:byte;
    c: T;
    
    public class function GetNew := default(t1<T>);
   
  end;

begin
  var a1 := t1&<byte>.GetNew();
  a1.b{@var t1<>.b: byte;@} := 3;
  a1.c{@var t1<>.c: byte;@} := 3;
  var a2 := t1&<byte>.Create;
  a2.b{@var t1<>.b: byte;@} := 4;

end.