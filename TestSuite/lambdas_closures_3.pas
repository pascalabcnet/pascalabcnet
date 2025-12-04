begin
  var xx := 1;
  var l := new List<integer>;
  l.Add(1);
  begin
    var b := 1;
    begin
      begin
        var ll := l.Select(x -> begin xx += 1; result := x + xx end).ToList();
        var t := l.Where(x -> begin b += 1; result := x > b end).ToList;
        
        assert(b = 2);
        assert(xx = 2);
        assert(ll.First = 3);
        assert(t.Count = 0);
      end;
    end;
  end;
end.