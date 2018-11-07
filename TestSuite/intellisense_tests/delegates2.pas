procedure A1(x: procedure) := x.DynamicInvoke{@function Delegate.DynamicInvoke(params args: array of Object): System.Object;@}();
procedure A2(x: procedure) := x.Invoke{@procedure Invoke(); virtual;@}();

function B1(x: function: byte) := x.DynamicInvoke{@function Delegate.DynamicInvoke(params args: array of Object): System.Object;@}();
function B2(x: function: byte) := x.Invoke{@function Invoke(): byte; virtual;@}();

function D1(x: Func0<byte>) := x.DynamicInvoke{@function Delegate.DynamicInvoke(params args: array of System.Object): System.Object;@}();
function D2(x: Func0<byte>) := x.Invoke{@function Func<>.Invoke(): byte; virtual;@}();

begin

end.