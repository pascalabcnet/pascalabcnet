var s: string;
type
  c1<TSelf> = class
  where TSelf: c1<TSelf>;
    x: string := 'abc';
    
    static a := new TSelf[1];
    
    static procedure p1;
    begin
      assert((a[0] as c1<TSelf>).x.Length = 3); // 3
      s := a[0].x; // nil
      a[0].x := 'abc';
      assert((a[0] as c1<TSelf>).x.Length = 3); // 0
    end;
    
  end;
  
  c2 = class(c1<c2>) end;
  
begin
  c2.a[0] := new c2;
  c2.p1;
  assert(c2.a[0].x.Length = 3); // 0
  assert(s = 'abc');
end.