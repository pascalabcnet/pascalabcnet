unit events2u;

interface

event
  ev1: procedure;
  ev2: function(i: integer): integer;
  ev3: function: char;

procedure RaiseEvent1;
function RaiseEvent2(i: integer): integer;
function RaiseEvent3: char;

implementation

function RaiseEvent2(i: integer): integer;
begin
  Result := ev2(i);
end;

procedure RaiseEvent1;
begin
  ev1;
end;

function RaiseEvent3: char;
begin
  Result := ev3;
end;

end.