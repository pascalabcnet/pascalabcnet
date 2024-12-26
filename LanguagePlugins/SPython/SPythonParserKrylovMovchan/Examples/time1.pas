unit Time1;

interface

function time(): real;

function forty_two(): real;

implementation

function time() : real := DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;

function forty_two() : real := 42;

end.