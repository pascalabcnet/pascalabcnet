
uses System, System.Text, System.IO, System.Reflection,System.Threading;

function SomeFunc : integer;
begin
 Console.WriteLine('Hello from SomeFunc');
end;

procedure SomeProc(a : integer; var b : real);
begin
end;

var f : FileInfo;
    writer : StreamWriter;
    t : System.Type;
    curDomain : AppDomain;
    a : Assembly;
    meth : MethodInfo;
    sb : StringBuilder;
    obj : System.Object;
    i : integer;
    s:string;
    num:integer;

procedure out_state;
begin
 num:=num+1;
 writeln(s+num.ToString);
end;

begin
 num:=0;
 s:='OK! State num: ';
 sb := StringBuilder.Create;
 //out_state;
 sb.Append(Environment.CurrentDirectory);
 //out_state;
 sb.Append('\');
 //out_state;
 sb.Append('file_test.exe');
 //out_state;
 curDomain := Thread.GetDomain;
 //out_state;
 a := Assembly.LoadFrom(sb.ToString);
 //out_state;
 t := a.GetType('MainProgram.MainClass');
 //out_state;
 meth := t.GetMethod('SomeFunc');
 //out_state;
 obj := Activator.CreateInstance(t);
 //out_state;
 f := FileInfo.Create('Disasm.txt');
 //out_state;
 writer := f.CreateText;
 //out_state;
 writer.WriteLine('Program disassembly');
 //out_state;
 for i := 0 to t.GetMethods.Length do
 begin 
  //out_state;
  writer.Write(t.GetMethods.GetValue(i).ToString);
  //out_state;
  writer.WriteLine(' {...}');
  //out_state;
  writer.WriteLine;
  //out_state;
 end;
 //out_state;
 writer.Close;
 //out_state;
 writeln('OK');
end.
