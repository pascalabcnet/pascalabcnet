var i: integer;

type
  t0base=abstract class
    procedure p1; abstract;
    procedure p2<U>(a: U); abstract;
  end;
  
  t0base2<T>=abstract class
    procedure p1(a:T); abstract;
  end;
  
  t0<T> = class(t0base)
    procedure p1; override := exit;
    procedure p2<U>(a: U); override := exit;
  end;
  
  t1=class(t0<byte>) end;
  
  t2<T> = class(t0base2<T>)
    procedure p1(a: T); override;
    begin
      assert(a = default(T));
      i := 3;
    end;
    
  end;
  
  t3=class(t2<byte>) end;
  
begin
  var o := new t0<word>;
  var o2 := new t1;
  var o3 := new t3;
  var o4 := new t2<word>;
  o3.p1(0);
  assert(i = 3);
  i := 0;
  o4.p1(0);
  assert(i = 3);
end.