type
  t1 = class
    function p1:byte; virtual := 1;
  end;
  t2 = class(t1)
    function p1:byte; override;
    begin
      var b:byte := inherited p1;
      Result := b;
    end;
  end;

begin 
  var o := new t2;
  assert(o.p1 = 1);
end.