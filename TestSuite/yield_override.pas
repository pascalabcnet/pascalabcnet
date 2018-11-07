type
  t1=abstract class
  public
    //i: integer := 1;
    function f1:sequence of t1; abstract;
  end;
  t2=class(t1)
  public
    function f1:sequence of t1; override;
    begin
      yield nil;
    end;
  end;

begin 
  Assert(t2.Create.f1.First = nil);
end.