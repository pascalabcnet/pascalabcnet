begin
  var o := new object();
  var p: procedure := ()-> begin
    match o with
      integer(o1):;
      double(o2):;
      char(o3):;
    else
      begin
        exit;
      end;
    end;
    assert(false);
  end;
  
  p;
end.