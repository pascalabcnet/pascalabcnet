using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace NETGenerator
{
    internal static class AppConfigUtil
    {
        private static class Namespaces
        {
            public const string Asm = "urn:schemas-microsoft-com:asm.v1";
        }

        private static class Elements
        {
            public static readonly XName Configuration = "configuration";
            public static readonly XName Runtime = "runtime";

            public static readonly XName AssemblyBinding = XName.Get("assemblyBinding", Namespaces.Asm);
            public static readonly XName DependentAssembly = XName.Get("dependentAssembly", Namespaces.Asm);
            public static readonly XName AssemblyIdentity = XName.Get("assemblyIdentity", Namespaces.Asm);
            public static readonly XName BindingRedirect = XName.Get("bindingRedirect", Namespaces.Asm);
        }

        public static void UpdateAppConfig(
            ICollection<AssemblyName> bindingRedirects,
            string appConfigPath)
        {
            if (bindingRedirects.Count == 0) return;
            var config = File.Exists(appConfigPath)
                ? XDocument.Load(appConfigPath)
                : new XDocument(new XElement(Elements.Configuration));
            var assemblyBinding = config.GetOrCreateElement(
                Elements.Configuration,
                Elements.Runtime,
                Elements.AssemblyBinding);

            var hasChanges = false;
            foreach (var redirect in bindingRedirects)
            {
                hasChanges |= assemblyBinding.CreateOrUpdateDependentAssembly(redirect);
            }

            if (hasChanges)
            {
                config.Save(appConfigPath);
            }
        }

        private static XElement GetOrCreateElement(this XContainer container, params XName[] names)
        {
            if (names.Length == 0) throw new ArgumentException("names should not be empty.", nameof(names));

            var currentPosition = container;
            foreach (var name in names)
            {
                var existingElement = currentPosition.Element(name);
                if (existingElement != null)
                {
                    currentPosition = existingElement;
                }
                else
                {
                    var newElement = new XElement(name);
                    currentPosition.Add(newElement);
                    currentPosition = newElement;
                }
            }

            return (XElement)currentPosition;
        }

        private static bool CreateOrUpdateDependentAssembly(this XContainer assemblyBinding, AssemblyName assemblyName)
        {
            var assemblyPublicKeyToken = assemblyName.GetPublicKeyToken();
            if (assemblyPublicKeyToken == null) return false;
            var publicKeyTokenString = string.Join(
                "",
                assemblyName.GetPublicKeyToken().Select(b => b.ToString("x2", CultureInfo.InvariantCulture)));
            var assemblyCulture = string.IsNullOrEmpty(assemblyName.CultureInfo?.Name)
                ? "neutral"
                : assemblyName.CultureInfo?.Name;

            foreach (var dependentAssembly in assemblyBinding.Elements(Elements.DependentAssembly))
            {
                var identity = dependentAssembly.Element(Elements.AssemblyIdentity);
                if (identity == null) continue;
                var name = identity.Attribute("name")?.Value;
                var publicKeyToken = identity.Attribute("publicKeyToken")?.Value;
                var culture = identity.Attribute("culture")?.Value;
                if (string.IsNullOrEmpty(culture)) culture = "neutral";

                if (name == assemblyName.Name
                    && publicKeyToken.Equals(publicKeyTokenString, StringComparison.CurrentCultureIgnoreCase)
                    && assemblyCulture == culture)
                {
                    var bindingRedirect = dependentAssembly.GetOrCreateElement(Elements.BindingRedirect);
                    {
                        return UpdateBindingRedirect(assemblyName, bindingRedirect);
                    }
                }
            }

            var newAssembly = new XElement(
                Elements.DependentAssembly,
                CreateIdentityElement(assemblyName, publicKeyTokenString, assemblyCulture),
                CreateBindingRedirect(assemblyName));
            assemblyBinding.Add(newAssembly);
            return true;

            
        }

        static bool UpdateBindingRedirect(AssemblyName assemblyName, XElement bindingRedirect)
        {
            var newVersion = assemblyName.Version.ToString(4);
            var oldVersionRange = $"0.0.0.0-{newVersion}";
            var hasChanges =
                bindingRedirect.Attribute("oldVersion")?.Value != oldVersionRange
                || bindingRedirect.Attribute("newVersion")?.Value != newVersion;
            if (!hasChanges) return false;

            bindingRedirect.SetAttributeValue("oldVersion", oldVersionRange);
            bindingRedirect.SetAttributeValue("newVersion", newVersion);
            return true;
        }

        static XElement CreateIdentityElement(AssemblyName assemblyName, string publicKeyTokenString, string assemblyCulture)
        {
            var element = new XElement(Elements.AssemblyIdentity);
            element.SetAttributeValue("name", assemblyName.Name);
            element.SetAttributeValue("publicKeyToken", publicKeyTokenString);
            element.SetAttributeValue("culture", assemblyCulture);
            return element;
        }

        static XElement CreateBindingRedirect(AssemblyName assemblyName)
        {
            var element = new XElement(Elements.BindingRedirect);
            UpdateBindingRedirect(assemblyName, element);
            return element;
        }
    }
}