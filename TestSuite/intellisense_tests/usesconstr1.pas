uses construnit1;
type
  t2=class
    
    class constructor := exit;
    private constructor := exit;
    public constructor(b:byte) := exit;
    
  end;

begin
  new t1{@constructor t1(b: byte);@}();
  new t2{@constructor t2();@}();
end.