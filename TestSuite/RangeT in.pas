##

var ri := 2..3;
Assert( 1 not in ri );
Assert( 2 in ri );
Assert( 2.5 in ri );
Assert( 3 in ri );
Assert( 4 not in ri );

var rc := 'b'..'c';
Assert( 'a' not in rc );
Assert( 'b' in rc );
Assert( 'c' in rc );
Assert( 'd' not in rc );

var rr := 2.0..3;
Assert( 1 not in rr );
Assert( 2 in rr );
Assert( 2.5 in rr );
Assert( 3 in rr );
Assert( 4 not in rr );

Assert( 2 in 2.0..2 );