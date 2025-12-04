uses System.Linq;

begin
  match 'a b c'.Any(x -> x = 'a') as object with
    string(s): Assert(false);
    boolean(b): Assert(b);
    integer(i): Assert(false);
    else Assert(false)
  end;
  
  var count := 0;
  match Arr('aa', 'bb').Select(x -> x = 'aa' ? x : nil).ToList with
    List<string>(l): 
      foreach var x in l do
        if not (x = nil) then
          loop 5 do count += 1;
  end;
  Assert(count = 5);
  
  match 'a b c'.Any(x -> x = 'a') as object with
    string(s): Assert(false);
    boolean(b):
      begin
        var r := Enumerable
        .Repeat(b, 3)
        .Select(
          x -> 
          begin
            match x with
              boolean(b): Result := not b;
            end;
          end);
        Assert(Enumerable.Repeat(false, 3).SequenceEqual(r));
      end;
    integer(i): Assert(false);
    else Assert(false);
  end;
end.