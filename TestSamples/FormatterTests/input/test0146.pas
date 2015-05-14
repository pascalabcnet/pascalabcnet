uses test0146u;

var 
    cls : TClass<char>;
    
begin
 arr4 := new object[2,2]((2,3),(4,6));
 assert(integer(arr4[1,1])=6);
 arr4[0,1] := 3;
 assert(integer(arr4[0,1])=3);
 cls := new TClass<char>(3,4);
  cls.SetElem(2,2,'m');
  assert(cls.GetElem(2,2)='m');
end.