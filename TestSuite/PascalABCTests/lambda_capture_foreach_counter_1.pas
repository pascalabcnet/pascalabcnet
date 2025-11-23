begin
  Arr(0).Select(
    i->
    begin
      Result := 0;
      foreach var i2 in Arr(1,2) do
      begin
        var a := i2;
      end;
    end
  );
end.