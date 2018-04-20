begin
  var a := new List<integer>(arr(1, 2));
  match a with
    integer(var c): write(1);
    boolean(var s): write(2);
    List<integer>(var l):write(3)
  end;
end.