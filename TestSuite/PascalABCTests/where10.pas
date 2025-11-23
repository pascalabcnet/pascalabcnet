procedure p1<T1,T2>(a: T1; b: T2); where T1, T2: record;
begin
  
end;
procedure p2<T1,T2,T3>(a: T1; b: T2; c: T3); where T1, T2: record; where T3: record;
begin
  
end;
procedure p3<T1>(a: T1); where T1: record;
begin
  
end;

type Digits = (one, two);
begin
  p1(0, 2);
  p2(2, 4, 6);
  p3(one);
end.