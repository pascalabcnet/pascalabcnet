begin
  var xx := 1;
  var l := new List<integer>;
  for var i := 1 to 5 do
  begin
    l.Add(i);
  end;
  begin
    var b := 1;
    begin
      begin
        var ll := l.Select(x -> begin b += 1; result := b + x + xx end).ToList();
        xx :=7;
        begin
          xx += 1;
        end;
        var t := l.Where(x -> begin xx += 1; result := x < xx end).ToList;
        writeln(b);
        writeln(xx);
        ll.Print;
        writeln();
        t.Print;
        
        assert(b = 6);
        assert(xx = 13);
        assert(ll.Contains(4));
        assert(ll.Contains(6));
        assert(ll.Contains(8));
        assert(ll.Contains(10));
        assert(ll.Contains(12));
        assert(t.Count = 5);
        assert(t.Contains(1));
        assert(t.Contains(2));
        assert(t.Contains(3));
        assert(t.Contains(4));
        assert(t.Contains(5));
      end;
    end;
  end;
end.