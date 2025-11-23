var i: integer;

type
  t1 = record
    private function InitA: integer;
    begin
      Inc(i);
    end;
    
    public a := InitA;
    public b: integer;
    
    public constructor(b: integer) := self.b := b;
    
  end;
  
begin
  var l := new List<t1>;
  l += new t1(2);
  l += new t1(3);
  foreach var o in l do
    ;
  assert(i = 2);
end.