type
  PNode = ^TNode;
  TNode = record
    Next: PNode;
  end;

begin
  var t:tnode;
  var p:=@t;
  writeln(p);
  new(p);
  writeln(p);
end.