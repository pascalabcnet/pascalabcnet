// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using PascalABCCompiler.Contracts;
using System.Windows.Forms;

namespace VisualPascalABC
{
    public class CodeCompletionImagesProvider
    {
        ImageList images = new ImageList();
        public int IconNumberMethod = -1;
        public int IconNumberField = -1;
        public int IconNumberProperty = -1;
        public int IconNumberEvent = -1;
        public int IconNumberClass = -1;
        public int IconNumberNamespace = -1;
        public int IconNumberInterface = -1;
        public int IconNumberStruct = -1;
        public int IconNumberDelegate = -1;
        public int IconNumberEnum = -1;
        public int IconNumberLocal = -1;
        public int IconNumberConstant = -1;
        public int IconNumberPrivateField = -1;
        public int IconNumberPrivateMethod = -1;
        public int IconNumberPrivateProperty = -1;
        public int IconNumberProtectedField = -1;
        public int IconNumberProtectedMethod = -1;
        public int IconNumberProtectedProperty = -1;
        public int IconNumberPrivateEvent = -1;
        public int IconNumberProtectedEvent = -1;
        public int IconNumberPrivateDelegate = -1;
        public int IconNumberProtectedDelegate = -1;
        public int IconNumberUnitNamespace = -1;
        public int IconNumberInternalField = -1;
        public int IconNumberInternalMethod = -1;
        public int IconNumberInternalProperty = -1;
        public int IconNumberInternalEvent = -1;
        public int IconNumberInternalDelegate = -1;
        public int IconNumberGotoText = -1;
        public int IconNumberPrivateConstant = -1;
        public int IconNumberProtectedConstant = -1;
        public int IconNumberInternalConstant = -1;
        public int IconNumberKeyword = -1;
        public int IconNumberEvalError = -1;
        public int IconNumberExtensionMethod = -1;

        int AddImageFromManifestResource(string ResName)
        {
            images.Images.Add(new System.Drawing.Bitmap(System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("VisualPascalABC.Resources." + ResName)));
            return images.Images.Count - 1;
        }

        public ImageList ImageList
        {
            get
            {
                return images;
            }
        }

        public CodeCompletionImagesProvider()
        {
            images.ColorDepth = ColorDepth.Depth32Bit;
            IconNumberMethod = AddImageFromManifestResource("Icons.16x16.Method.png");
            IconNumberField = AddImageFromManifestResource("Icons.16x16.Field.png");
            IconNumberProperty = AddImageFromManifestResource("Icons.16x16.Property.png");
            IconNumberEvent = AddImageFromManifestResource("Icons.16x16.Event.png");
            IconNumberClass = AddImageFromManifestResource("Icons.16x16.Class.png");
            IconNumberNamespace = AddImageFromManifestResource("Icons.16x16.NameSpace.png");
            IconNumberInterface = AddImageFromManifestResource("Icons.16x16.Interface.png");
            IconNumberStruct = AddImageFromManifestResource("Icons.16x16.Struct.png");
            IconNumberDelegate = AddImageFromManifestResource("Icons.16x16.Delegate.png");
            IconNumberEnum = AddImageFromManifestResource("Icons.16x16.Enum.png");
            IconNumberLocal = AddImageFromManifestResource("Icons.16x16.Local.png");
            IconNumberConstant = AddImageFromManifestResource("Icons.16x16.Literal.png");
            IconNumberPrivateField = AddImageFromManifestResource("Icons.16x16.PrivateField.png");
            IconNumberPrivateMethod = AddImageFromManifestResource("Icons.16x16.PrivateMethod.png");
            IconNumberPrivateProperty = AddImageFromManifestResource("Icons.16x16.PrivateProperty.png");
            IconNumberProtectedField = AddImageFromManifestResource("Icons.16x16.ProtectedField.png");
            IconNumberProtectedMethod = AddImageFromManifestResource("Icons.16x16.ProtectedMethod.png");
            IconNumberProtectedProperty = AddImageFromManifestResource("Icons.16x16.ProtectedProperty.png");
            IconNumberPrivateEvent = AddImageFromManifestResource("Icons.16x16.PrivateEvent.png");
            IconNumberProtectedEvent = AddImageFromManifestResource("Icons.16x16.ProtectedEvent.png");
            IconNumberPrivateDelegate = AddImageFromManifestResource("Icons.16x16.PrivateDelegate.png");
            IconNumberProtectedDelegate = AddImageFromManifestResource("Icons.16x16.ProtectedDelegate.png");
            IconNumberUnitNamespace = AddImageFromManifestResource("Icons.16x16.UnitNamespace.png");
            IconNumberInternalField = AddImageFromManifestResource("Icons.16x16.InternalField.png");
            IconNumberInternalMethod = AddImageFromManifestResource("Icons.16x16.InternalMethod.png");
            IconNumberInternalProperty = AddImageFromManifestResource("Icons.16x16.InternalProperty.png");
            IconNumberInternalEvent = AddImageFromManifestResource("Icons.16x16.InternalEvent.png");
            IconNumberInternalDelegate = AddImageFromManifestResource("Icons.16x16.InternalDelegate.png");
            IconNumberGotoText = AddImageFromManifestResource("Icons.16x16.GotoText.png");
            IconNumberInternalConstant = AddImageFromManifestResource("Icons.16x16.InternalConstant.png");
            IconNumberPrivateConstant = AddImageFromManifestResource("Icons.16x16.PrivateConstant.png");
            IconNumberProtectedConstant = AddImageFromManifestResource("Icons.16x16.ProtectedConstant.png");
            IconNumberKeyword = AddImageFromManifestResource("Icons.16x16.Keyword.png");
            IconNumberEvalError = AddImageFromManifestResource("Icons.16x16.ErrorInWatch.png");
            IconNumberExtensionMethod = AddImageFromManifestResource("Icons.16x16.ExtensionMethod.png");
        }

        public int GetPictureNum(SymInfo si)
        {
            switch (si.kind)
            {

                case SymbolKind.Field:
                    switch (si.acc_mod)
                    {
                        case PascalABCCompiler.SyntaxTree.access_modifer.private_modifer:
                            return IconNumberPrivateField;
                        case PascalABCCompiler.SyntaxTree.access_modifer.protected_modifer:
                            return IconNumberProtectedField;
                        case PascalABCCompiler.SyntaxTree.access_modifer.internal_modifer:
                            return IconNumberInternalField;
                        default:
                            return IconNumberField;
                    }
                case SymbolKind.Method:
                    switch (si.acc_mod)
                    {
                        case PascalABCCompiler.SyntaxTree.access_modifer.private_modifer:
                            return IconNumberPrivateMethod;
                        case PascalABCCompiler.SyntaxTree.access_modifer.protected_modifer:
                            return IconNumberProtectedMethod;
                        case PascalABCCompiler.SyntaxTree.access_modifer.internal_modifer:
                            return IconNumberInternalMethod;
                        default:
                            if (si.description != null && si.description.Contains("(" + PascalABCCompiler.StringResources.Get("CODE_COMPLETION_EXTENSION")))
                                return IconNumberExtensionMethod;
                            return IconNumberMethod;
                    }
                case SymbolKind.Property:
                    switch (si.acc_mod)
                    {
                        case PascalABCCompiler.SyntaxTree.access_modifer.private_modifer:
                            return IconNumberPrivateProperty;
                        case PascalABCCompiler.SyntaxTree.access_modifer.protected_modifer:
                            return IconNumberProtectedProperty;
                        case PascalABCCompiler.SyntaxTree.access_modifer.internal_modifer:
                            return IconNumberInternalProperty;
                        default:
                            return IconNumberProperty;
                    }
                case SymbolKind.Event:
                    switch (si.acc_mod)
                    {
                        case PascalABCCompiler.SyntaxTree.access_modifer.private_modifer:
                            return IconNumberPrivateEvent;
                        case PascalABCCompiler.SyntaxTree.access_modifer.protected_modifer:
                            return IconNumberProtectedEvent;
                        case PascalABCCompiler.SyntaxTree.access_modifer.internal_modifer:
                            return IconNumberInternalEvent;
                        default:
                            return IconNumberEvent;
                    }
                case SymbolKind.Delegate:
                    switch (si.acc_mod)
                    {
                        case PascalABCCompiler.SyntaxTree.access_modifer.private_modifer:
                            return IconNumberPrivateDelegate;
                        case PascalABCCompiler.SyntaxTree.access_modifer.protected_modifer:
                            return IconNumberProtectedDelegate;
                        case PascalABCCompiler.SyntaxTree.access_modifer.internal_modifer:
                            return IconNumberInternalDelegate;
                        default:
                            return IconNumberDelegate;
                    }
                case SymbolKind.Variable:
                case SymbolKind.Parameter:
                    return IconNumberLocal;
                case SymbolKind.Type:
                case SymbolKind.Class:
                    return IconNumberClass;
                case SymbolKind.Namespace:
                    if (si.IsUnitNamespace)
                        return IconNumberUnitNamespace;
                    else
                        return IconNumberNamespace;
                case SymbolKind.Interface:
                    return IconNumberInterface;
                case SymbolKind.Struct:
                    return IconNumberStruct;
                case SymbolKind.Enum:
                    return IconNumberEnum;
                case SymbolKind.Constant:
                    switch (si.acc_mod)
                    {
                        case PascalABCCompiler.SyntaxTree.access_modifer.private_modifer:
                            return IconNumberPrivateConstant;
                        case PascalABCCompiler.SyntaxTree.access_modifer.protected_modifer:
                            return IconNumberProtectedConstant;
                        case PascalABCCompiler.SyntaxTree.access_modifer.internal_modifer:
                            return IconNumberInternalConstant;
                        default:
                            return IconNumberConstant;
                    }
            }
            return IconNumberMethod;
        }


    }
}
