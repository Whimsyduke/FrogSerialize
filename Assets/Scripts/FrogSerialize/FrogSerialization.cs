using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using UnityEditor;

namespace FrogSerialization
{
    #region 蛤序列化支持

    #region 公共声明

    #region 委托

    #endregion 委托

    #region 枚举

    /// <summary>
    /// 序列化模式
    /// </summary>
    public enum EnumFrogSerializationType
    {
        /// <summary>
        /// 不可序列化的基本类型，每个具体类型都必须写好对应的序列化方法
        /// </summary>
        NonSerialized,
        /// <summary>
        /// 可序列化类型，对象将拆分序列化其字段
        /// </summary>
        Serializable,
    }

    #endregion 枚举

    #region 定义

    #endregion 定义

    #endregion 公共声明

    #region 公共类

    /// <summary>
    /// 蛤序列化支持
    /// </summary>
    public static class FrogSerialization
    {
        #region 内部声明

        #region 常量

        #region 元素名称

        public const string Const_XmlNameEle_NonSerializedField = "NonSerializedField";
        public const string Const_XmlNameEle_SerializableField = "SerializableField";
        public const string Const_XmlNameEle_Object = "Object";
        public const string Const_XmlNameEle_Type = "Type";
        public const string Const_XmlNameEle_TypeInfo = "TypeInfo";

        #endregion 元素名称

        #region 属性名称

        public const string Const_XmlNameAtt_Field = "Field";
        public const string Const_XmlNameAtt_Name = "Name";
        public const string Const_XmlNameAtt_Reference = "Reference";
        public const string Const_XmlNameAtt_Type = "Type";
        public const string Const_XmlNameAtt_Value = "Value";

        #endregion 属性名称

        #region 公共默认参数

        /// <summary>
        /// 无元素类型数组
        /// </summary>
        public static readonly Type[] Const_NoElementTypeArray = new Type[] { };

        /// <summary>
        /// 无元素object数组
        /// </summary>
        public static readonly object[] Const_NoElementObjectArray = new object[] { };

        #endregion 公共默认参数

        #endregion 常量

        #region 枚举

        #endregion 枚举

        #region 定义

        #region XML转换助手

        private abstract class XmalHelper
        {
            #region 内部声明

            #region 常量

            #endregion 常量

            #region 枚举

            #endregion 枚举

            #region 定义

            #endregion 定义

            #region 委托

            #endregion 委托

            #endregion 内部声明

            #region 属性字段

            #region 事件

            #endregion 事件

            #region 属性

            /// <summary>
            /// 当前路径列表
            /// </summary>
            public Stack<string> StackCurrentPath { get; } = new Stack<string>();

            public Dictionary<Type, IDictionary<string, (FieldInfo Field, EnumFrogSerializationType Type, string Comment)>> DictSerializeFieldForType { get; } = new Dictionary<Type, IDictionary<string, (FieldInfo Field, EnumFrogSerializationType Type, string Comment)>>();

            #endregion 属性

            #region 字段

            #endregion 字段

            #endregion 属性字段

            #region 构造函数

            /// <summary>
            /// 构造函数
            /// </summary>
            public XmalHelper()
            {
                StackCurrentPath.Push("-");
#if SDP_HELPER_LOG
            ListLog.AppendLine("================================");
#endif //SDP_HELPER_LOG
            }

            #endregion 构造函数

            #region 方法

            #region 通用方法

            /// <summary>
            /// 初始化
            /// </summary>
            /// <param name="initObj">对象</param>
            public abstract void Init(object initObj);

#if SDP_HELPER_LOG
        /// <summary>
        /// Helper日志
        /// </summary>
        public StringBuilder ListLog = new StringBuilder();
#endif //SDP_HELPER_LOG

            /// <summary>
            ///  压入名称
            /// </summary>
            /// <param name="name">名称</param>
            public void PushName(XName name)
            {
#if SDP_HELPER_LOG
            ListLog.AppendLine($"{StackCurrentPath.Peek()}/{name}");
#endif //SDP_HELPER_LOG
                StackCurrentPath.Push($"{StackCurrentPath.Peek()}/{name}");
            }

            /// <summary>
            ///  弹出名称
            /// </summary>
            public void PopName()
            {
                StackCurrentPath.Pop();
            }

            #endregion 通用方法

            #region 重写方法

            #endregion 重写方法

            #region 事件方法

            #endregion 事件方法 

            #endregion 方法

        }

        /// <summary>
        /// XML写转换助手
        /// </summary>
        private class XmlWriteHelper : XmalHelper
        {
            #region 内部声明

            #region 常量

            #endregion 常量

            #region 枚举

            #endregion 枚举

            #region 定义

            #endregion 定义

            #region 委托

            #endregion 委托

            #endregion 内部声明

            #region 属性字段

            #region 事件

            #endregion 事件

            #region 属性

            #endregion 属性

            #region 字段

            /// <summary>
            /// 值路径字典
            /// </summary>
            private readonly Dictionary<object, string> mDictValuePath = new Dictionary<object, string>();

            /// <summary>
            /// 类型字典
            /// </summary>
            private readonly Dictionary<Type, (string Name, bool IsDefinition)> mDictType = new Dictionary<Type, (string Name, bool IsDefinition)>();

            #endregion 字段

            #endregion 属性字段

            #region 构造函数

            #endregion 构造函数

            #region 方法

            #region 通用方法

            #region 其它方法

            /// <summary>
            /// 初始化
            /// </summary>
            /// <param name="initObj">对象</param>
            public override void Init(object initObj)
            {
                StackCurrentPath.Push("-");
                mDictValuePath[initObj] = StackCurrentPath.Peek();
            }

            /// <summary>
            /// 获取值XML
            /// </summary>
            /// <param name="value">值</param>
            /// <returns>值对应XML</returns>
            /// <remarks>引用的路径值为空时，对象为null。</remarks>
            public XElement GetValueXml(object value)
            {
                if (value == null)
                {
                    return FrogSerialization.ToXml_Reference("");
                }
                else if (mDictValuePath.ContainsKey(value))
                {
                    return FrogSerialization.ToXml_Reference(mDictValuePath[value]);
                }
                else
                {
                    if (value.GetType().IsClass)
                    {
                        mDictValuePath[value] = StackCurrentPath.Peek();
                    }
                    return FrogSerialization.SerializeWithHelper(this, value);
                }
            }

            #endregion 其它方法

            #region 类型方法

            /// <summary>
            /// 获取简单名称，重复的自动加上#号。
            /// </summary>
            /// <param name="type">类型</param>
            /// <returns>名称</returns>
            private string GetSimpleName(Type type)
            {
                string baseName = type.Name;
                string name = baseName;
                int index = 0;
                while (mDictType.Count(R => R.Value.Name == name) != 0)
                {
                    name = $"{baseName}#{++index}";
                }
                return name;
            }

            /// <summary>
            /// 获取类型名称
            /// </summary>
            /// <param name="type">类型</param>
            /// <returns>名称</returns>
            public string GetTypeName(Type type)
            {
                if (!mDictType.ContainsKey(type))
                {
                    if (type.IsGenericTypeDefinition)
                    {
                        mDictType.Add(type, (GetSimpleName(type), true));
                    }
                    else if (type.IsGenericType)
                    {
                        Type baseType = type.GetGenericTypeDefinition();
                        string baseName = GetTypeName(baseType);
                        StringBuilder builder = new StringBuilder();
                        builder.Append(baseName);
                        builder.Append("[");
                        bool isFirst = true;
                        foreach (Type select in type.GetGenericArguments())
                        {
                            if (isFirst)
                            {
                                isFirst = false;
                            }
                            else
                            {
                                builder.Append("|");
                            }
                            builder.Append(GetTypeName(select));
                        }
                        builder.Append("]");
                        mDictType.Add(type, (builder.ToString(), false));
                    }
                    else
                    {
                        mDictType.Add(type, (GetSimpleName(type), false));
                    }
                }
                return mDictType[type].Name;
            }

            /// <summary>
            /// 生成Type列表Xml数据
            /// </summary>
            /// <param name="xml">列表Xml</param>
            /// <returns>Xml数据</returns>
            public void GenerateTypeList(XElement xml)
            {
                foreach (var select in mDictType)
                {
                    if (select.Value.IsDefinition) continue;
                    xml.Add(FrogSerialization.ToXml_TypeInfo(select.Value.Name, select.Key));
                }
            }

            #endregion 类型方法

            #endregion 通用方法

            #region 重写方法

            #endregion 重写方法

            #region 事件方法

            #endregion 事件方法 

            #endregion 方法
        }

        /// <summary>
        /// XML度转换助手
        /// </summary>
        private class XmlReadHelper : XmalHelper
        {
            #region 内部声明

            #region 常量

            #endregion 常量

            #region 枚举

            #endregion 枚举

            #region 定义

            #endregion 定义

            #region 委托

            #endregion 委托

            #endregion 内部声明

            #region 属性字段

            #region 事件

            #endregion 事件

            #region 属性

            #endregion 属性

            #region 字段

            /// <summary>
            /// 值路径字典
            /// </summary>
            public readonly Dictionary<string, object> mDictValuePath = new Dictionary<string, object>();

            /// <summary>
            /// 类型字典
            /// </summary>
            public readonly Dictionary<string, Type> mDictType = new Dictionary<string, Type>();

            #endregion 字段

            #endregion 属性字段

            #region 构造函数

            #endregion 构造函数

            #region 方法

            #region 通用方法

            #region 其它方法

            /// <summary>
            /// 初始化
            /// </summary>
            /// <param name="initObj">对象</param>
            public override void Init(object initObj)
            {
                mDictValuePath[StackCurrentPath.Peek()] = initObj;
            }

            /// <summary>
            /// 获取Xml对应的值
            /// </summary>
            /// <param name="value">Xml</param>
            /// <returns>值</returns>
            /// <remarks>引用的路径值为空时，对象为null。</remarks>
            public object GetXmlValue(XElement value)
            {
                XAttribute attRef = FrogSerialization.FromXml_Reference(value);
                if (attRef != null)
                {
                    // 值是一个引用
                    if (string.IsNullOrEmpty(attRef.Value))
                    {
                        return null;
                    }
                    else
                    {
                        if (!mDictValuePath.ContainsKey(attRef.Value))
                        {
                            throw new Exception("引用对象不存在或尚未解析。");
                        }
                        return mDictValuePath[attRef.Value];
                    }
                }
                else
                {
                    // 值为object
                    object baseVal = FrogSerialization.FromXml_Object(this, value);
                    mDictValuePath[StackCurrentPath.Peek()] = baseVal;
                    FrogSerialization.DeserializeWithHelper(this, value, baseVal);
                    return baseVal;
                }
            }

            #endregion 其它方法

            #region 类型方法

            /// <summary>
            /// 获取类型
            /// </summary>
            /// <param name="name">类型名字</param>
            /// <returns>类型</returns>
            public Type GetType(string name)
            {
                if (!mDictType.ContainsKey(name))
                {
                    throw new Exception("找不到名称对应的类型。");
                }
                return mDictType[name];
            }

            /// <summary>
            /// 初始化类型列表
            /// </summary>
            /// <param name="xml"></param>
            public void InitTypeList(XElement xml)
            {
                IEnumerable<XElement> elements = FrogSerialization.GetXElements(xml, FrogSerialization.Const_XmlNameEle_TypeInfo);
                foreach (XElement select in elements)
                {
                    (string Name, Type Type) info = FrogSerialization.FromXml_TypeInfo(select);
                    mDictType.Add(info.Name, info.Type);
                }
            }

            #endregion 类型方法

            #endregion 通用方法

            #region 重写方法

            #endregion 重写方法

            #region 事件方法

            #endregion 事件方法 

            #endregion 方法
        }

        #endregion XML转换助手

        #endregion 定义

        #region 委托

        /// <summary>
        /// 序列化字段委托
        /// </summary>
        /// <param name="helper">助手</param>
        /// <param name="field">字段</param>
        /// <param name="val">值</param>
        /// <returns>Xml数据</returns>
        private delegate XElement Delegate_SerializeField(XmlWriteHelper helper, FieldInfo field, object val);

        /// <summary>
        /// 反序列化字段委托
        /// </summary>
        /// <param name="helper">助手</param>
        /// <param name="xml">Xml数据</param>
        /// <param name="Obj">字段所属对象</param>
        /// <returns>Xml数据</returns>
        private delegate void Delegate_DeserializeField(XmlReadHelper helper, XElement xml, object obj);

        #endregion 委托

        #endregion 内部声明

        #region 属性字段

        #region 静态属性

        #endregion 静态属性

        #region 属性

        #endregion 属性

        #region 字段

        /// <summary>
        /// 基本类型序列化方法
        /// </summary>
        private static readonly Dictionary<Type, (Type ValueType, Delegate_SerializeField SerializeFunc, Delegate_DeserializeField DeserializeFunc)> mListSerializeFunc =
            new Dictionary<Type, (Type ValueType, Delegate_SerializeField SerializeFunc, Delegate_DeserializeField DeserializeFunc)>()
            {
                { typeof(bool), (typeof(bool), ToXml_Bool, FromXml_Bool) },
                { typeof(bool), (typeof(byte), ToXml_Byte, FromXml_Byte) },
                { typeof(bool), (typeof(char), ToXml_Char, FromXml_Char) },
                { typeof(bool), (typeof(decimal), ToXml_Decimal, FromXml_Decimal) },
                { typeof(bool), (typeof(double), ToXml_Double, FromXml_Double) },
                { typeof(bool), (typeof(Enum), ToXml_Enum, FromXml_Enum) },
                { typeof(bool), (typeof(float), ToXml_Float, FromXml_Float) },
                { typeof(bool), (typeof(int), ToXml_Int, FromXml_Int) },
                { typeof(bool), (typeof(long), ToXml_Long, FromXml_Long) },
                { typeof(bool), (typeof(sbyte), ToXml_SByte, FromXml_SByte) },
                { typeof(bool), (typeof(short), ToXml_Short, FromXml_Short) },
                { typeof(bool), (typeof(string), ToXml_String, FromXml_String) },
                { typeof(bool), (typeof(uint), ToXml_UInt, FromXml_UInt) },
                { typeof(bool), (typeof(ulong), ToXml_ULong, FromXml_ULong) },
                { typeof(bool), (typeof(ushort), ToXml_UShort, FromXml_UShort) },
                { typeof(bool), (typeof(UnityEngine.Object), ToXml_Asset, FromXml_Asset) },
            };

        #endregion 字段

        #region 事件

        #endregion 事件

        #endregion 属性字段

        #region 构造函数

        #endregion 构造函数

        #region 方法

        #region 通用方法

        #region 序列化方法

        /// <summary>
        /// 序列化对象
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns>Xml数据</returns>
        public static XElement Serialize(object obj)
        {
            XmlWriteHelper helper = new XmlWriteHelper();
            return SerializeWithHelper(helper, obj);
        }

        /// <summary>
        /// 反序列化对象
        /// </summary>
        /// <param name="xml">Xml数据</param>
        /// <returns>对象</returns>
        public static object Deserialize(XElement xml)
        {
            XmlReadHelper helper = new XmlReadHelper();
            object obj = FromXml_Object(helper, xml);
            return DeserializeWithHelper(helper, xml, obj);
        }

        /// <summary>
        /// 序列化对象
        /// </summary>
        /// <param name="helper">助手</param>
        /// <param name="obj">对象</param>
        /// <returns>Xml数据</returns>
        private static XElement SerializeWithHelper(XmlWriteHelper helper, object obj)
        {
            Type type = obj.GetType();
            if (!helper.DictSerializeFieldForType.ContainsKey(type))
            {
                helper.DictSerializeFieldForType.Add(type, GetSerializableFields(type));
            }
            IDictionary<string, (FieldInfo Field, EnumFrogSerializationType Type, string Comment)> dict = helper.DictSerializeFieldForType[type];
            XElement element = ToXml_Object(helper, obj);
            foreach (var item in dict)
            {
                switch (item.Value.Type)
                {
                    case EnumFrogSerializationType.NonSerialized:
                        {
                            Type fieldType = GetBaseSerializeType(item.Value.Field.FieldType);
                            if (!mListSerializeFunc.ContainsKey(fieldType))
                            {
                                throw new Exception("字段类型无法序列化。");
                            }
                            XElement fieldXml = mListSerializeFunc[fieldType].SerializeFunc(helper, item.Value.Field, item.Value.Field.GetValue(obj));
                            XComment comment = new XComment(item.Value.Comment);
                            element.Add(comment);
                            element.Add(fieldXml);
                        }
                        break;
                    case EnumFrogSerializationType.Serializable:
                        {
                            object val = item.Value.Field.GetValue(obj);
                            XElement fieldXml = GenerateXElement(Const_XmlNameEle_SerializableField,
                                ToXml_TypeSimple(helper, val.GetType()),
                                SerializeWithHelper(helper, val)
                                );
                        }
                        break;
                    default:
                        break;
                }
            }
            return null;
        }

        /// <summary>
        /// 反序列化对象
        /// </summary>
        /// <param name="helper">助手</param>
        /// <param name="xml">Xml数据</param>
        /// <param name="obj">对象</param>
        /// <returns>对象</returns>
        private static object DeserializeWithHelper(XmlReadHelper helper, XElement xml, object obj)
        {
            return null;
        }

        /// <summary>
        /// 获取序列化基础变量类型，此类型决定了具体的处理方式
        /// </summary>
        /// <param name="type">原始类型</param>
        /// <returns>转换类型</returns>
        private static Type GetBaseSerializeType(Type type)
        {
            foreach (var item in mListSerializeFunc)
            {
                if (type.IsAssignableFrom(item.Key))
                {
                    return item.Key;
                }
            }
            throw new Exception("无效的基本变量类型");
        }

        /// <summary>
        /// 获取可序列化字段字典
        /// </summary>
        /// <param name="type">处理类型</param>
        /// <returns>处理结果</returns>
        public static IDictionary<string, (FieldInfo Field, EnumFrogSerializationType Type, string Comment)> GetSerializableFields(Type type)
        {
            List<FieldInfo> fields = type.GetFields().ToList().Where(r => r.IsDefined(typeof(FrogSerializableAttribute), true)).ToList();
            Dictionary<string, (FieldInfo Field, EnumFrogSerializationType Type, string Comment)> dict = new Dictionary<string, (FieldInfo Field, EnumFrogSerializationType Type, string Comment)>();
            foreach (FieldInfo field in fields)
            {
                List<FrogSerializableAttribute> atts = field.GetCustomAttributes(true).Where(r=>r.GetType() == typeof(FrogSerializableAttribute)).Cast<FrogSerializableAttribute>().ToList();
                if (atts.Count == 0) continue;
                FrogSerializableAttribute att = atts.First();
                dict.Add(field.Name, (field, att.SerializeType, att.Comment));
            }
            return dict;
        }

        #endregion 序列化方法

        #region 类型处理方法

        #region Bool

        /// <summary>
        /// 序列化字段(Bool)
        /// </summary>
        /// <param name="helper">助手</param>
        /// <param name="field">字段</param>
        /// <param name="val">值</param>
        /// <returns>Xml数据</returns>
        private static XElement ToXml_Bool(XmlWriteHelper helper, FieldInfo field, object val)
        {
            return GenerateXElement(Const_XmlNameEle_NonSerializedField, 
                GenerateXAttribute(Const_XmlNameAtt_Field, field.Name),
                GenerateXAttribute(Const_XmlNameAtt_Value, val),
                ToXml_TypeSimple(helper, val.GetType())
                );
        }

        /// <summary>
        /// 反序列化字段(Bool)
        /// </summary>
        /// <param name="helper">助手</param>
        /// <param name="xml">Xml数据</param>
        /// <param name="obj">字段所属对象</param>
        /// <returns>Xml数据</returns>
        private static void FromXml_Bool(XmlReadHelper helper, XElement xml, object obj)
        {
            string fieldName = GetXAttribute(xml, Const_XmlNameAtt_Field).Value;
            FieldInfo field = obj.GetType().GetField(fieldName);
            XAttribute valAtt = GetXAttribute(xml, Const_XmlNameAtt_Value);
            field.SetValue(obj, (bool)valAtt);
        }

        #endregion Bool

        #region Byte

        /// <summary>
        /// 序列化字段(Byte)
        /// </summary>
        /// <param name="helper">助手</param>
        /// <param name="field">字段</param>
        /// <param name="val">值</param>
        /// <returns>Xml数据</returns>
        private static XElement ToXml_Byte(XmlWriteHelper helper, FieldInfo field, object val)
        {
            return GenerateXElement(Const_XmlNameEle_NonSerializedField,
                GenerateXAttribute(Const_XmlNameAtt_Field, field.Name),
                GenerateXAttribute(Const_XmlNameAtt_Value, val),
                ToXml_TypeSimple(helper, val.GetType())
                );
        }

        /// <summary>
        /// 反序列化字段(Byte)
        /// </summary>
        /// <param name="helper">助手</param>
        /// <param name="xml">Xml数据</param>
        /// <param name="obj">字段所属对象</param>
        /// <returns>Xml数据</returns>
        private static void FromXml_Byte(XmlReadHelper helper, XElement xml, object obj)
        {
            string fieldName = GetXAttribute(xml, Const_XmlNameAtt_Field).Value;
            FieldInfo field = obj.GetType().GetField(fieldName);
            XAttribute valAtt = GetXAttribute(xml, Const_XmlNameAtt_Value);
            field.SetValue(obj, byte.Parse(valAtt.Value));
        }

        #endregion Byte

        #region Char

        /// <summary>
        /// 序列化字段(Char)
        /// </summary>
        /// <param name="helper">助手</param>
        /// <param name="field">字段</param>
        /// <param name="val">值</param>
        /// <returns>Xml数据</returns>
        private static XElement ToXml_Char(XmlWriteHelper helper, FieldInfo field, object val)
        {
            return GenerateXElement(Const_XmlNameEle_NonSerializedField,
                GenerateXAttribute(Const_XmlNameAtt_Field, field.Name),
                GenerateXAttribute(Const_XmlNameAtt_Value, val),
                ToXml_TypeSimple(helper, val.GetType())
                );
        }

        /// <summary>
        /// 反序列化字段(Char)
        /// </summary>
        /// <param name="helper">助手</param>
        /// <param name="xml">Xml数据</param>
        /// <param name="obj">字段所属对象</param>
        /// <returns>Xml数据</returns>
        private static void FromXml_Char(XmlReadHelper helper, XElement xml, object obj)
        {
            string fieldName = GetXAttribute(xml, Const_XmlNameAtt_Field).Value;
            FieldInfo field = obj.GetType().GetField(fieldName);
            XAttribute valAtt = GetXAttribute(xml, Const_XmlNameAtt_Value);
            field.SetValue(obj, char.Parse(valAtt.Value));
        }

        #endregion Char

        #region Decimal

        /// <summary>
        /// 序列化字段(Decimal)
        /// </summary>
        /// <param name="helper">助手</param>
        /// <param name="field">字段</param>
        /// <param name="val">值</param>
        /// <returns>Xml数据</returns>
        private static XElement ToXml_Decimal(XmlWriteHelper helper, FieldInfo field, object val)
        {
            return GenerateXElement(Const_XmlNameEle_NonSerializedField,
                GenerateXAttribute(Const_XmlNameAtt_Field, field.Name),
                GenerateXAttribute(Const_XmlNameAtt_Value, val),
                ToXml_TypeSimple(helper, val.GetType())
                );
        }

        /// <summary>
        /// 反序列化字段(Decimal)
        /// </summary>
        /// <param name="helper">助手</param>
        /// <param name="xml">Xml数据</param>
        /// <param name="obj">字段所属对象</param>
        /// <returns>Xml数据</returns>
        private static void FromXml_Decimal(XmlReadHelper helper, XElement xml, object obj)
        {
            string fieldName = GetXAttribute(xml, Const_XmlNameAtt_Field).Value;
            FieldInfo field = obj.GetType().GetField(fieldName);
            XAttribute valAtt = GetXAttribute(xml, Const_XmlNameAtt_Value);
            field.SetValue(obj, decimal.Parse(valAtt.Value));
        }

        #endregion Decimal

        #region Double

        /// <summary>
        /// 序列化字段(Double)
        /// </summary>
        /// <param name="helper">助手</param>
        /// <param name="field">字段</param>
        /// <param name="val">值</param>
        /// <returns>Xml数据</returns>
        private static XElement ToXml_Double(XmlWriteHelper helper, FieldInfo field, object val)
        {
            return GenerateXElement(Const_XmlNameEle_NonSerializedField,
                GenerateXAttribute(Const_XmlNameAtt_Field, field.Name),
                GenerateXAttribute(Const_XmlNameAtt_Value, val),
                ToXml_TypeSimple(helper, val.GetType())
                );
        }

        /// <summary>
        /// 反序列化字段(Double)
        /// </summary>
        /// <param name="helper">助手</param>
        /// <param name="xml">Xml数据</param>
        /// <param name="obj">字段所属对象</param>
        /// <returns>Xml数据</returns>
        private static void FromXml_Double(XmlReadHelper helper, XElement xml, object obj)
        {
            string fieldName = GetXAttribute(xml, Const_XmlNameAtt_Field).Value;
            FieldInfo field = obj.GetType().GetField(fieldName);
            XAttribute valAtt = GetXAttribute(xml, Const_XmlNameAtt_Value);
            field.SetValue(obj, (double)valAtt);
        }

        #endregion Double

        #region Enum

        /// <summary>
        /// 序列化字段(Enum)
        /// </summary>
        /// <param name="helper">助手</param>
        /// <param name="field">字段</param>
        /// <param name="val">值</param>
        /// <returns>Xml数据</returns>
        private static XElement ToXml_Enum(XmlWriteHelper helper, FieldInfo field, object val)
        {
            return GenerateXElement(Const_XmlNameEle_NonSerializedField,
                GenerateXAttribute(Const_XmlNameAtt_Field, field.Name),
                GenerateXAttribute(Const_XmlNameAtt_Value, Enum.GetName(val.GetType(), val)),
                ToXml_TypeSimple(helper, val.GetType())
                );
        }

        /// <summary>
        /// 反序列化字段(Enum)
        /// </summary>
        /// <param name="helper">助手</param>
        /// <param name="xml">Xml数据</param>
        /// <param name="obj">字段所属对象</param>
        /// <returns>Xml数据</returns>
        private static void FromXml_Enum(XmlReadHelper helper, XElement xml, object obj)
        {
            string fieldName = GetXAttribute(xml, Const_XmlNameAtt_Field).Value;
            FieldInfo field = obj.GetType().GetField(fieldName);
            XAttribute valAtt = GetXAttribute(xml, Const_XmlNameAtt_Value);
            XAttribute typeXml = GetXAttribute(xml, Const_XmlNameAtt_Type);
            Type type = FromXml_TypeSimple(helper, typeXml);
            field.SetValue(obj, Enum.Parse(type, valAtt.Value));
        }

        #endregion Enum

        #region Float

        /// <summary>
        /// 序列化字段(Float)
        /// </summary>
        /// <param name="helper">助手</param>
        /// <param name="field">字段</param>
        /// <param name="val">值</param>
        /// <returns>Xml数据</returns>
        private static XElement ToXml_Float(XmlWriteHelper helper, FieldInfo field, object val)
        {
            return GenerateXElement(Const_XmlNameEle_NonSerializedField,
                GenerateXAttribute(Const_XmlNameAtt_Field, field.Name),
                GenerateXAttribute(Const_XmlNameAtt_Value, val),
                ToXml_TypeSimple(helper, val.GetType())
                );
        }

        /// <summary>
        /// 反序列化字段(Float)
        /// </summary>
        /// <param name="helper">助手</param>
        /// <param name="xml">Xml数据</param>
        /// <param name="obj">字段所属对象</param>
        /// <returns>Xml数据</returns>
        private static void FromXml_Float(XmlReadHelper helper, XElement xml, object obj)
        {
            string fieldName = GetXAttribute(xml, Const_XmlNameAtt_Field).Value;
            FieldInfo field = obj.GetType().GetField(fieldName);
            XAttribute valAtt = GetXAttribute(xml, Const_XmlNameAtt_Value);
            field.SetValue(obj, (float)valAtt);
        }

        #endregion Float

        #region Int

        /// <summary>
        /// 序列化字段(Int)
        /// </summary>
        /// <param name="helper">助手</param>
        /// <param name="field">字段</param>
        /// <param name="val">值</param>
        /// <returns>Xml数据</returns>
        private static XElement ToXml_Int(XmlWriteHelper helper, FieldInfo field, object val)
        {
            return GenerateXElement(Const_XmlNameEle_NonSerializedField,
                GenerateXAttribute(Const_XmlNameAtt_Field, field.Name),
                GenerateXAttribute(Const_XmlNameAtt_Value, val),
                ToXml_TypeSimple(helper, val.GetType())
                );
        }

        /// <summary>
        /// 反序列化字段(Int)
        /// </summary>
        /// <param name="helper">助手</param>
        /// <param name="xml">Xml数据</param>
        /// <param name="obj">字段所属对象</param>
        /// <returns>Xml数据</returns>
        private static void FromXml_Int(XmlReadHelper helper, XElement xml, object obj)
        {
            string fieldName = GetXAttribute(xml, Const_XmlNameAtt_Field).Value;
            FieldInfo field = obj.GetType().GetField(fieldName);
            XAttribute valAtt = GetXAttribute(xml, Const_XmlNameAtt_Value);
            field.SetValue(obj, (int)valAtt);
        }

        #endregion Int

        #region Long

        /// <summary>
        /// 序列化字段(Long)
        /// </summary>
        /// <param name="helper">助手</param>
        /// <param name="field">字段</param>
        /// <param name="val">值</param>
        /// <returns>Xml数据</returns>
        private static XElement ToXml_Long(XmlWriteHelper helper, FieldInfo field, object val)
        {
            return GenerateXElement(Const_XmlNameEle_NonSerializedField,
                GenerateXAttribute(Const_XmlNameAtt_Field, field.Name),
                GenerateXAttribute(Const_XmlNameAtt_Value, val),
                ToXml_TypeSimple(helper, val.GetType())
                );
        }

        /// <summary>
        /// 反序列化字段(Long)
        /// </summary>
        /// <param name="helper">助手</param>
        /// <param name="xml">Xml数据</param>
        /// <param name="obj">字段所属对象</param>
        /// <returns>Xml数据</returns>
        private static void FromXml_Long(XmlReadHelper helper, XElement xml, object obj)
        {
            string fieldName = GetXAttribute(xml, Const_XmlNameAtt_Field).Value;
            FieldInfo field = obj.GetType().GetField(fieldName);
            XAttribute valAtt = GetXAttribute(xml, Const_XmlNameAtt_Value);
            field.SetValue(obj,(long)valAtt);
        }

        #endregion Long

        #region SByte

        /// <summary>
        /// 序列化字段(SByte)
        /// </summary>
        /// <param name="helper">助手</param>
        /// <param name="field">字段</param>
        /// <param name="val">值</param>
        /// <returns>Xml数据</returns>
        private static XElement ToXml_SByte(XmlWriteHelper helper, FieldInfo field, object val)
        {
            return GenerateXElement(Const_XmlNameEle_NonSerializedField,
                GenerateXAttribute(Const_XmlNameAtt_Field, field.Name),
                GenerateXAttribute(Const_XmlNameAtt_Value, val),
                ToXml_TypeSimple(helper, val.GetType())
                );
        }

        /// <summary>
        /// 反序列化字段(SByte)
        /// </summary>
        /// <param name="helper">助手</param>
        /// <param name="xml">Xml数据</param>
        /// <param name="obj">字段所属对象</param>
        /// <returns>Xml数据</returns>
        private static void FromXml_SByte(XmlReadHelper helper, XElement xml, object obj)
        {
            string fieldName = GetXAttribute(xml, Const_XmlNameAtt_Field).Value;
            FieldInfo field = obj.GetType().GetField(fieldName);
            XAttribute valAtt = GetXAttribute(xml, Const_XmlNameAtt_Value);
            field.SetValue(obj, (sbyte)valAtt);
        }

        #endregion SByte

        #region Short

        /// <summary>
        /// 序列化字段(Short)
        /// </summary>
        /// <param name="helper">助手</param>
        /// <param name="field">字段</param>
        /// <param name="val">值</param>
        /// <returns>Xml数据</returns>
        private static XElement ToXml_Short(XmlWriteHelper helper, FieldInfo field, object val)
        {
            return GenerateXElement(Const_XmlNameEle_NonSerializedField,
                GenerateXAttribute(Const_XmlNameAtt_Field, field.Name),
                GenerateXAttribute(Const_XmlNameAtt_Value, val),
                ToXml_TypeSimple(helper, val.GetType())
                );
        }

        /// <summary>
        /// 反序列化字段(Short)
        /// </summary>
        /// <param name="helper">助手</param>
        /// <param name="xml">Xml数据</param>
        /// <param name="obj">字段所属对象</param>
        /// <returns>Xml数据</returns>
        private static void FromXml_Short(XmlReadHelper helper, XElement xml, object obj)
        {
            string fieldName = GetXAttribute(xml, Const_XmlNameAtt_Field).Value;
            FieldInfo field = obj.GetType().GetField(fieldName);
            XAttribute valAtt = GetXAttribute(xml, Const_XmlNameAtt_Value);
            field.SetValue(obj, (short)valAtt);
        }

        #endregion Short

        #region String

        /// <summary>
        /// 序列化字段(String)
        /// </summary>
        /// <param name="helper">助手</param>
        /// <param name="field">字段</param>
        /// <param name="val">值</param>
        /// <returns>Xml数据</returns>
        private static XElement ToXml_String(XmlWriteHelper helper, FieldInfo field, object val)
        {
            return GenerateXElement(Const_XmlNameEle_NonSerializedField,
                GenerateXAttribute(Const_XmlNameAtt_Field, field.Name),
                GenerateXAttribute(Const_XmlNameAtt_Value, val),
                ToXml_TypeSimple(helper, val.GetType())
                );
        }

        /// <summary>
        /// 反序列化字段(String)
        /// </summary>
        /// <param name="helper">助手</param>
        /// <param name="xml">Xml数据</param>
        /// <param name="obj">字段所属对象</param>
        /// <returns>Xml数据</returns>
        private static void FromXml_String(XmlReadHelper helper, XElement xml, object obj)
        {
            string fieldName = GetXAttribute(xml, Const_XmlNameAtt_Field).Value;
            FieldInfo field = obj.GetType().GetField(fieldName);
            XAttribute valAtt = GetXAttribute(xml, Const_XmlNameAtt_Value);
            field.SetValue(obj, valAtt.Value);
        }

        #endregion String

        #region UInt

        /// <summary>
        /// 序列化字段(UInt)
        /// </summary>
        /// <param name="helper">助手</param>
        /// <param name="field">字段</param>
        /// <param name="val">值</param>
        /// <returns>Xml数据</returns>
        private static XElement ToXml_UInt(XmlWriteHelper helper, FieldInfo field, object val)
        {
            return GenerateXElement(Const_XmlNameEle_NonSerializedField,
                GenerateXAttribute(Const_XmlNameAtt_Field, field.Name),
                GenerateXAttribute(Const_XmlNameAtt_Value, val),
                ToXml_TypeSimple(helper, val.GetType())
                );
        }

        /// <summary>
        /// 反序列化字段(UInt)
        /// </summary>
        /// <param name="helper">助手</param>
        /// <param name="xml">Xml数据</param>
        /// <param name="obj">字段所属对象</param>
        /// <returns>Xml数据</returns>
        private static void FromXml_UInt(XmlReadHelper helper, XElement xml, object obj)
        {
            string fieldName = GetXAttribute(xml, Const_XmlNameAtt_Field).Value;
            FieldInfo field = obj.GetType().GetField(fieldName);
            XAttribute valAtt = GetXAttribute(xml, Const_XmlNameAtt_Value);
            field.SetValue(obj, (uint)valAtt);
        }

        #endregion UInt

        #region ULong

        /// <summary>
        /// 序列化字段(ULong)
        /// </summary>
        /// <param name="helper">助手</param>
        /// <param name="field">字段</param>
        /// <param name="val">值</param>
        /// <returns>Xml数据</returns>
        private static XElement ToXml_ULong(XmlWriteHelper helper, FieldInfo field, object val)
        {
            return GenerateXElement(Const_XmlNameEle_NonSerializedField,
                GenerateXAttribute(Const_XmlNameAtt_Field, field.Name),
                GenerateXAttribute(Const_XmlNameAtt_Value, val),
                ToXml_TypeSimple(helper, val.GetType())
                );
        }

        /// <summary>
        /// 反序列化字段(ULong)
        /// </summary>
        /// <param name="helper">助手</param>
        /// <param name="xml">Xml数据</param>
        /// <param name="obj">字段所属对象</param>
        /// <returns>Xml数据</returns>
        private static void FromXml_ULong(XmlReadHelper helper, XElement xml, object obj)
        {
            string fieldName = GetXAttribute(xml, Const_XmlNameAtt_Field).Value;
            FieldInfo field = obj.GetType().GetField(fieldName);
            XAttribute valAtt = GetXAttribute(xml, Const_XmlNameAtt_Value);
            field.SetValue(obj, (ulong)valAtt);
        }

        #endregion ULong

        #region UShort

        /// <summary>
        /// 序列化字段(UShort)
        /// </summary>
        /// <param name="helper">助手</param>
        /// <param name="field">字段</param>
        /// <param name="val">值</param>
        /// <returns>Xml数据</returns>
        private static XElement ToXml_UShort(XmlWriteHelper helper, FieldInfo field, object val)
        {
            return GenerateXElement(Const_XmlNameEle_NonSerializedField,
                GenerateXAttribute(Const_XmlNameAtt_Field, field.Name),
                GenerateXAttribute(Const_XmlNameAtt_Value, val),
                ToXml_TypeSimple(helper, val.GetType())
                );
        }

        /// <summary>
        /// 反序列化字段(UShort)
        /// </summary>
        /// <param name="helper">助手</param>
        /// <param name="xml">Xml数据</param>
        /// <param name="obj">字段所属对象</param>
        /// <returns>Xml数据</returns>
        private static void FromXml_UShort(XmlReadHelper helper, XElement xml, object obj)
        {
            string fieldName = GetXAttribute(xml, Const_XmlNameAtt_Field).Value;
            FieldInfo field = obj.GetType().GetField(fieldName);
            XAttribute valAtt = GetXAttribute(xml, Const_XmlNameAtt_Value);
            field.SetValue(obj, ushort.Parse(valAtt.Value));
        }

        #endregion UShort

        #region Asset

        /// <summary>
        /// 序列化字段(Asset)
        /// </summary>
        /// <param name="helper">助手</param>
        /// <param name="field">字段</param>
        /// <param name="val">值</param>
        /// <returns>Xml数据</returns>
        private static XElement ToXml_Asset(XmlWriteHelper helper, FieldInfo field, object val)
        {
            if (!(val is UnityEngine.Object obj))
            {
                throw new Exception("错误的资源类型");
            }
            string path = AssetDatabase.GetAssetPath(obj);
            return GenerateXElement(Const_XmlNameEle_NonSerializedField,
                GenerateXAttribute(Const_XmlNameAtt_Field, field.Name),
                GenerateXAttribute(Const_XmlNameAtt_Value, path),
                ToXml_TypeSimple(helper, val.GetType())
                );
        }

        /// <summary>
        /// 反序列化字段(Asset)
        /// </summary>
        /// <param name="helper">助手</param>
        /// <param name="xml">Xml数据</param>
        /// <param name="obj">字段所属对象</param>
        /// <returns>Xml数据</returns>
        private static void FromXml_Asset(XmlReadHelper helper, XElement xml, object obj)
        {
            string fieldName = GetXAttribute(xml, Const_XmlNameAtt_Field).Value;
            FieldInfo field = obj.GetType().GetField(fieldName);
            XAttribute valAtt = GetXAttribute(xml, Const_XmlNameAtt_Value);
            UnityEngine.Object asset = AssetDatabase.LoadMainAssetAtPath(valAtt.Value);
            field.SetValue(obj, asset);
        }

        #endregion Asset

        #endregion 类型处理方法

        #region 反射相关

        /// <summary>
        /// 获取带有程序集名的类型全称
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>全称</returns>
        private static string TypeFullNameWithAssembly(Type type)
        {
            return $"{type.FullName},{type.Assembly.FullName}";
        }

        #endregion 反射相关

        #region 验证

        /// <summary>
        /// 验证元素名称
        /// </summary>
        /// <param name="xml">元素</param>
        /// <param name="name">名称</param>
        private static void CheckXelementName(XElement xml, XName name)
        {
            if (xml == null)
            {
                throw new Exception("Xml数据为空。");
            }
            if (xml.Name != name)
            {
                throw new Exception("元素名称错误。");
            }
        }

        #endregion 验证

        #region 生成Xml数据

        /// <summary>
        /// 生成Xml元素
        /// </summary>
        /// <param name="name">生成名称</param>
        /// <param name="content">包含内容</param>
        /// <returns>生成元素</returns>
        private static XElement GenerateXElement(XName name, params object[] content)
        {
            return new XElement(name, content);
        }

        /// <summary>
        /// 生成Xml属性
        /// </summary>
        /// <param name="name">生成名称</param>
        /// <param name="value">值</param>
        /// <returns>生成属性</returns>
        private static XAttribute GenerateXAttribute(XName name, object value)
        {
            return new XAttribute(name, value);
        }

        #endregion 生成Xml数据

        #region 获取Xml元素数据

        /// <summary>
        /// 获取唯一XML元素
        /// </summary>
        /// <param name="xml">所属元素</param>
        private static XElement GetOnlyValue(XElement xml)
        {
            IEnumerable<XElement> elements = xml.Elements();
            if (elements.Count() == 0)
            {
                throw new Exception("不允许属性不存在！");
            }
            if (elements.Count() >= 2)
            {
                throw new Exception("不允许多个结果！");
            }
            return elements.First();
        }

        /// <summary>
        /// 获取XML元素(多个)
        /// </summary>
        /// <param name="xml">所属元素</param>
        /// <param name="name">获取名称</param>
        /// <returns>结果元素</returns>
        private static IEnumerable<XElement> GetXElements(XElement xml, XName name)
        {
            return xml.Elements(name);
        }

        /// <summary>
        /// 获取模板值数据
        /// </summary>
        /// <param name="xml">Xml数据</param>
        /// <param name="keys">名称列表</param>
        /// <returns>模板Xml数据</returns>
        private static IDictionary<string, XElement> GetXElements(XElement xml, string[] keys)
        {
            IDictionary<string, XElement> dict = new Dictionary<string, XElement>();
            foreach (string key in keys)
            {
                dict.Add(key, GetXElement(xml, key));
            }
            return dict;
        }


        /// <summary>
        /// 获取XML元素
        /// </summary>
        /// <param name="xml">所属元素</param>
        /// <param name="name">获取名称</param>
        /// <param name="allowNull">允许不存在</param>
        /// <param name="getFirst">获取第一个</param>
        /// <remarks>如果getFirst为false，且有多个同名元素存在时抛出异常</remarks>
        /// <returns>结果元素</returns>
        private static XElement GetXElement(XElement xml, XName name, bool allowNull, bool getFirst)
        {
            List<XElement> elements = GetXElements(xml, name).ToList();
            if (elements.Count == 1)
            {
                return elements.First();
            }
            else if (elements.Count == 0)
            {
                if (!allowNull)
                {
                    throw new Exception("不允许元素不存在！");
                }
                return null;
            }
            else
            {
                if (!getFirst)
                {
                    throw new Exception("不允许多个结果！");
                }
                return elements.First();
            }
        }

        /// <summary>
        /// 获取XML元素
        /// </summary>
        /// <param name="xml">所属元素</param>
        /// <param name="name">获取名称</param>
        /// <returns>结果元素</returns>
        private static XElement GetXElement(XElement xml, XName name)
        {
            List<XElement> elements = GetXElements(xml, name).ToList();
            if (elements.Count() == 0)
            {
                throw new Exception("不允许属性不存在！");
            }
            if (elements.Count() >= 2)
            {
                throw new Exception("不允许多个结果！");
            }
            return elements.First();
        }

        /// <summary>
        /// 获取XML属性(多个)
        /// </summary>
        /// <param name="xml">所属元素</param>
        /// <param name="name">获取名称</param>
        /// <returns>结果属性</returns>
        private static IEnumerable<XAttribute> GetXAttributes(XElement xml, XName name)
        {
            return xml.Attributes(name);
        }

        /// <summary>
        /// 获取XML属性
        /// </summary>
        /// <param name="xml">所属元素</param>
        /// <param name="name">获取名称</param>
        /// <param name="allowNull">允许不存在</param>
        /// <param name="getFirst">获取第一个</param>
        /// <remarks>如果getFirst为false，且有多个同名属性存在时抛出异常</remarks>
        /// <returns>结果属性</returns>
        private static XAttribute GetXAttribute(XElement xml, XName name, bool allowNull, bool getFirst)
        {
            List<XAttribute> elements = GetXAttributes(xml, name).ToList();
            if (elements.Count == 1)
            {
                return elements.First();
            }
            else if (elements.Count == 0)
            {
                if (!allowNull)
                {
                    throw new Exception("不允许元素不存在！");
                }
                return null;
            }
            else
            {
                if (!getFirst)
                {
                    throw new Exception("不允许多个结果！");
                }
                return elements.First();
            }
        }

        /// <summary>
        /// 获取XML属性
        /// </summary>
        /// <param name="xml">所属元素</param>
        /// <param name="name">获取名称</param>
        /// <returns>结果属性</returns>
        private static XAttribute GetXAttribute(XElement xml, XName name)
        {
            List<XAttribute> elements = GetXAttributes(xml, name).ToList();
            if (elements.Count() == 0)
            {
                throw new Exception("不允许属性不存在！");
            }
            if (elements.Count() >= 2)
            {
                throw new Exception("不允许多个结果！");
            }
            return elements.First();
        }

        #endregion 获取Xml元素数据

        #region Type

        /// <summary>
        /// Type获取Xml
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="name">元素名称</param>
        /// <returns>类型Xml</returns>
        private static XElement ToXml_Type(Type type, XName name)
        {
            if (type.IsGenericType)
            {
                Type baseType = type.GetGenericTypeDefinition();
                XElement xml = GenerateXElement(name, GenerateXAttribute(Const_XmlNameAtt_Type, TypeFullNameWithAssembly(baseType)));
                foreach (Type select in type.GetGenericArguments())
                {
                    xml.Add(ToXml_Type(select));
                }
                return xml;
            }
            else
            {
                return GenerateXElement(name, GenerateXAttribute(Const_XmlNameAtt_Type, TypeFullNameWithAssembly(type)));
            }
        }

        /// <summary>
        /// Type获取Xml
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>类型Xml</returns>
        private static XElement ToXml_Type(Type type)
        {
            return ToXml_Type(type, Const_XmlNameEle_Type);
        }

        /// <summary>
        /// 生成Type的简单名称
        /// </summary>
        /// <param name="helper">助手</param>
        /// <param name="type">Type</param>
        /// <returns>类型Xml</returns>
        private static XAttribute ToXml_TypeSimple(XmlWriteHelper helper, Type type)
        {
            return GenerateXAttribute(Const_XmlNameAtt_Type, helper.GetTypeName(type));
        }

        /// <summary>
        /// Type来自Xml
        /// </summary>
        /// <param name="xml">类型Xml</param>
        /// <param name="name">元素名称</param>
        /// <returns>类型</returns>
        private static Type FromXml_Type(XElement xml, XName name)
        {
            CheckXelementName(xml, name);
            XAttribute att = GetXAttribute(xml, Const_XmlNameAtt_Type);
            IEnumerable<XElement> elements = GetXElements(xml, Const_XmlNameEle_Type);
            if (elements.Count() == 0)
            {
                return Type.GetType(att.Value);
            }
            else
            {
                Type baseType = Type.GetType(att.Value);
                Type[] paramTypes = elements.Where(r => r.Name == Const_XmlNameEle_Type).Select(r => FromXml_Type(r)).ToArray();
                return baseType.MakeGenericType(paramTypes);
            }
        }

        /// <summary>
        /// Type来自Xml
        /// </summary>
        /// <param name="helper">助手</param>
        /// <param name="xml">类型Xml</param>
        /// <returns>类型</returns>
        private static Type FromXml_Type(XElement xml)
        {
            return FromXml_Type(xml, Const_XmlNameEle_Type);
        }

        /// <summary>
        /// 读取Type的简单名称
        /// </summary>
        /// <param name="xml">类型Xml</param>
        /// <returns>类型</returns>
        private static Type FromXml_TypeSimple(XmlReadHelper helper, XAttribute xml)
        {
            return helper.GetType(xml.Value);
        }

        /// <summary>
        /// 生成类型信息
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private static XElement ToXml_TypeInfo(string name, Type type)
        {
            return GenerateXElement(
                    Const_XmlNameEle_TypeInfo,
                    GenerateXAttribute(
                        Const_XmlNameAtt_Name, name
                        ),
                    ToXml_Type(type)
                    );
        }

        /// <summary>
        /// 获取类型信息
        /// </summary>
        /// <param name="xml">文档Xml</param>
        /// <returns>类型信息</returns>
        private static (string Name, Type Type) FromXml_TypeInfo(XElement xml)
        {
            XAttribute nameAtt = GetXAttribute(xml, Const_XmlNameAtt_Name);
            XElement typeXml = GetXElement(xml, Const_XmlNameEle_Type);
            Type type = FromXml_Type(typeXml);
            return (nameAtt.Value, type);
        }

        #endregion Type

        #region Reference

        /// <summary>
        /// 获取引用值Xml结构
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns>XElement对象</returns>
        private static XElement ToXml_Reference(string path)
        {
            return GenerateXElement(Const_XmlNameEle_Object,
                GenerateXAttribute(Const_XmlNameAtt_Reference, path)
                );
        }

        /// <summary>
        /// 获取对象引用数据属性
        /// </summary>
        /// <param name="xml">数据父级</param>
        /// <returns>数据属性枚举</returns>
        private static XAttribute FromXml_Reference(XElement xml)
        {
            return GetXAttribute(xml, Const_XmlNameAtt_Reference, true, true);
        }

        #endregion Reference

        #region Object

        /// <summary>
        /// 获取对象XElement
        /// </summary>
        /// <param name="helper">助手</param>
        /// <param name="obj">对象</param>
        /// <returns>XEl对象</returns>
        private static XElement ToXml_Object(XmlWriteHelper helper, object obj)
        {
            Type type = obj.GetType();
            return GenerateXElement(Const_XmlNameEle_Object,
                ToXml_TypeSimple(helper, type)
                );
        }

        /// <summary>
        /// 实例化类
        /// </summary>
        /// <param name="helper">助手</param>
        /// <param name="xml">配置数据</param>
        /// <returns>实例对象</returns>
        private static object FromXml_Object(XmlReadHelper helper, XElement xml)
        {
            CheckXelementName(xml, Const_XmlNameEle_Object);
            XAttribute typeXML = GetXAttribute(xml, Const_XmlNameAtt_Type);
            Type type = FromXml_TypeSimple(helper, typeXML);
            return Activator.CreateInstance(type, Const_NoElementObjectArray); ;
        }

        #endregion Object

        #endregion 通用方法

        #region 重写方法

        #endregion 重写方法

        #region 事件方法

        #endregion 事件方法 

        #endregion 方法
    }

    #endregion 公共类

    #region 支持类

    #region 特性

    /// <summary>
    /// 可序列化字段特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class FrogSerializableAttribute : Attribute
    {
        #region 内部声明

        #region 常量

        #endregion 常量

        #region 枚举

        #endregion 枚举

        #region 定义

        #endregion 定义

        #region 委托

        #endregion 委托

        #endregion 内部声明

        #region 属性字段

        #region 静态属性

        #endregion 静态属性

        #region 属性

        /// <summary>
        /// 字段注释
        /// </summary>
        public string Comment { set; get; }

        /// <summary>
        /// 序列化类型
        /// </summary>
        public EnumFrogSerializationType SerializeType { set; get; }

        #endregion 属性

        #region 字段

        #endregion 字段

        #region 事件

        #endregion 事件

        #endregion 属性字段

        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="type">序列化类型</param>
        public FrogSerializableAttribute(EnumFrogSerializationType type)
        {
            SerializeType = type;
        }

        #endregion 构造函数

        #region 方法

        #region 通用方法

        #endregion 通用方法

        #region 重写方法

        #endregion 重写方法

        #region 事件方法

        #endregion 事件方法 

        #endregion 方法

    }

    #endregion 特性

    #endregion 支持类

    #endregion 蛤序列化支持
}
