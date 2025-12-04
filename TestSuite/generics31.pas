type
  t1<T> = class
    a := new integer[5];
  end;
  
  t2 = class
  end;
  
procedure p1<T,TEl>(x: T); where T: t1<TEl>;
begin
  assert(x.a.GetType.FullName = 'System.Int32[]'); // говорит что "t1<byte>", хотя должно быть "array of word"
end;

procedure p2<T>(x: T);
begin
  assert(x.ToString = '2');
end;

procedure p3<T>(x: T); where T: class;
begin
  assert(x.ToString = 'generics31.t2');
end;

begin
  p1&<t1<byte>,byte>(new t1<byte>);
  p2(2);
  p3(new t2);
end.