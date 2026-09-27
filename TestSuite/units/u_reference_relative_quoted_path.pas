unit u_reference_relative_quoted_path;

{$reference 'reference fixtures\Compiler.dll'}

var
  compilerType: System.Type := typeof(PascalABCCompiler.Compiler);
  compilerInstance: PascalABCCompiler.Compiler;

end.
