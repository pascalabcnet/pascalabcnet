uses vcl,System,System.Collections,System.Text;


function Sum(a, b : integer) : integer;
begin
 Sum := a + b;
end;

var
ht:hashtable;
sb : StringBuilder;
//r : Rectangle;
i : integer;
fr:System.Windows.Forms.Form;
str : string;

begin
i := 10;
str := System.String.Concat(2.ToString,(i+23).ToString);
writeln(str);
ht:=hashtable.Create(10);
ht.Add('Hello world',ht);
i := 45;
ht.Add(i,ht);
if (ht.Contains('Hello world')) then
begin
writeln(1);
end;
if ht.Contains(i) then
begin
 Console.WriteLine('ok');
end;
sb := StringBuilder.Create('Hello ');
sb.Append('w');
sb.Append('o');
Console.WriteLine(sb.ToString);
Console.WriteLine(Sum(4,5).ToString);
i := 4;
Console.WriteLine(i.ToString);

fr:=System.Windows.Forms.Form.Create;
//fr.show;


System.Windows.Forms.Application.Run(fr);

System.Threading.thread.sleep(1000);

writeln('Hello world1');

fr.width:=500;
writeln('Hello world2');
fr.invalidate;
writeln('Hello world3');


end.
