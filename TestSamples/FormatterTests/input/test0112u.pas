unit test0112u;

type TClass<T> = class where T:record;
//a : T;
procedure Test<U>(params arr : array of U);
var t : TClass<T>;
begin
end;
end;

{TClass3<T> = template class
a : T;
end;

TClass2<T> = template class
a : TClass3<T>;
end;}

var cls : TClass<integer>;

begin
cls := new TClass<integer>;
//cls.Test(2,3,4);
end.