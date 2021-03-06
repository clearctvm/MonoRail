// Copyright 2004-2011 Castle Project - http://www.castleproject.org/
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

namespace Castle.MonoRail.Views.Brail
{
	using System;
	using System.Collections;
	using Boo.Lang.Compiler;
	using Boo.Lang.Compiler.Ast;
	using Framework;
	using Macros;

	public class SectionMacro : AbstractAstMacro
	{
		private string componentContextName;
		private string componentVariableName;

		public override Statement Expand(MacroStatement macro)
		{
			if (macro.Arguments.Count == 0)
				throw new MonoRailException("Section must be called with a name");

			var component = GetParentComponent(macro);

			componentContextName = ComponentNaming.GetComponentContextName(component);
			componentVariableName = ComponentNaming.GetComponentNameFor(component);


			var sectionName = macro.Arguments[0].ToString();
			var block = new Block();
			//if (!Component.SupportsSection(section.Name))
			//   throw new ViewComponentException( String.Format("The section '{0}' is not supported by the ViewComponent '{1}'", section.Name, ComponentName));
			var supportsSection = new MethodInvocationExpression(
				AstUtil.CreateReferenceExpression(componentVariableName + ".SupportsSection"),
				new StringLiteralExpression(sectionName));
			//create the new exception
			var raiseSectionNotSupportted = new RaiseStatement(
				new MethodInvocationExpression(
					AstUtil.CreateReferenceExpression(typeof(ViewComponentException).FullName),
					new StringLiteralExpression(
						String.Format("The section '{0}' is not supported by the ViewComponent '{1}'", sectionName,
						              component.Arguments[0])
						)
					));

			var trueBlock = new Block();
			trueBlock.Add(raiseSectionNotSupportted);
			var ifSectionNotSupported =
				new IfStatement(new UnaryExpression(UnaryOperatorType.LogicalNot, supportsSection),
				                trueBlock, null);
			block.Add(ifSectionNotSupported);
			//componentContext.RegisterSection(sectionName);
			var mie = new MethodInvocationExpression(
				new MemberReferenceExpression(new ReferenceExpression(componentContextName), "RegisterSection"),
				new StringLiteralExpression(sectionName),
				CodeBuilderHelper.CreateCallableFromMacroBody(CodeBuilder, macro));
			block.Add(mie);

			var sections = (IDictionary) component["sections"];
			if (sections == null)
			{
				component["sections"] = sections = new Hashtable();
			}
			sections.Add(sectionName, block);
			return null;
		}

		private static MacroStatement GetParentComponent(Node macro)
		{
			var parent = macro.ParentNode;
			while(!(parent is MacroStatement))
			{
				parent = parent.ParentNode;
			}
			var parentComponent = (MacroStatement) parent;
			if (parentComponent == null ||
			    parentComponent.Name.ToLowerInvariant() != "component")
			{
				throw new MonoRailException("A section must be contained in a component");
			}
			return parentComponent;
		}
	}
}