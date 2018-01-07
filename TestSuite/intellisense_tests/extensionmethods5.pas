type TClass = class
procedure Test2;
end;

type TClass2 = TClass;

procedure TClass2.Test;
begin
  
end;

function TClass2.Test3: integer;
begin
  
end;

procedure TClass.Test2;
begin
  
end;

type myarr = array of integer;

procedure MyProc(self: myarr); extensionmethod;
begin
  
end;

begin
  var o: TClass2;
  var o2: TClass;
  o.Test{@(расширение) procedure TClass2.Test();@}();
  o2.Test{@(расширение) procedure TClass2.Test();@}();
  o.Test3{@(расширение) function TClass2.Test3(): integer;@}();
  var arr1 := new integer[10];
  arr1.MyProc{@(расширение) procedure myarr.MyProc();@}();
end.