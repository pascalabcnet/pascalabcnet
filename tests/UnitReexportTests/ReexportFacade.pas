unit ReexportFacade;

interface

uses ReexportOrigin;

export ReexportOrigin.F1, ReexportOrigin.F2 as FFF2;
export ReexportOrigin.TOriginPoint as PublicPoint, ReexportOrigin.TOriginAlias as PublicAlias,
       ReexportOrigin.TOriginBox as PublicBox;
export ReexportOrigin.OriginCount as PublicCount, ReexportOrigin.UnchangedConstant,
       ReexportOrigin.OriginCounter as Counter;
export ReexportOrigin.BumpPoint, ReexportOrigin.BoxValue, ReexportOrigin.ReadOriginCounter;
export ReexportOrigin.Identity as Same, ReexportOrigin.Pick as SelectItem;

implementation

end.
