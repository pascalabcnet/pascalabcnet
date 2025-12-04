begin
  var s := 0;
  for var i := 1 to 5 do
    for var j := 1 to 5 do
      for var k := 1 to 5 do
        for var l := 1 to 5 do
          for var m := 1 to 5 do
            for var n := 1 to 5 do
              for var o := 1 to 5 do
                for var p := 1 to 5 do
                  for var q := 1 to 5 do
                    for var r := 1 to 5 do
                      Inc(s);
  assert(s = Power(5, 10));
end.