type TRec = record
  a : integer;
end;

procedure Test(const a : integer);
begin
  assert(a = 10);
end;

procedure Test2(const s : set of char);
begin
  assert(s = ['1','2','3']);
end;

procedure Test4(const s : set of 1..3);
begin
  assert(s=[1,2]);
end;

procedure Test3(const s : string);
begin
  assert(s = 'abc');
end;

procedure Test5(const s : string[3]);
begin
  assert(s='abc');
end;

procedure Test6(const s : 1..4);
begin
  assert(s=3);  
end;

procedure Test7(const r : real);
begin
  assert(r=2.0);
end;

procedure Test8(const arr : array of integer);
begin
  assert(arr[0]=2);  
end;

procedure Test9(const rec: TRec);
begin
  assert(rec.a = 12);
end;

var rec : TRec;

begin
  Test(10);
  Test2(['1','2','3']);
  Test3('abc');
  Test4([1,2]);
  Test5('abcdef');
  Test6(3);
  Test7(2);
  Test8(new integer[3](2, 4, 8));
  rec.a := 12;
  Test9(rec);
end.