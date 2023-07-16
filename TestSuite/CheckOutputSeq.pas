procedure CheckOutputSeq(a: sequence of char); begin end;

procedure CheckOutputSeq(a: sequence of object); begin end;

begin
  var a := ArrFill(10,'*');
  CheckOutputSeq(a);
end.