using System;
using System.IO;
using System.Linq;
using System.Text;
using Nino.Shared.Mgr;
using Nino.Shared.Util;
using System.Reflection;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Nino.Serialization
{
    /// <summary>
    /// Nino code generator
    /// TODO support nullable, hashset, queue, stack
    /// </summary>
    public static class CodeGenerator
    {
        /// <summary>
        /// Generate serialization code file at Assets/ouputPath
        /// editor only method
        /// </summary>
        /// <param name="outputPath"></param>
        /// <param name="assemblies"></param>
        public static void GenerateSerializationCodeForAllTypePossible(string outputPath = "Nino/Generated",
            Assembly[] assemblies = null)
        {
            //find all types
            var types = (assemblies ?? AppDomain.CurrentDomain.GetAssemblies()).SelectMany(a => a.GetTypes()).ToList()
                .FindAll(t =>
                {
                    //find NinoSerializeAttribute
                    NinoSerializeAttribute[] ns =
                        (NinoSerializeAttribute[])t.GetCustomAttributes(typeof(NinoSerializeAttribute), true);
                    if (ns.Length == 0) return false;
                    return true;
                }).ToList();
            //iterate
            foreach (var type in types)
            {
                //gen
                GenerateSerializationCode(type, outputPath);
            }
        }

        /// <summary>
        /// Get a valid nino serialize class
        /// </summary>
        /// <param name="type"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        private static bool GetValidNinoClass(Type type, bool log = true)
        {
            //nested
            if (type.IsNested)
            {
                if (log)
                    Logger.E("Code Gen", $"Can not generate code for type: {type} due to it is a nested class");
                return false;
            }

            //generic
            if (type.IsGenericType)
            {
                if (log)
                    Logger.E("Code Gen", $"Can not generate code for type: {type} due to it is a generic class");
                return false;
            }

            //find NinoSerializeAttribute
            NinoSerializeAttribute[] ns =
                (NinoSerializeAttribute[])type.GetCustomAttributes(typeof(NinoSerializeAttribute), true);
            if (ns.Length == 0) return false;

            CodeGenIgnoreAttribute[] ci =
                (CodeGenIgnoreAttribute[])type.GetCustomAttributes(typeof(CodeGenIgnoreAttribute), true);
            if (ci.Length != 0) return false;
            return true;
        }

        /// <summary>
        /// Generate serialization code file at Assets/ouputPath
        /// editor only method
        /// </summary>
        /// <param name="type"></param>
        /// <param name="outputPath"></param>
        // ReSharper disable CognitiveComplexity
        public static void GenerateSerializationCode(Type type, string outputPath = "Nino/Generated")
            // ReSharper restore CognitiveComplexity
        {
            //nino class only
            if (!GetValidNinoClass(type)) return;

            //code template
            string template =
                @"/* this is generated by nino */
using System.Runtime.CompilerServices;

{namespace}
{start}
    public partial struct {type}
    {
        public static {type}.SerializationHelper NinoSerializationHelper = new {type}.SerializationHelper();
        public unsafe class SerializationHelper: Nino.Serialization.NinoWrapperBase<{type}>
        {
            #region NINO_CODEGEN
            public SerializationHelper()
            {
{ctor}
            }
            
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public override void Serialize({type} value, ref Nino.Serialization.Writer writer)
            {
                {nullCheck}
                writer.Write(true);
{members}
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public override {type} Deserialize(Nino.Serialization.Reader reader)
            {
                if(!reader.ReadBool())
                    {returnNull}
                {type} value = new {type}();
{fields}
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public override int GetSize({type} value)
            {
                {sizeNull}
{size}
            }
            #endregion
        }
    }
{end}";
            if (type.IsClass)
            {
                template = template.Replace("struct", "class");
                template = template.Replace("{nullCheck}", @"if(value == null)
                {
                    writer.Write(false);
                    return;
                }");
                template = template.Replace("{returnNull}", "return null;");
                template = template.Replace("{sizeNull}", @"if(value == null)
                {
                    return 1;
                }");
            }
            else
            {
                template = template.Replace("{nullCheck}", @"");
                template = template.Replace("{returnNull}", "return default;");
                template = template.Replace("{sizeNull}", @"");
            }

            //replace namespace
            if (!string.IsNullOrEmpty(type.Namespace))
            {
                template = template.Replace("{namespace}", $"namespace {type.Namespace}");
                template = template.Replace("{start}", "{");
                template = template.Replace("{end}", "}");
            }
            else
            {
                template = template.Replace("{namespace}", string.Empty);
                template = template.Replace("{start}", string.Empty);
                template = template.Replace("{end}", string.Empty);
            }

            //class full name
            var classFullName =
                $"{type.GetFriendlyName()}";
            //replace full name
            template = template.Replace("{type}", classFullName);

            //find members
            TypeModel.TryGetModel(type, out var model);
            //invalid model
            if (model != null)
            {
                if (!model.Valid)
                {
                    throw new InvalidOperationException("invalid model");
                }
            }

            //generate model
            if (model == null)
            {
                model = TypeModel.CreateModel(type);
            }

            HashSet<MemberInfo> members = model.Members;

            #region serialize

            //build params
            StringBuilder sb = new StringBuilder();
            foreach (var member in members)
            {
                var mt = member is FieldInfo fi ? fi.FieldType : ((PropertyInfo)member).PropertyType;
                //enum
                if (mt.IsEnum)
                {
                    sb.Append(
                        $"                writer.WriteEnum<{BeautifulLongTypeName(mt)}>(value.{member.Name});\n");
                }
                //array/list
                else if (mt.IsArray || (mt.IsGenericType && mt.GetGenericTypeDefinition() == ConstMgr.ListDefType))
                {
                    sb.Append($"                writer.Write(value.{member.Name});\n");
                }
                //dict
                else if (mt.IsGenericType && mt.GetGenericTypeDefinition() == ConstMgr.DictDefType)
                {
                    sb.Append($"                writer.Write(value.{member.Name});\n");
                }
                //basic type
                else
                {
                    sb.Append(
                        $"                {GetSerializeBasicTypeStatement(mt, $"value.{member.Name}", member is PropertyInfo)};\n");
                }
            }

            if (members.Count > 0)
            {
                //remove \n at the end
                sb.Remove(sb.Length - 1, 1);
            }

            //replace template members
            template = template.Replace("{members}", sb.ToString());

            #endregion

            #region deserialize

            sb.Clear();
            foreach (var member in members)
            {
                var mt = member is FieldInfo fi ? fi.FieldType : ((PropertyInfo)member).PropertyType;
                //enum
                if (mt.IsEnum)
                {
                    if (member is PropertyInfo)
                    {
                        sb.Append(
                            $"                value.{member.Name} = reader.ReadEnum<{BeautifulLongTypeName(mt)}>();\n");
                    }
                    else
                    {
                        sb.Append(
                            $"                reader.ReadEnum<{BeautifulLongTypeName(mt)}>(ref value.{member.Name});\n");
                    }
                }
                //array/list
                else if (mt.IsArray || (mt.IsGenericType && mt.GetGenericTypeDefinition() == ConstMgr.ListDefType))
                {
                    Type elemType = mt.IsGenericType ? mt.GenericTypeArguments[0] : mt.GetElementType();

                    //create field
                    if (mt.IsArray)
                    {
                        //multidimensional array
                        if (mt.GetArrayRank() > 1)
                        {
                            throw new NotSupportedException(
                                "can not serialize multidimensional array, use jagged array instead");
                        }

                        sb.Append(
                            $"                value.{member.Name} = reader.ReadArray<{BeautifulLongTypeName(elemType)}>();\n");
                    }
                    else
                    {
                        sb.Append(
                            $"                value.{member.Name} = reader.ReadList<{BeautifulLongTypeName(elemType)}>();\n");
                    }
                }
                //dict
                else if (mt.IsGenericType && mt.GetGenericTypeDefinition() == ConstMgr.DictDefType)
                {
                    var args = mt.GetGenericArguments();
                    Type keyType = args[0];
                    Type valueType = args[1];

                    sb.Append(
                        $"                value.{member.Name} = reader.ReadDictionary<{BeautifulLongTypeName(keyType)},{BeautifulLongTypeName(valueType)}>();\n");
                }
                //not enum -> basic type
                else
                {
                    string val = GetDeserializeBasicTypeStatement(mt, member is PropertyInfo,
                        $"value.{member.Name}");
                    sb.Append($"                {val}\n");
                }
            }

            sb.Append("                return value;\n");
            //remove comma at the end
            sb.Remove(sb.Length - 1, 1);

            //replace template fields
            template = template.Replace("{fields}", sb.ToString());

            #endregion

            #region getsize

            sb.Clear();
            if (TypeModel.IsUnmanaged(type))
            {
                sb.Append($"                return Nino.Serialization.Serializer.GetFixedSize<{BeautifulLongTypeName(type)}>();");
            }
            else
            {
                sb.Append("                int ret = 1;\n");
                foreach (var member in members)
                {
                    sb.Append(
                        $"                ret += Nino.Serialization.Serializer.GetSize(value.{member.Name});\n");
                }
                sb.Append("                return ret;");
            }

            //replace template fields
            template = template.Replace("{size}", sb.ToString());

            #endregion

            #region ctor

            sb.Clear();
            if (TypeModel.IsUnmanaged(type))
            {
                sb.Append("                int ret = 1;\n");
                foreach (var member in members)
                {
                    var mt = member is FieldInfo fi ? fi.FieldType : ((PropertyInfo)member).PropertyType;
                    sb.Append(
                        $"                ret += sizeof({BeautifulLongTypeName(mt)});\n");
                }
                sb.Append($"                Nino.Serialization.Serializer.SetFixedSize<{BeautifulLongTypeName(type)}>(ret);");
            }

            //replace template fields
            template = template.Replace("{ctor}", sb.ToString());

            #endregion

            //save path
            var output = Path.Combine(ConstMgr.AssetPath, outputPath);
            if (!Directory.Exists(output))
            {
                Directory.CreateDirectory(output);
            }

            //save file path
            output = Path.Combine(output,
                $"{type.Namespace}{(!string.IsNullOrEmpty(type.Namespace) ? "." : "")}{type.GetFriendlyName()}"
                    .Replace(".", "_").Replace(",", "_")
                    .Replace("<", "_").Replace(">", "_") +
                "_Serialize.cs");
            if (File.Exists(output))
            {
                File.Delete(output);
            }

            //save
            File.WriteAllText(output, template);

#if UNITY_2017_1_OR_NEWER
            Logger.D("Code Gen", $"saved {output}");
#else
            Logger.D("Code Gen", $"saved {output}, please move this file to your project");
#endif
        }

        // ReSharper disable CognitiveComplexity
        private static string GetDeserializeBasicTypeStatement(Type mt, bool isProperty, string val = "",
                string space = "                ")
            // ReSharper restore CognitiveComplexity
        {
            switch (Type.GetTypeCode(mt))
            {
                case TypeCode.Int32:
                case TypeCode.UInt32:
                case TypeCode.Int64:
                case TypeCode.UInt64:
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.Int16:
                case TypeCode.UInt16:
                case TypeCode.Boolean:
                case TypeCode.Double:
                case TypeCode.Single:
                case TypeCode.Decimal:
                case TypeCode.Char:
                case TypeCode.DateTime:
                    return isProperty
                        ? $"{val} = reader.Read<{BeautifulLongTypeName(mt)}>(sizeof({BeautifulLongTypeName(mt)}));"
                        : $"reader.Read<{BeautifulLongTypeName(mt)}>(ref {val}, sizeof({BeautifulLongTypeName(mt)}));";
                case TypeCode.String:
                    return $"{val} = reader.ReadString();";
                default:
                    if (GetValidNinoClass(mt, false))
                    {
                        return $"{val} = {BeautifulLongTypeName(mt)}.NinoSerializationHelper.Deserialize(reader)";
                    }

                    //enum
                    if (mt.IsEnum)
                    {
                        return isProperty
                            ? $"{val} = reader.ReadEnum<{BeautifulLongTypeName(mt)}>();"
                            : $"reader.ReadEnum<{BeautifulLongTypeName(mt)}>(ref {val});";
                    }

                    if (mt.IsArray ||
                        (mt.IsGenericType && mt.GetGenericTypeDefinition() == ConstMgr.ListDefType))
                    {
                        Type elemType = mt.IsGenericType ? mt.GenericTypeArguments[0] : mt.GetElementType();
                        StringBuilder builder = new StringBuilder();
                        if (mt.IsArray)
                        {
                            //multidimensional array
                            if (mt.GetArrayRank() > 1)
                            {
                                throw new NotSupportedException(
                                    "can not serialize multidimensional array, use jagged array instead");
                            }

                            builder.Append(
                                $"{val} = reader.ReadArray<{BeautifulLongTypeName(elemType)}>();\n");
                        }
                        else
                        {
                            builder.Append(
                                $"{val} = reader.ReadList<{BeautifulLongTypeName(elemType)}>();\n");
                        }

                        return builder.ToString();
                    }

                    if (mt.IsGenericType && mt.GetGenericTypeDefinition() == ConstMgr.DictDefType)
                    {
                        StringBuilder builder = new StringBuilder();
                        var args = mt.GetGenericArguments();
                        Type keyType = args[0];
                        Type valueType = args[1];
                        //create field
                        builder.Append(space).Append(
                            $"{BeautifulLongTypeName(mt)} {val} = reader.ReadDictionary<{BeautifulLongTypeName(keyType)},{BeautifulLongTypeName(valueType)}>();\n");
                        return builder.ToString();
                    }

                    return $"{val} = reader.ReadCommonVal<{BeautifulLongTypeName(mt)}>();";
            }
        }

        // ReSharper disable CognitiveComplexity
        private static string GetSerializeBasicTypeStatement(Type mt, string val, bool isProperty, int indent = 0,
                string space = "                        ")
            // ReSharper restore CognitiveComplexity
        {
            switch (Type.GetTypeCode(mt))
            {
                case TypeCode.Int32:
                case TypeCode.UInt32:
                case TypeCode.Int64:
                case TypeCode.UInt64:
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.Int16:
                case TypeCode.UInt16:
                case TypeCode.Boolean:
                case TypeCode.Double:
                case TypeCode.Single:
                case TypeCode.Decimal:
                case TypeCode.Char:
                case TypeCode.DateTime:
                    return isProperty
                        ? $"writer.Write({val})"
                        : $"writer.Write(ref {val}, sizeof({BeautifulLongTypeName(mt)}))";
                case TypeCode.String:
                    return $"writer.Write({val})";
                default:
                    if (GetValidNinoClass(mt, false))
                    {
                        return $"{BeautifulLongTypeName(mt)}.NinoSerializationHelper.Serialize({val}, writer)";
                    }

                    if (mt.IsArray || (mt.IsGenericType && mt.GetGenericTypeDefinition() == ConstMgr.ListDefType))
                    {
                        StringBuilder builder = new StringBuilder();
                        Type elemType = mt.IsGenericType ? mt.GenericTypeArguments[0] : mt.GetElementType();
                        if (elemType == null)
                        {
                            throw new InvalidOperationException("Invalid array type");
                        }

                        builder.Append(space).Append(Repeat("    ", indent))
                            .Append($"writer.Write({val});\n");
                        return builder.ToString();
                    }

                    if (mt.IsGenericType && mt.GetGenericTypeDefinition() == ConstMgr.DictDefType)
                    {
                        StringBuilder builder = new StringBuilder();
                        builder.Append(space).Append(Repeat("    ", indent))
                            .Append($"writer.Write({val});\n");
                        return builder.ToString();
                    }

                    return $"writer.WriteCommonVal<{BeautifulLongTypeName(mt)}>({val})";
            }
        }

        private static string Repeat(string value, int count)
        {
            if (count < 1) return string.Empty;
            return new StringBuilder(value.Length * count).Insert(0, value, count).ToString();
        }

        private static string BeautifulLongTypeName(this Type mt)
        {
            return $"{mt.Namespace}{(!string.IsNullOrEmpty(mt.Namespace) ? "." : "")}{mt.GetFriendlyName()}";
        }

        /// <summary>
        /// 获取类型名字
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static string GetFriendlyName(this Type type)
        {
            string friendlyName = type.Name;
            if (type.IsGenericType)
            {
                int iBacktick = friendlyName.IndexOf('`');
                if (iBacktick > 0)
                {
                    friendlyName = friendlyName.Remove(iBacktick);
                }

                friendlyName += "<";
                Type[] typeParameters = type.GetGenericArguments();
                for (int i = 0; i < typeParameters.Length; ++i)
                {
                    string typeParamName =
                        $"{typeParameters[i].Namespace}{(!string.IsNullOrEmpty(typeParameters[i].Namespace) ? "." : "")}{GetFriendlyName(typeParameters[i])}";
                    friendlyName += (i == 0 ? typeParamName : "," + typeParamName);
                }

                friendlyName += ">";
            }

            return friendlyName;
        }
    }
}