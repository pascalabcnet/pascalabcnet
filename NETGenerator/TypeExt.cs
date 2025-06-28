using System;

namespace PascalABCCompiler.NETGenerator
{
	internal static class TypeExt
	{
		/// <summary>
		/// Повторяет работу свойства System.Type.IsConstructedGenericType из net fx 4.5
		/// </summary>
		public static bool IsConstructedGenericType(this Type t)
		{
			return t.IsGenericType && !t.IsGenericTypeDefinition;
		}
	}
}
