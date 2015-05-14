type
  interf = interface
    procedure proc;
  end;
  
  rec=record(interf)
  public
    y:integer;
    procedure proc; //virtual;
    begin
      writeln(y, ' rec.proc was called');
    end; 
  end;

var
  r:rec;
  i: interf;
  o: object;
  
begin
  r.y:=11;
  r.proc;
  i := r;
  i := r;
  i.proc;
  readln;
end.