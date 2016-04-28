using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using EnvDTE;
using EnvDTE80;
using T4Documentation.Generator.Constants;
using T4Documentation.Generator.Domain;

namespace T4Documentation.Generator.Logic
{
    /// <summary>
    ///     A library of ad-hoc functions for use in the document generator.
    /// </summary>
    public static class Functions
    {
        /// <summary>
        ///     Converts a collection of CodeFunction2 objects to a collection of domain specific MethodInfo objects.
        /// </summary>
        public static List<MethodInfo> GetMethodInfo(IEnumerable<CodeFunction2> functions)
        {
            var methodInfos = new List<MethodInfo>();

            if (functions != null && functions.Any())
            {
                foreach (var function in functions)
                {
                    var parameters = GetParameters(function);
                    var parameterInfos = GetParamterInfo(parameters);

                    var methodInfo = new MethodInfo
                    {
                        Access = GetAccess(function.Access, function.IsShared, function.MustImplement),
                        Name = function.Name,
                        ReturnType = function.Type.AsString,
                        ParameterInfos = parameterInfos
                    };

                    if (!string.IsNullOrEmpty(function.DocComment))
                    {
                        methodInfo.Summary = GetTagContent("summary", function.DocComment);
                        methodInfo.Remarks = GetTagContent("remarks", function.DocComment);
                        methodInfo.Example = GetTagContent("example", function.DocComment);
                        methodInfo.ExampleCode = GetTagContent("code", function.DocComment);
                    }

                    methodInfos.Add(methodInfo);
                }
            }

            return methodInfos;
        }

        /// <summary>
        ///     Converts a collection of CodeParameter objects to a collection of domain specific ParameterInfo objects.
        /// </summary>
        public static List<ParameterInfo> GetParamterInfo(IEnumerable<CodeParameter> parameters)
        {
            var paramterInfos = new List<ParameterInfo>();

            if (parameters != null && parameters.Any())
            {
                foreach (var parameter in parameters)
                {
                    var paramterInfo = new ParameterInfo
                    {
                        Name = parameter.Name,
                        Type = parameter.Type.AsString
                    };

                    paramterInfos.Add(paramterInfo);
                }
            }

            return paramterInfos;
        }

        /// <summary>
        ///     Converts a collection of CodeProperty2 objects to a collection of domain specific PropertyInfo objects.
        /// </summary>
        public static List<PropertyInfo> GetPropertyInfo(IEnumerable<CodeProperty2> properties)
        {
            var propertyInfos = new List<PropertyInfo>();

            if (properties != null && properties.Any())
            {
                foreach (var property in properties)
                {
                    var propertyInfo = new PropertyInfo
                    {
                        Access = GetAccess(property.Access, property.IsShared, false),
                        Name = property.Name,
                        Type = property.Type.AsString
                    };

                    if (!string.IsNullOrEmpty(property.DocComment))
                    {
                        propertyInfo.Summary = GetTagContent("summary", property.DocComment);
                        propertyInfo.Remarks = GetTagContent("remarks", property.DocComment);
                        propertyInfo.Example = GetTagContent("example", property.DocComment);
                        propertyInfo.ExampleCode = GetTagContent("code", property.DocComment);
                    }

                    propertyInfos.Add(propertyInfo);
                }
            }

            return propertyInfos;
        }

        /// <summary>
        /// Returns a descriptive string of access, static and abtract status'
        /// </summary>
        /// <param name="access"></param>
        /// <param name="isShared"></param>
        /// <param name="isAbstract"></param>
        /// <returns></returns>
        /// <code>
        /// var methodInfo = new MethodInfo
        ///  {
        ///      Access = GetAccess(function.Access, function.IsShared, function.MustImplement),
        ///      Name = function.Name,
        ///      ReturnType = function.Type.AsString,
        ///      ParameterInfos = parameterInfos
        ///  };
        /// </code>
        public static string GetAccess(vsCMAccess access, bool isShared, bool isAbstract)
        {
            var sb = new StringBuilder();

            switch (access)
            {
                case vsCMAccess.vsCMAccessPublic:
                    sb.Append("Public");
                    break;
                case vsCMAccess.vsCMAccessProjectOrProtected:
                case vsCMAccess.vsCMAccessProtected:
                    sb.Append("Protected");
                    break;
                case vsCMAccess.vsCMAccessAssemblyOrFamily:
                    sb.Append("Internal");
                    break;
                case vsCMAccess.vsCMAccessPrivate:
                default:
                    sb.Append("Private");
                    break;
            }

            if (isShared)
            {
                sb.Append(" static");
            }

            if (isAbstract)
            {
                sb.Append(" abstract");
            }

            return sb.ToString();
        }

        /// <summary>
        ///     Returns the value of a specific xml tag from an input string.
        /// </summary>
        public static string GetTagContent(string tagName, string input)
        {
            var sb = new StringBuilder();

            try
            {
                var xmldoc = new XmlDocument();
                xmldoc.LoadXml(input);

                var nodeList = xmldoc.GetElementsByTagName(tagName);
             
                foreach (XmlNode node in nodeList)
                {
                    sb.Append(node.InnerText.Trim());
                }
            }
            catch (Exception ex)
            {
                sb.Append("Ensure all character are valid XML symbols. eg; '&' should be '&amp;'.");
                sb.Append(ex.Message);
            }


            return sb.ToString();
        }

        /// <summary>
        ///     Tests a CodeFunction2's function kind.
        /// </summary>
        /// <param name="f"></param>
        /// <param name="kind"></param>
        /// <returns></returns>
        public static bool TestFunctionKind(CodeFunction2 f, vsCMFunction kind)
        {
            try
            {
                return f.FunctionKind == kind;
            }
            catch
            {
                // FunctionKind blows up in some cases. Just ignore.
                return false;
            }
        }

        /// <summary>
        ///     Return all the CodeFunction2 objects in the CodeStruct2's member collection.
        /// </summary>
        /// <param name="codeStruct"></param>
        /// <returns></returns>
        public static IEnumerable<CodeFunction2> GetMethods(CodeStruct2 codeStruct)
        {
            return codeStruct.Members.OfType<CodeFunction2>()
                .Where(f => TestFunctionKind(f, vsCMFunction.vsCMFunctionFunction));
        }

        /// <summary>
        ///     Return all the CodeFunction2 objects in the CodeClass2's member collection.
        /// </summary>
        /// <param name="codeClass"></param>
        /// <returns></returns>
        public static IEnumerable<CodeFunction2> GetMethods(CodeClass2 codeClass)
        {
            return codeClass.Members.OfType<CodeFunction2>()
                .Where(f => TestFunctionKind(f, vsCMFunction.vsCMFunctionFunction));
        }


        /// <summary>
        ///     Return all the CodeFunction2 objects in the CodeInterface's member collection.
        /// </summary>
        /// <param name="codeInterface"></param>
        /// <returns></returns>
        public static IEnumerable<CodeFunction2> GetMethods(CodeInterface codeInterface)
        {
            return codeInterface.Members.OfType<CodeFunction2>()
                .Where(f => TestFunctionKind(f, vsCMFunction.vsCMFunctionFunction));
        }


        /// <summary>
        ///     Return all the CodeProperty2 objects in the CodeClass2's member collection.
        /// </summary>
        /// <param name="codeClass"></param>
        /// <returns></returns>
        public static IEnumerable<CodeProperty2> GetProperties(CodeClass2 codeClass)
        {
            return codeClass.Members.OfType<CodeProperty2>();
        }

        /// <summary>
        ///     Return all the CodeProperty2 objects in the CodeInterface's member collection.
        /// </summary>
        /// <param name="codeInterface"></param>
        /// <returns></returns>
        public static IEnumerable<CodeProperty2> GetProperties(CodeInterface codeInterface)
        {
            return codeInterface.Members.OfType<CodeProperty2>();
        }

        /// <summary>
        ///     Return all the CodeProperty2 objects in the CodeStruct2's member collection.
        /// </summary>
        /// <param name="codeStruct"></param>
        /// <returns></returns>
        public static IEnumerable<CodeProperty2> GetProperties(CodeStruct2 codeStruct)
        {
            return codeStruct.Members.OfType<CodeProperty2>();
        }

        /// <summary>
        ///     Return all the CodeParameter objects in the CodeFunction2's Parameters collection.
        /// </summary>
        /// <param name="codeFunction"></param>
        /// <returns></returns>
        public static IEnumerable<CodeParameter> GetParameters(CodeFunction2 codeFunction)
        {
            return codeFunction.Parameters.OfType<CodeParameter>();
        }

        /// <summary>
        ///    Sets the ProjectItemInfo type and typeplural value depending on the originating object type.
        /// </summary>
        /// <returns></returns>
        public static void SetProjectItemInfoType(ProjectItemInfo projectItem, object objectType)
        {
            if (objectType is CodeClass2)
            {
                var classType = (CodeClass2) objectType;

                if (IsController(classType))
                {
                    projectItem.Type = "Controller";
                    projectItem.TypePlural = "Controllers";
                }
                else if (IsWebApi(classType))
                {
                    projectItem.Type = "Web API Controller";
                    projectItem.TypePlural = "Web API Controllers";
                }
                else if (IsModel(classType))
                {
                    projectItem.Type = "Model";
                    projectItem.TypePlural = "Models";
                }
                else
                {
                    projectItem.Type = "Class";
                    projectItem.TypePlural = "Classes";
                }
            }
            else if (objectType is CodeInterface)
            {
                projectItem.Type = "Interface";
                projectItem.TypePlural = "Interfaces";
            }
            else if (objectType is CodeStruct2)
            {
                projectItem.Type = "Struct";
                projectItem.TypePlural = "Structs";
            }
        }

        /// <summary>
        ///     Returns true if a ProjectItem is a folder.
        /// </summary>
        /// <returns></returns>
        public static bool IsFolder(ProjectItem item)
        {
            return item.Kind == EnvDTEConstants.vsProjectItemKindPhysicalFolder;
        }

        /// <summary>
        ///     Returns true if a CodeClass2 is a controller.
        /// </summary>
        /// <returns></returns>
        public static bool IsController(CodeClass2 type)
        {
            for (; type.FullName != "System.Web.Mvc.Controller"; type = (CodeClass2) type.Bases.Item(1))
            {
                if (type.Bases.Count == 0)
                    return false;
            }
            return true;
        }

        /// <summary>
        ///     Returns true if a CodeClass2 is a web API controller.
        /// </summary>
        /// <returns></returns>
        public static bool IsWebApi(CodeClass2 type)
        {
            for (; type.FullName != "System.Web.Http.ApiController"; type = (CodeClass2) type.Bases.Item(1))
            {
                if (type.Bases.Count == 0)
                    return false;
            }
            return true;
        }

        /// <summary>
        ///     Returns true if a CodeClass2 is a model.
        /// </summary>
        /// <returns></returns>
        public static bool IsModel(CodeClass2 type)
        {
            if (type.Name.ToLower().Contains("model"))
            {
                return true;
            }

            return false;
        }


        /// <summary>
        ///     Returns true if a CodeClass2 is an async controller.
        /// </summary>
        /// <returns></returns>
        public static bool IsAsyncController(CodeClass2 type)
        {
            for (; type.FullName != "System.Web.Mvc.AsyncController"; type = (CodeClass2) type.Bases.Item(1))
            {
                if (type.Bases.Count == 0)
                    return false;
            }
            return true;
        }

        /// <summary>
        ///     Returns a ProjectItem from a Project based on a name.
        /// </summary>
        /// <returns></returns>
        public static ProjectItem GetProjectItem(Project project, string name)
        {
            return GetProjectItem(project.ProjectItems, name);
        }



        /// <summary>
        ///     Returns a ProjectItem from a Project based on a subPath.
        /// </summary>
        /// <returns></returns>
        public static ProjectItem GetProjectItem(ProjectItems items, string subPath)
        {
            ProjectItem current = null;
            foreach (var name in subPath.Split('\\'))
            {
                try
                {
                    // ProjectItems.Item() sometimes throws when it doesn't exist, so catch the exception
                    // to return null instead.
                    current = items.Item(name);

                    if (current == null) return null;
                }
                catch
                {
                    // If any chunk couldn't be found, fail
                    return null;
                }

                items = current.ProjectItems;
            }

            return current;
        }

        
    }
}
