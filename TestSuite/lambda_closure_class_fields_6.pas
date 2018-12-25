procedure Invoke(p: procedure);
begin end;

type 
WH = class
  Width,Height: real;
end;
C = class
  gr: WH;
end;

B = class(C)
    property Width: real 
      write Invoke(procedure->begin gr.Width := value end); 
end;

begin
  Assert(1=1)
end.