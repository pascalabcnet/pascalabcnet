begin
  match nil with
    List<integer>(s): Assert(false);
    integer(i): Assert(false);
  end;
end.