uses test0176u;

var rec : TRec;

begin
  Test(10);
  Test2(['1','2','3']);
  Test3('abc');
  Test4([1,2,4]);
  Test5('abcdef');
  Test6(3);
  Test7(2);
  Test8(new integer[3](2, 4, 8));
  rec.a := 12;
  Test9(rec);
end.