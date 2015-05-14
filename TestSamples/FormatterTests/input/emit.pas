uses System,System.Reflection,System.Reflection.Emit;

var ad : AppDomain;
    ab : AssemblyBuilder;
    mb : ModuleBuilder;
    tb : TypeBuilder;
    methodb : MethodBuilder;
    &il : ILGenerator;
    an : AssemblyName;
    ta : TypeAttributes;
    a : Assembly;
    int_type : &Type;
    lb1 : LocalBuilder;
    lb2 : LocalBuilder;
    
begin
writeln(2);
  a := Assembly.Load('mscorlib');
  int_type := typeof(integer);
  ad := System.Threading.Thread.GetDomain;
  an := AssemblyName.Create;
  an.Name := 'emit_output.exe';
  ab := ad.DefineDynamicAssembly(an,AssemblyBuilderAccess.Save);
  mb := ab.DefineDynamicModule('output',an.Name,false);
  
  tb := mb.DefineType('MainNamespace.MainClass',ta);
  methodb := tb.DefineMethod('Main',MethodAttributes.Static) ;
  &il := methodb.GetILGenerator();
  lb1 := &il.DeclareLocal(int_type);
  lb2 := &il.DeclareLocal(int_type);
  &il.Emit(OpCodes.Ldc_I4_3);
  &il.Emit(OpCodes.Stloc,lb1);
  &il.Emit(OpCodes.Ldc_I4,1000);
  &il.Emit(OpCodes.Stloc,lb2);
  &il.Emit(OpCodes.Ldloc,lb1);
  &il.Emit(OpCodes.Ldloc,lb2);
  &il.Emit(OpCodes.Add);
  &il.Emit(OpCodes.Stloc,lb1);
  &il.Emit(OpCodes.Ret);
    tb.CreateType;
  tb := mb.DefineType('IAssemblyName',TypeAttributes.Interface or TypeAttributes.Abstract or TypeAttributes.Import or TypeAttributes.Public);
  tb.CreateType();

  ab.SetEntryPoint(methodb);
  ab.Save(an.Name);
end.