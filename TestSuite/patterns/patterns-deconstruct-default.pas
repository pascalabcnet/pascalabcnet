begin
  match 'asd' with
    string(s): 
      match s as object with
        integer(i): Assert(false);
        string(s1) when s.Length > 1: ;
        else Assert(false);
      end;
  end;
  
  match 'asd' as object with
    integer(i): Assert(false);
    string(s1) when s1.Length > 3: Assert(false);
  end;
end.