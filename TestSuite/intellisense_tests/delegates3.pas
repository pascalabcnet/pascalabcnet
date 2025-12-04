begin
  var d: procedure := procedure->Sleep(1000);
  d.BeginInvoke{@function BeginInvoke(callback: AsyncCallback; object: Object): IAsyncResult; virtual;@}(nil,nil);
  d.EndInvoke{@procedure EndInvoke(result: IAsyncResult); virtual;@}(nil);
end.