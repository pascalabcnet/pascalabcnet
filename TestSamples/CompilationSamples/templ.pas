
type 
  Adder<T> = template class
    class function Add(params args: array of T): T;
    var
      i: integer;
      s: T;
    begin
      result := args[0];
      for i:=1 to args.Length-1 do
        result := result + args[i];
    end;
  end;

type 
  IntAdder = Adder<integer>;
  StringAdder = Adder<string>;

begin
  Writeln(IntAdder.Add(1,2,3,4,5,6,7,8,9));
  Writeln(StringAdder.Add('str1 ','str2 ','str3 ','str4 ','str5'));
  Readln;
end.