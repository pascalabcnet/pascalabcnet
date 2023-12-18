##

Assert( (2..3).Count = 2 );
Assert( (3..2).Count = 0 );

Assert( ('b'..'c').Count = 2 );
Assert( ('c'..'b').Count = 0 );

Assert( (2.0..3).Size = 1 );
Assert( (3.0..2).Size = 0 );