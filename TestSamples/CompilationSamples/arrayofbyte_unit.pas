unit arrayofbyte_unit;


var
  BFField:array of byte;  


procedure Init;
begin
  SetLength(BFField,100);
end;


procedure IncValue;
begin
  BFField[0]:=BFField[0]+1;
end;



end.
