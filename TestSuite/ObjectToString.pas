type
  r1 = record
    public a := 123;
    public b := 'abc';
    internal c := 'err';
    
    public property p: byte read byte(a);
    internal property e: string read 'err';
    
  end;
  
  Base<T1,T2,T3> = class end;
  Derived<T> = class(Base<T, array of T, word>) end;
  
var res := new StringBuilder;
procedure TestO(o: object);
begin
  TypeName(o, res);
  res.Append('{ ');
  _ObjectToString(o, res);
  res.Append(' }'#10);
end;
procedure TestT(t: System.Type);
begin
  TypeToTypeName(t, res);
  res.Append(#10);
end;

begin
  
  TestO(1);
  TestO('abc');
  TestO(new System.UIntPtr(123));
  TestO(System.ConsoleColor.Cyan);
  TestO(Lst(0).GetEnumerator);
  
  TestO(System.Tuple.Create(1,2.3));
  TestO(System.ValueTuple.Create(1,2.3));
  
  TestO(new byte[2,1,1,1,1,1,1,1,2]);
  TestO(new byte[2,1,1,1,0,1,1,1,2]);
  TestO(|1.2,3|.ToList);
  TestO(|1,2,3,4,5|.Skip(1).Take(3));
  TestO(new System.Collections.ArrayList(|6,7,8|)); // Пример НЕ типизированной последовательности
  
  TestO(new r1);
  
  var d1 := procedure(b: byte)->exit();
  var d2: Action<byte> := d1;
  var d3 := function: object->d1;
  var d4: Func0<object> := d3;
  TestO(d1);
  TestO(d2);
  TestO(Delegate(d3));
  TestO(Delegate(d4));
  
  TestT(typeof(Dictionary<,>));
  TestT(typeof(Derived<byte>).BaseType);
  TestT(typeof(Derived<>).BaseType);
  TestT(Lst(0).GetType.GetGenericTypeDefinition);
  TestT(Lst(0).GetEnumerator.GetType.GetGenericTypeDefinition);
  
  begin
    {$reference ObjectToStringExt.dll}
    var t := typeof(OTS.CS.Parent<>).GetNestedTypes.Single;
    TestT(t);
    TestT(t.MakeGenericType(typeof(byte), typeof(word)));
    TestT(t.MakeGenericType(typeof(byte), t.GetGenericArguments[1]));
    TestT(t.MakeGenericType(t.GetGenericArguments[0], typeof(word)));
  end;
  
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