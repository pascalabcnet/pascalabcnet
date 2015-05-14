#reference 'System.Windows.Forms.dll'
uses System.Windows.Forms,System.Reflection;

{type 
  TMainForm = class(Form)
    procedure CenterToScreen;
    begin
      inherited CenterToScreen; 
    end;
  end;}

var mi:array of MemberInfo;
    i:=0;
begin
  mi:=typeof(form).GetMembers();
  for i:=0 to mi.length-1 do
    writeln(mi[i]);
  readln
end.