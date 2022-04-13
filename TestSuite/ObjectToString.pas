type
  r1 = record
    public a := 123;
    public b := 'abc';
    internal c := 'err';
    
    public property p: byte read byte(a);
    internal property e: string read 'err';
    
  end;
  
var res := new StringBuilder;

procedure Test(o: object);
begin
  TypeName(o, res);
  res += '{ ';
  _ObjectToString(o, res);
  res += ' }'#10;
end;

begin
  
  Test(1);
  Test('abc');
  Test(new System.UIntPtr(123));
  
  Test(new byte[2,1,1,1,1,1,1,1,2]);
  Test(new byte[2,1,1,1,0,1,1,1,2]);
  
  Test(new r1);
  
  var d1 := procedure(b: byte)->exit();
  var d2 := function: object->d1;
  Test(d1);
  Test(Action&<byte>(d1));
  Test(d2);
  Test(Func0&<object>(d2));
  
  var otp_fname := 'ObjectToString.txt';
  // Убрать одну звёздочку, если надо поменять содержимое вывода
  (**)
  Assert(ReadAllText('..\'+otp_fname) = res.ToString);
  (*)
  WriteAllText(otp_fname, res.ToString);
  (**)
  
end.