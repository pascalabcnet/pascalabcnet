type Rec = record
  i: integer;
end;

function f1: sequence of word;
begin
  var r: Rec := (i: 2);
  //var a: array of array of integer := ((1,2),(3));
  yield 0;
end;

begin 
  
end.