begin
  var t := typeof(object);
  var o := t.GetConstructor(System.Type.EmptyTypes).Invoke{@function ConstructorInfo.Invoke(parameters: array of Object): System.Object;@}(new object[0]);
end.