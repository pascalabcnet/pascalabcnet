unit u_extensionmethods1;

function object.My : integer;
begin
  var l : integer;
  Result := 5;
  
end;

type MyInt = integer;

type TClass = class
event ev : System.EventHandler;
procedure Test;

end;

procedure TClass.Test;
begin
  var a : integer;
  
end;

function MyInt.Test : real;
begin
  var a : integer;
  var b : char;
  var i : MyInt;
  
end;

procedure Test;
begin
var a : integer;
  
end;

type TRec = record
a : integer;
b : integer;
end;

type IntArr = array of integer;

function IntArr.Test : integer;
begin
  Result := 7;
end;

var o : object;
begin
o := 3; assert(o.My() = 5);
var jj := o.My();
var i : integer; assert(i.My() = 5);
var arr : IntArr := (2,3); assert(arr.My() = 5);
var arr3 : array of integer;

assert(arr.Test()=7);
var rec : TRec; assert(rec.My()=5);
var arr2 : array of TRec; assert(arr2.My()=5);
end.