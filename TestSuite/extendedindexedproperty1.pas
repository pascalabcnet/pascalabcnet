type
  TExample = class
    private a := new integer[10,10];
    
    property p[i, j: integer]: integer read a[i, j] write begin a[i,j] := value; end;
  end;
 
begin
  var c := new TExample();
  c.p[3,2] := 10;
  c.p[0,0] := -2;
  assert(c.p[3,2] = 10);
  assert(c.p[0,0] = -2);
  assert(c.p[1,3] = 0);
  assert(c.p[9,9] = 0);
end.