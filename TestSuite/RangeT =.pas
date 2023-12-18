##
Assert( 2..3 = 2..3 );
Assert( 2.0..3 = 2..3.0 );
Assert( 'b'..'c' = 'b'..'c' );

Assert( 2..3 in |2..3|.ToHashSet );
Assert( (2..3) as object in |2..3|.Cast&<object>.ToHashSet );

Assert( 2.0..3 in |2..3.0|.ToHashSet );
Assert( (2.0..3) as object in |2..3.0|.Cast&<object>.ToHashSet );

Assert( 'b'..'c' in |'b'..'c'|.ToHashSet );
Assert( ('b'..'c') as object in |'b'..'c'|.Cast&<object>.ToHashSet );