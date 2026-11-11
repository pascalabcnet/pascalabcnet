unit ReexportTestOrigin;

interface

type int = integer;
const InitialValue = 21;
var SharedValue: integer;

function Twice(n: integer): integer;
function ReadSharedValue: integer;

implementation

function Twice(n: integer): integer := 2 * n;
function ReadSharedValue: integer := SharedValue;

end.
