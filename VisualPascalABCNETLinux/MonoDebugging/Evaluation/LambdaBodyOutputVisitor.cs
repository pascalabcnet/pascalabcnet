using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Mono.Debugging.Client;

namespace Mono.Debugging.Evaluation
{
	// Outputs lambda expression inputted on Immediate Pad. Also
	// this class works for followings.
	// - Check if body of lambda has not-supported expression
	// - Output reserved words like `this` or `base` with generated
	//   identifer.
	// - Collect variable references for outside the lambda
	//   (local variables/properties...)
	/*public class LambdaBodyOutputVisitor : CSharpSyntaxWalker
	{
		readonly Dictionary<string, ValueReference> userVariables;
		readonly EvaluationContext ctx;

		Dictionary<string, Tuple<string, object>> localValues;
		List<string> definedIdentifier;
		int gensymCount;

		public LambdaBodyOutputVisitor (EvaluationContext ctx, Dictionary<string, ValueReference> userVariables)
		{
			this.ctx = ctx;
			this.userVariables = userVariables;
			this.localValues = new Dictionary<string, Tuple<string, object>> ();
			this.definedIdentifier = new List<string> ();
		}

		public Tuple<string, object>[] GetLocalValues ()
		{
			var locals = new Tuple<string, object>[localValues.Count];
			int n = 0;
			foreach (var localv in localValues.Values) {
				locals[n] = localv;
				n++;
			}
			return locals;
		}

		static Exception NotSupported ()
		{
			return new NotSupportedExpressionException ();
		}

		static Exception EvaluationError (string message, params object[] args)
		{
			return new EvaluatorException (message, args);
		}

		bool IsPublicValueFlag (ObjectValueFlags flags)
		{
			var isField = (flags & ObjectValueFlags.Field) != 0;
			var isProperty = (flags & ObjectValueFlags.Property) != 0;
			var isPublic = (flags & ObjectValueFlags.Public) != 0;

			return !(isField || isProperty) || isPublic;
		}

		void AssertPublicType (object type)
		{
			if (!ctx.Adapter.IsPublic (ctx, type)) {
				var typeName = ctx.Adapter.GetDisplayTypeName (ctx, type);
				throw EvaluationError ("Not Support to reference non-public type: `{0}'", typeName);
			}
		}

		void AssertPublicValueReference (ValueReference vr)
		{
			if (!(vr is NamespaceValueReference)) {
				var typ = vr.Type;
				AssertPublicType (typ);
			}
			if (!IsPublicValueFlag (vr.Flags)) {
				throw EvaluationError ("Not Support to reference non-public thing: `{0}'", vr.Name);
			}
		}

		ValueReference Evaluate (IdentifierNameSyntax t)
		{
			var visitor = new NRefactoryExpressionEvaluatorVisitor (ctx, t.Identifier.ValueText, null, userVariables);
			return visitor.Visit (t);
		}

		ValueReference Evaluate (BaseExpressionSyntax t)
		{
			var visitor = new NRefactoryExpressionEvaluatorVisitor (ctx, "base", null, userVariables);
			return visitor.Visit (t);
		}

		ValueReference Evaluate (ThisExpressionSyntax t)
		{
			var visitor = new NRefactoryExpressionEvaluatorVisitor (ctx, "this", null, userVariables);
			return visitor.Visit (t);
		}

		string GenerateSymbol (string s)
		{
			var prefix = "__" + s;
			var sym = prefix;
			while (ExistsLocalName (sym)) {
				sym = prefix + gensymCount++;
			}

			return sym;
		}

		string AddToLocals (string name, ValueReference vr, bool shouldRename = false)
		{
			if (localValues.ContainsKey (name))
				return GetLocalName (name);

			string localName;
			if (shouldRename) {
				localName = GenerateSymbol (name);
			} else if (!ExistsLocalName (name)) {
				localName = name;
			} else {
				throw EvaluationError ("Cannot use a variable named {0} inside lambda", name);
			}

			AssertPublicValueReference (vr);

			if (!(vr is NamespaceValueReference)) {
				var valu = vr?.Value;
				var pair = Tuple.Create(localName, valu);
				localValues.Add(name, pair);
			}
			return localName;
		}

		string GetLocalName (string name)
		{
			Tuple<string, object> pair;
			if (localValues.TryGetValue (name, out pair))
				return pair.Item1;
			return null;
		}

		bool ExistsLocalName (string localName)
		{
			foreach (var pair in localValues.Values) {
				if (pair.Item1 == localName)
					return true;
			}
			return definedIdentifier.Contains (localName);
		}

		#region IAstVisitor implementation

		public override void VisitAssignmentExpression (AssignmentExpressionSyntax node)
		{
			throw EvaluationError ("Not support assignment expression inside lambda");
		}

		public override void VisitBaseExpression (BaseExpressionSyntax node)
		{
			var basee = "base";
			var localbase = GetLocalName (basee);
			if (localbase == null) {
				var vr = Evaluate (node);
				localbase = AddToLocals (basee, vr, true);
			}
		}

		public override void VisitIdentifierName (IdentifierNameSyntax node)
		{
			var identifier = node.Identifier.ValueText;
			var localIdentifier = "";

			if (definedIdentifier.Contains (identifier)) {
				localIdentifier = identifier;
			} else {
				localIdentifier = GetLocalName (identifier);
				if (localIdentifier == null) {
					var vr = Evaluate (node);
					localIdentifier = AddToLocals (identifier, vr);
				}
			}
		}

		public override void VisitGenericName (GenericNameSyntax node)
		{
			foreach (var arg in node.TypeArgumentList.Arguments) {
				Visit (arg);
			}
		}

		public override void VisitInvocationExpression (InvocationExpressionSyntax node)
		{
			var invocationTarget = node.Expression;
			if (!(invocationTarget is IdentifierNameSyntax method)) {
				Visit(invocationTarget);
				return;
			}

			var argc = node.ArgumentList.Arguments.Count;
			var methodName = method.Identifier.ValueText;
			var vref = ctx.Adapter.GetThisReference (ctx);
			var vtype = ctx.Adapter.GetEnclosingType (ctx);
			string accessor = null;

			var hasInstanceMethod = ctx.Adapter.HasMethodWithParamLength (ctx, vtype, methodName, BindingFlags.Instance, argc);
			var hasStaticMethod = ctx.Adapter.HasMethodWithParamLength (ctx, vtype, methodName, BindingFlags.Static, argc);
			var publicFlags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static;
			var hasPublicMethod = ctx.Adapter.HasMethodWithParamLength (ctx, vtype, methodName, publicFlags, argc);

			if ((hasInstanceMethod || hasStaticMethod) && !hasPublicMethod)
				throw EvaluationError ("Only support public method invocation inside lambda");

			if (vref == null && hasStaticMethod) {
				AssertPublicType (vtype);
				var typeName = ctx.Adapter.GetTypeName (ctx, vtype);
				accessor = ctx.Adapter.GetDisplayTypeName (typeName);
			} else if (vref != null) {
				AssertPublicValueReference (vref);
				if (hasInstanceMethod) {
					if (hasStaticMethod) {
						// It's hard to determine which one is expected because
						// we don't have any information of parameter types now.
						throw EvaluationError ("Not supported invocation of static/instance overloaded method");
					}
					accessor = GetLocalName ("this");
					if (accessor == null)
						accessor = AddToLocals ("this", vref, true);
				} else if (hasStaticMethod) {
					var typeName = ctx.Adapter.GetTypeName (ctx, vtype);
					accessor = ctx.Adapter.GetDisplayTypeName (typeName);
				}
			}

			for (int i = 0; i < node.ArgumentList.Arguments.Count; i++) {
				Visit (node.ArgumentList.Arguments[i]);
			}
		}

		public override void VisitSimpleLambdaExpression (SimpleLambdaExpressionSyntax node)
		{
			if (node.Parameter.Modifiers.Count > 0)
				throw NotSupported ();

			definedIdentifier.Add (node.Parameter.Identifier.ValueText);
			Visit (node.Body);
		}

		public override void VisitThisExpression (ThisExpressionSyntax node)
		{
			var thiss = "this";
			var localthis = GetLocalName (thiss);
			if (localthis == null) {
				var vr = Evaluate (node);
				localthis = AddToLocals (thiss, vr, true);
			}
		}
		#endregion
	}*/
}
