##

Assert( (2..4).Step(2).SequenceEqual(|2,4|) );
Assert( ('b'..'d').Step(2).SequenceEqual('bd') );

Assert( (2..3).Reverse.SequenceEqual(|3,2|) );
Assert( ('b'..'c').Reverse.SequenceEqual('cb') );