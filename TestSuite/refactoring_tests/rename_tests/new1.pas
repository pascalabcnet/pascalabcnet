type
  {!}MyEx = class(Exception)
    
    public constructor(o: object; s: string) := inherited Create(s);
    
  end;

begin
  raise new {@}MyEx;
end.