type
  t0<T> = class end;
  
  t1 = class
    
    function f1<T>: array of T; virtual := nil;
    function f2<T>: t0<T>; virtual := nil;
    function f3<T>: ^T; virtual := nil;
    function f4<T1,T2>(a: T1; b: T2): (T1,T2); virtual := nil;
  end;
  
  t2 = class(t1)
    function f1<T>: array of T; override := new T[2];
    function f2<T>: t0<T>; override := new t0<T>;
    function f3<T>: ^T; override := nil;
    function f4<T2,T1>(a: T2; b: T1): (T2,T1); override := (a,b);
  end;
  
begin 
  var o: t1 := new t2;
  var a := o.f1&<integer>;
  assert(a.Length = 2);
  var o2 := o.f2&<integer>;
  assert(o2 <> nil);
  var o3 := o.f4&<integer,string>(2,'a');
  assert(o3.Item1 = 2);
  assert(o3.Item2 = 'a');
end.