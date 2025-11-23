var b: boolean := default(System.Nullable<byte>) = byte(0);
begin
  assert(default(System.Nullable<byte>) = default(System.Nullable<byte>));
  assert(default(System.Nullable<byte>) <> byte(0));
  assert(default(System.Nullable<byte>) = nil);
  assert(nil = default(System.Nullable<byte>));
  assert(byte(0) <> default(System.Nullable<byte>));
  assert(not b);
end.