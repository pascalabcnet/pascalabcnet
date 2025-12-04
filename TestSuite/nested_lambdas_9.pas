procedure p<T>(aa: T); where T: class; 
begin
  var a := Seq(1,2,3).Select(x -> begin 
                                    var tt := new List<T>(); 
                                    writeln(aa);
                                    tt.Add(aa); 
                                    result := tt.Select(y -> begin result := aa; writeln(aa) end).ToList() 
                                  end).ToList();
end;

begin
  p('sss');
end.