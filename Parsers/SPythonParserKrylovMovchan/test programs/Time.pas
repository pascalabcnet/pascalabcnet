unit Time;

interface

function ttime(): real;

implementation

function ttime() : real := DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;

end.