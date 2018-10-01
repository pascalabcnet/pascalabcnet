type
  TClass = class
    fByte: byte := 2;
  end;

begin
  var x: TClass := new TClass;
  var xxx1 := x=nil ? nil : x.fByte;
  var xxx := x?.fByte;
  
  Assert(xxx.Value=2);
  Assert(xxx1.Value=2);
end.