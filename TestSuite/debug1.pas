begin
var i: integer;
{$IFDEF DEBUG}
i := 2;
{$ENDIF}
{$IFDEF RELEASE}
i := 3;
{$ENDIF}
assert(i = {$IFDEF DEBUG}2{$ELSE}3{$ENDIF});
end.