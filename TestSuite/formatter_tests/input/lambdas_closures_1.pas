begin
  var xx := 1;
  var l := new List<integer>;
  for var i := 1 to 5 do
    l.Add(i);
  begin
    var b := 5;
    begin
      begin
        var ll := l.Select(x -> begin  
                                  b += 1;
                                  xx *= 2;
                                  result := b + x + xx 
                                end).ToList;
        assert(b = 10);
        assert(xx = 32);
        assert(ll.Contains(9));
        assert(ll.Contains(13));
        assert(ll.Contains(19));
        assert(ll.Contains(29));
        assert(ll.Contains(47));
      end;
    end;
  end;
end.