type
  t1=class
    private f1:byte;
    private f2:word;
    
    public property p1:byte read f1{@var t1.f1: byte;@} write f1;
    public property p2:word read f2;
  end;

begin
  var a := new t1;
  writeln(a.p2{@property t1.p2: word; readonly;@});
end.