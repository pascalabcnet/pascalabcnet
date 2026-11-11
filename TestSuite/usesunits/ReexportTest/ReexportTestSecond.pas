unit ReexportTestSecond;

interface

uses ReexportTestOrigin;

export ReexportTestOrigin.Twice as DoubleIt, ReexportTestOrigin.int,
       ReexportTestOrigin.InitialValue, ReexportTestOrigin.SharedValue,
       ReexportTestOrigin.ReadSharedValue;

implementation

end.
