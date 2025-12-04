type
  t1 = class end;
  t2 = class end;
  i1 = interface end;

begin
  var a1: t1;
  var a2: t2;
  var a3: i1;
  
  if a1 is t1(var o) then;     
  if a1 is i1(var o) then;  
  
  if a2 is t2(var o) then;    
  if a2 is i1(var o) then;   
  
  if a3 is i1(var o) then; 
  if a3 is t1(var o) then;
  if a3 is t2(var o) then; 
end.