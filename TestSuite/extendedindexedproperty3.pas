type
  TBase = abstract class
    protected a := new integer[10,10,10];
  end;
  
  TExample = class(TBase)
    public property p[i, j: integer; k: integer]: integer 
        read a[i, j, k] write begin a[i,j,value] := k*k+i+j; end;
  end;
 
begin
  var c := new TExample();
  var k := 5;
  for var i:=0 to 9 do
    for var j:=0 to 9 do
        c.p[i, j, k] := 0;
      
  for var i:=0 to 9 do
    for var j:=0 to 9 do
      assert(c.p[i, j, 0] = 25 + i + j);
      
  for var i:=0 to 9 do
    for var j:=0 to 9 do
      for var d:=1 to 9 do
        assert(c.p[i, j, d] = 0);
end.