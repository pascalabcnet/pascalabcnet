uses vcl;

var
f:Form;

procedure eh(sender:object; ea:System.EventArgs);
begin
writeln('Click '+ea.Tostring);
end;

begin
f:=new Form;
f.Click+=eh;
f.ShowDialog;
end.