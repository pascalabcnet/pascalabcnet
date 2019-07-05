namespace nstypedef;

type 
  myint2 = integer;
  myfnc = function(o: TClass): integer;
  
type TClass = class
  f: myint2;
end;

end.