type
  TClass = class
    public const b: array of string = ('abc','def');
    public const s: string = 'tt';
  end;

begin
  assert(TClass.b[0] = 'abc');
  assert(TClass.b[1] = 'def');
  assert(TClass.s = 'tt');
end.