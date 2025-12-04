type t1 = class
    function test: sequence of integer;
    begin
      Result := true?
        Seq(0).Where(m->begin
        var temp:byte;
        writeln(temp{@var temp: byte;@});
        Result := true;
        end):
        Seq(0);
    end;
  end;

begin end.