type A = class
  constructor;
  begin

  end;
  function Clone(): A;
  begin
    Result := self.Create;
  end;
end;

begin

end.