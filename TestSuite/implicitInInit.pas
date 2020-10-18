type
  MyString = class
  public  
    d: string;
    static function operator implicit(s: string): MyString;
    begin
      Result := new MyString;
      Result.d := s;
    end;
    
  end;
  
begin
  var s: MyString := 'abcde';
  Assert(s.d = 'abcde');
end.