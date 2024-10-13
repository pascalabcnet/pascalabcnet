unit Time1;

interface

function time(): real;

implementation

function time() : real := DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;

end.