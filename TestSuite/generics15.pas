type
  t1<T>=class
    class l: byte;
    a: byte;
    class procedure test;
    begin
      t1&<T>.l := 3;
      assert(t1&<T>.l = 3);
    end;
    class procedure test(var a: byte);
    begin
      assert(a = 3);
    end;
  end;

begin
  t1&<byte>.l := 2;
  t1&<char>.l := 3;
  assert(t1&<byte>.l = 2);
  assert(t1&<char>.l = 3);
  var o := new t1<byte>;
  o.a := 2;
  assert(o.a = 2);
  t1&<char>.test;
  t1&<char>.test(t1&<char>.l);
end.