var global: integer;

type int = integer;

type Class1<T,T1> = class
  classfield: integer;
  const cc = 2;
  function p<PPT>(pa,pb: real): integer;
  type int1 = integer;
  var
    A: record
      i: array of integer := nil{cc {classfield};
      jlocal1: record
        j: integer;
        mostinternal1: set of integer := [j];
      end;
      klocal1: int := global {* classfield};
    end;
  begin
    
  end;  
end;  
  
begin
  Assert(True);
end.