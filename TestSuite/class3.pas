type
  t1=class
    
    public i1 := 1;
    public i2 := 2;
    public o: object := new Object;
    
    public constructor(i1, i2: integer);
    begin
      assert(o <> nil);
      self.i1 := i1;
      self.i2 := i2;
    end;
    
    public constructor(i1: integer) :=
    Create(i1,6);
    
  end;
  
  t2 = class
    a: array of integer := (1, 2, 3);
    b: set of char := ['a', 'b', 'c'];
    constructor(a: array of integer);
    begin
      assert('b' in b);
      assert(self.a[0] = 1);
      self.a := a;
    end;
  end;
  

begin
  var o1 := new t1(3,4);
  var o2 := new t1(5);
  assert(o2.i1 = 5);
  assert(o2.i2 = 6);
  var o3 := new t2(Arr(4, 5, 6));
  assert(o3.a[0] = 4);
end.