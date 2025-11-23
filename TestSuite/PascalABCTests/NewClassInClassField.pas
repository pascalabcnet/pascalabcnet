Type
  ИмяКласса_1 = class
    ИмяКласса_2 := new class(d:=1);
  end;
Begin
  var s := new ИмяКласса_1;
  Assert(s.ИмяКласса_2.d = 1);
End.