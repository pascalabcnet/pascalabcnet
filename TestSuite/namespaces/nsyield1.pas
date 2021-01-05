namespace nsyield1;

type
  T1 = class
    public static function Seq(): sequence of integer;
    begin
      yield 1;
    end;
  end;


end.