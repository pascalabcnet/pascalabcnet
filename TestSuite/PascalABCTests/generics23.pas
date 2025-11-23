type
  TSome = record 
    fX: integer := 1;
    function Method() := fX; 
  end;

function CastTo<T>(reference: object) := T(reference); 

begin
  assert(CastTo&<TSome>(new TSome()).Method() = 1);
end.