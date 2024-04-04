## var r := 2..3;
Assert( r.SequenceEqual(|2,3|) );
Assert( r.ToArray.SequenceEqual(|2,3|) );
Assert( r.AsEnumerable.ToArray.SequenceEqual(|2,3|) );
var r2 := 'b'..'c';
Assert( r2.SequenceEqual('bc') );
Assert( r2.ToArray.SequenceEqual('bc') );
Assert( r2.AsEnumerable.ToArray.SequenceEqual('bc') );