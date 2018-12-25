type
  TExample = record
    private a := new integer[10,10];
    
    property p[i, j: integer]: integer read a[i, j] write begin a[i,j] := value; end;
  end;
 
begin
  var c := new TExample();
  for var i:=0 to 9 do
    for var j:=0 to 9 do
      c.p[j, i] := i+j*j;
      
  for var i:=0 to 9 do
    for var j:=0 to 9 do
      assert(c.p[j, i] = i+j*j);
end.