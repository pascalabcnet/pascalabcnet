##

Assert( (2..2).IsEmpty = false );
Assert( (2..3).IsEmpty = false );
Assert( (3..2).IsEmpty = true );

Assert( ('b'..'b').IsEmpty = false );
Assert( ('b'..'c').IsEmpty = false );
Assert( ('c'..'b').IsEmpty = true );

Assert( (2.0..2).IsEmpty = false );
Assert( (2.0..3).IsEmpty = false );
Assert( (3.0..2).IsEmpty = true );