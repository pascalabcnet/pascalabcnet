
 unit cycle_unit2;

interface

type TClass = class
	public
	constructor create;
     begin
       writeln('tclass.create');
     end;
	procedure Hello;
	end;

implementation

uses cycle_unit;

procedure TClass.Hello;
begin
 SayHello2;
end;

end.
