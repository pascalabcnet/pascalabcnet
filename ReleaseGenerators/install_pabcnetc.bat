if exist %windir%\Microsoft.NET\Framework64\v4.0.30319\ngen.exe (
	%windir%\Microsoft.NET\Framework64\v4.0.30319\ngen.exe install pabcnetcclear.exe
	%windir%\Microsoft.NET\Framework64\v4.0.30319\ngen.exe install pabcnetc.exe
	%windir%\Microsoft.NET\Framework64\v4.0.30319\ngen.exe install PascalABCParser.dll
) else (
    %windir%\Microsoft.NET\Framework\v4.0.30319\ngen.exe install pabcnetcclear.exe
	%windir%\Microsoft.NET\Framework\v4.0.30319\ngen.exe install pabcnetc.exe
	%windir%\Microsoft.NET\Framework\v4.0.30319\ngen.exe install PascalABCParser.dll
)