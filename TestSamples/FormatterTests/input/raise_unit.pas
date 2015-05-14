unit raise_unit;

procedure Test;
var j:integer;
begin
 try
   var i:integer; 
   write(j div i);
 except
   on System.Exception do raise;
 end;
 Write('done');
end;

end.