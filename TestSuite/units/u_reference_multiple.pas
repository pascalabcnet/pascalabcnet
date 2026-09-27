unit u_reference_multiple;

{$reference 'reference fixtures\Compiler.dll'}
{$reference 'reference fixtures\Errors.dll'}

var
  compilerType: System.Type := typeof(PascalABCCompiler.Compiler);
  compilerError: PascalABCCompiler.Errors.Error;

end.
