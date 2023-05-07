using System;
using Mono.Debugging.Evaluation;
using Mono.Debugger.Soft;

namespace Mono.Debugging.Soft
{
	public abstract class SoftValueReference: ValueReference
	{
		public SoftValueReference (EvaluationContext ctx) : base(ctx)
		{
		}

		protected void EnsureContextHasDomain (AppDomainMirror domain)
		{
			var softEvaluationContext = (SoftEvaluationContext)Context;
			if (softEvaluationContext.Domain == domain)
				return;

			var clone = (SoftEvaluationContext)softEvaluationContext.Clone ();
			clone.Domain = domain;
			Context = clone;
		}
	}
}

