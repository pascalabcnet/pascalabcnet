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
  res.Append('{ ');
  _ObjectToString(o, res);
  res.Append(' }'#10);
end;

begin
  
  Test(1);
  Test('abc');
  Test(new System.UIntPtr(123));
  Test(System.ConsoleColor.Cyan);
  
  Test(new byte[2,1,1,1,1,1,1,1,2]);
  Test(new byte[2,1,1,1,0,1,1,1,2]);
  Test(|1.2,3|.ToList);
  Test(|1,2,3,4,5|.Skip(1).Take(3));
  Test(new System.Collections.ArrayList(|6,7,8|)); // Пример НЕ типизированной последовательности
  
  Test(new r1);
  
  var d1 := procedure(b: byte)->exit();
  var d2: Action<byte> := d1;
  var d3 := function: object->d1;
  var d4: Func0<object> := d3;
  Test(d1);
  Test(d2);
  Test(Delegate(d3));
  Test(Delegate(d4));
  
  var otp_fname := 'ObjectToString.txt';
  var otp_new_fname := 'ObjectToString[new].txt';
  var enc := new System.Text.UTF8Encoding(true);
  
  var dir := GetCurrentDir;
  if System.IO.Path.GetFileName(dir)='exe' then
    dir := System.IO.Path.GetDirectoryName(dir);
  
  var expected := ReadAllText(dir+'\'+otp_fname, enc).Remove(#13);
  var curr := res.ToString;
  var tr := expected = curr;
  if not tr then WriteAllText(dir+'\'+otp_new_fname, curr, enc);
  Assert(tr);
end.