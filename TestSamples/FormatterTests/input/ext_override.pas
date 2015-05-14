
type first=class
public
constructor create;
begin
 writeln('first created');
end;

function tostring:string;override;
begin
 tostring:='overrided by first tostring method';
end;

end;

var
f:first;

begin

f:=first.create;

writeln(f.tostring);

writeln('END TEST');

System.console.readline;

end.
