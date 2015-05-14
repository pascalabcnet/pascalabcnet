type
  pnode=^node;
  node=record
    y:integer;
    procedure zz;
    var pp:pnode;
    begin
      pp^.y:=10;
    end;
  end;

begin  
end.