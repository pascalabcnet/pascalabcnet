type Tuple3 = (integer,real);

begin
var i: Tuple3{@type Tuple3 = (integer,real)@};
writeln(i.Item2{@property Tuple<>.Item2: real; readonly;@});
end.