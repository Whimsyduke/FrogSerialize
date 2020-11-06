using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;
using UnityEngine;
using UnityEditor;

namespace FrogSerialization
{
    #region 蛤序列化支持

    #region 公共声明

    #region 委托

    #endregion 委托

    #region 枚举

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

        #region 文档名称

        private const string Const_XmlNameDoc_Document = "Document";
        private const string Const_XmlNameDoc_TypeList = "TypeList";
        private const string Const_XmlNameDoc_ObjectList = "ObjectList";

        #endregion 文档名称

        #region 元素名称

        public const string Const_XmlNameEle_NonSerializedField = "NonSerializedField";
        public const string Const_XmlNameEle_NonSerializeObject = "NonSerializeObject";
        public const string Const_XmlNameEle_Null = "Null";
        public const string Const_XmlNameEle_Object = "Object";
        public const string Const_XmlNameEle_SerializableField = "SerializableField";
        public const string Const_XmlNameEle_Type = "Type";
        public const string Const_XmlNameEle_TypeInfo = "TypeInfo";
        public const string Const_XmlNameEle_KeyValuePair = "KeyValuePair";

        #endregion 元素名称

        #region 属性名称

        public const string Const_XmlNameAtt_Field = "Field";
        public const string Const_XmlNameAtt_Name = "Name";
        public const string Const_XmlNameAtt_Reference = "Reference";
        public const string Const_XmlNameAtt_Type = "Type";
        public const string Const_XmlNameAtt_Key = "Key";
        public const string Const_XmlNameAtt_Value = "Value";
        public const string Const_XmlNameAtt_Count = "Count";

        #endregion 属性名称

        #region 其它名称

        public const string Const_XmlNameOth_Dictionary = "Dictionary";
        public const string Const_XmlNameOth_List = "List";
        public const string Const_XmlNameOth_Array = "Array";

        #endregion 其它名称

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

        #region 素材类型

        /// <summary>
        /// 视为素材的类型，这个类型会尝试用Database查找对应文件
        /// </summary>
        private static readonly IList<Type> Const_ListAssetType = new List<Type>()
        {
            typeof(AudioClip),
            typeof(GameObject),
            typeof(Material),
            typeof(ScriptableObject),
            typeof(Texture2D),
            typeof(TextAsset),
        };

        #endregion 素材类型

        #endregion 常量

        #region 枚举

        #endregion 枚举

        #region 定义

        #region 数据结构体

        /// <summary>
        /// Xml类型信息
        /// </summary>
        private struct XmlTypeInfo
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
            /// 名称
            /// </summary>
            public string Name { set; get; }

            /// <summary>
            /// 类型
            /// </summary>
            public Type Type { set; get; }

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
            /// <param name="name">名称</param>
            /// <param name="type">类型</param>
            public XmlTypeInfo(string name, Type type)
            {
                Name = name;
                Type = type;
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

        /// <summary>
        /// 序列化方法
        /// </summary>
        private struct SerializationMethod
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
            /// 值类型
            /// </summary>
            public Type ValueType { set; get; }

            /// <summary>
            /// 序列化函数委托
            /// </summary>
            public Delegate_SerializeField SerializeFunc { set; get; }

            /// <summary>
            /// 反序列化函数委托
            /// </summary>
            public Delegate_DeserializeField DeserializeFunc { set; get; }

            /// <summary>
            /// 基本类型获取函数委托
            /// </summary>
            public Delegate_GetBaseType BaseTypeFunc { set; get; }

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
            /// <param name="valueType">值类型</param>
            /// <param name="serializeFunc">序列化函数委托</param>
            /// <param name="deserializeFunc">反序列化函数委托</param>
            /// <param name="baseTypeFunc">基本类型获取函数委托</param>
            public SerializationMethod(Type valueType, Delegate_SerializeField serializeFunc, Delegate_DeserializeField deserializeFunc, Delegate_GetBaseType baseTypeFunc)
            {
                ValueType = valueType;
                SerializeFunc = serializeFunc;
                DeserializeFunc = deserializeFunc;
                BaseTypeFunc = baseTypeFunc;
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

        /// <summary>
        /// 特性配置字段信息
        /// </summary>
        private struct FieldAttributeInfo
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
            /// 字段信息
            /// </summary>
            public FieldInfo Field { set; get; }

            /// <summary>
            /// 注释内容
            /// </summary>
            public string Comment { set; get; }

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
            /// <param name="field">字段信息</param>
            /// <param name="comment">注释内容</param>
            public FieldAttributeInfo(FieldInfo field, string comment)
            {
                Field = field;
                Comment = comment;
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

        #endregion 数据结构体

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
            public Stack<string> StackCurrentPath { private set;  get; }

            public Dictionary<Type, IDictionary<string, FieldAttributeInfo>> DictSerializeFieldForType { private set;  get; }

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
                StackCurrentPath = new Stack<string>();
                StackCurrentPath.Push("-");
                DictSerializeFieldForType = new Dictionary<Type, IDictionary<string, FieldAttributeInfo>>();
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
            public abstract void AddObjeInPath(object initObj);

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
            ListLog.AppendLine(StackCurrentPath.Peek() + "/" + name);
#endif //SDP_HELPER_LOG
                StackCurrentPath.Push(StackCurrentPath.Peek() + "/" + name);
            }

            /// <summary>
            ///  弹出名称
            /// </summary>
            public void PopName()
            {
                StackCurrentPath.Pop();
            }

            /// <summary>
            /// 最后的名称
            /// </summary>
            /// <returns></returns>
            public string PeekName()
            {
                return StackCurrentPath.Peek();
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

            /// <summary>
            /// 字典类型信息
            /// </summary>
            struct DictTypeInfo
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
                /// 名称
                /// </summary>
                public string Name { set; get; }

                /// <summary>
                /// 定义
                /// </summary>
                public bool IsDefinition { set; get; }

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
                /// <param name="name"></param>
                /// <param name="isDefinition"></param>
                public DictTypeInfo(string name, bool isDefinition)
                {
                    Name = name;
                    IsDefinition = isDefinition;
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
            private readonly Dictionary<Type, DictTypeInfo> mDictType = new Dictionary<Type, DictTypeInfo>();

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
            public override void AddObjeInPath(object initObj)
            {
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
                    return ToXml_Reference("");
                }
                else if (mDictValuePath.ContainsKey(value))
                {
                    return ToXml_Reference(mDictValuePath[value]);
                }
                else
                {
                    return SerializeWithHelper(this, value);
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
                    index++;
                    name = baseName + "#" + index;
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
                        mDictType.Add(type, new DictTypeInfo(GetSimpleName(type), true));
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
                        mDictType.Add(type, new DictTypeInfo(builder.ToString(), false));
                    }
                    else
                    {
                        mDictType.Add(type, new DictTypeInfo(GetSimpleName(type), false));
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
                    xml.Add(ToXml_TypeInfo(select.Value.Name, select.Key));
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
            public override void AddObjeInPath(object initObj)
            {
                mDictValuePath[StackCurrentPath.Peek()] = initObj;
            }

            /// <summary>
            /// 获取Xml对应的值
            /// </summary>
            /// <param name="xml">Xml</param>
            /// <param name="val">对象，为空时自动新建，否则修改字段值</param>
            /// <remarks>引用的路径值为空时，对象为null。</remarks>
            public void GetXmlValue(XElement xml, ref object val)
            {
                XAttribute attRef = FromXml_Reference(xml);
                if (attRef != null)
                {
                    // 值是一个引用
                    if (string.IsNullOrEmpty(attRef.Value))
                    {
                        val = null;
                    }
                    else
                    {
                        if (!mDictValuePath.ContainsKey(attRef.Value))
                        {
                            throw new Exception("引用对象不存在或尚未解析。");
                        }
                        val = mDictValuePath[attRef.Value];
                    }
                }
                else
                {
                    // 值为object
                    if (val == null) val = FromXml_Object(this, xml);
                    mDictValuePath[StackCurrentPath.Peek()] = val;
                    DeserializeWithHelper(this, xml, ref val);
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
                IEnumerable<XElement> elements = GetXElements(xml, Const_XmlNameEle_TypeInfo);
                foreach (XElement select in elements)
                {
                    XmlTypeInfo info = FromXml_TypeInfo(select);
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
        /// <param name="val">字段所属对象</param>
        /// <returns>Xml数据</returns>
        private delegate void Delegate_DeserializeField(XmlReadHelper helper, XElement xml, ref object val);

        /// <summary>
        /// 获取序列化支持基础类型，不匹配时序返回Null
        /// </summary>
        /// <param name="type">字段所属对象</param>
        /// <returns>基础类型</returns>
        private delegate Type Delegate_GetBaseType(Type type);

        #endregion 委托

        #endregion 内部声明

        #region 属性字段

        #region 静态属性

        #endregion 静态属性

        #region 属性

        /// <summary>
        /// 打印路径注释
        /// </summary>
        public static bool PrintCommentPath { set; get; }

        /// <summary>
        /// 打印自定义注释
        /// </summary>
        public static bool PrintCustomPath { set; get; }

        #endregion 属性

        #region 字段

        /// <summary>
        /// 基本类型序列化方法
        /// </summary>
        private static readonly Dictionary<Type, SerializationMethod> mListSerializeFunc =
            new Dictionary<Type, SerializationMethod>()
            {
                { typeof(bool), new SerializationMethod(typeof(bool), ToXml_Bool, FromXml_Bool, Type_GetBaseBool) },
                { typeof(byte), new SerializationMethod(typeof(byte), ToXml_Byte, FromXml_Byte, Type_GetBaseByte) },
                { typeof(char), new SerializationMethod(typeof(char), ToXml_Char, FromXml_Char, Type_GetBaseChar) },
                { typeof(decimal), new SerializationMethod(typeof(decimal), ToXml_Decimal, FromXml_Decimal, Type_GetBaseDecimal) },
                { typeof(double), new SerializationMethod(typeof(double), ToXml_Double, FromXml_Double, Type_GetBaseDouble) },
                { typeof(Enum), new SerializationMethod(typeof(Enum), ToXml_Enum, FromXml_Enum, Type_GetBaseEnum) },
                { typeof(float), new SerializationMethod(typeof(float), ToXml_Float, FromXml_Float, Type_GetBaseFloat) },
                { typeof(int), new SerializationMethod(typeof(int), ToXml_Int, FromXml_Int, Type_GetBaseInt) },
                { typeof(long), new SerializationMethod(typeof(long), ToXml_Long, FromXml_Long, Type_GetBaseLong) },
                { typeof(sbyte), new SerializationMethod(typeof(sbyte), ToXml_SByte, FromXml_SByte, Type_GetBaseSByte) },
                { typeof(short), new SerializationMethod(typeof(short), ToXml_Short, FromXml_Short, Type_GetBaseShort) },
                { typeof(string), new SerializationMethod(typeof(string), ToXml_String, FromXml_String, Type_GetBaseString) },
                { typeof(uint), new SerializationMethod(typeof(uint), ToXml_UInt, FromXml_UInt, Type_GetBaseUInt) },
                { typeof(ulong), new SerializationMethod(typeof(ulong), ToXml_ULong, FromXml_ULong, Type_GetBaseULong) },
                { typeof(ushort), new SerializationMethod(typeof(ushort), ToXml_UShort, FromXml_UShort, Type_GetBaseUShort) },
                { typeof(UnityEngine.Object), new SerializationMethod(typeof(UnityEngine.Object), ToXml_Asset, FromXml_Asset, Type_GetBaseAsset) },
                { typeof(Array), new SerializationMethod(typeof(Array), ToXml_Array, FromXml_Array, Type_GetBaseArray) },
                { typeof(IList), new SerializationMethod(typeof(IList), ToXml_List, FromXml_List, Type_GetBaseList) },
                { typeof(IDictionary), new SerializationMethod(typeof(IDictionary), ToXml_Dictionary, FromXml_Dictionary, Type_GetBaseDictionary) },
            };

        #endregion 字段

        #region 事件

        #endregion 事件

        #endregion 属性字段

        #region 构造函数

        /// <summary>
        /// 静态构造函数
        /// </summary>
        static FrogSerialization()
        {
            PrintCommentPath = false;
            PrintCustomPath = true;
        }

        #endregion 构造函数

        #region 方法

        #region 通用方法

        #region 序列化方法

        /// <summary>
        /// 序列化对象，只处理带有<see cref="FrogSerializableAttribute"/>特性的字段。可以在特性中使用<see cref="FrogSerializableAttribute.Comment"/>属性添加注释。
        /// 字段支持基本类型，资源类型（<see cref="Const_ListAssetType"/>），以及其它可序列化类型。
        /// 基础类型目前包括bool，byte，char，decimal，double，enum（所有类型，以值名称存储），float，int，long，sbyte，short，string，uint，ulong，ushort。
        /// 如需新增基础类型，参考<see cref="mListSerializeFunc"/>的列表数据。
        /// 素材类型需支持<see cref="AssetDatabase.GetAssetPath(UnityEngine.Object)"/>方法查找。
        /// 可序列化类型将会序列化其带有<see cref="FrogSerializableAttribute"/>特性的字段。并需要支持无参数<see cref="Activator.CreateInstance(Type, object[])"/>创建实例。
        /// 对于支持<see cref="List{T}"/>，<see cref="Dictionary{TKey, TValue}"/>，以及一维数组（支持交错数组）。对于其中元素的处理方式如上。
        /// 如实现了继承<see cref="List{T}"/>或<see cref="Dictionary{TKey, TValue}"/>的类，请自行添加处理方法，参考<see cref="mListSerializeFunc"/>的列表数据。
        /// 可以通过设置<see cref="PrintCommentPath"/>来决定是否打印数据的内部路径（用于同一序列化类型对象的位置的标记），默认为False。
        /// 可以通过<see cref="PrintCustomPath"/>来决定是否打印自定义注释，自定义注释通过<see cref="FrogSerializableAttribute.Comment"/>属性添加。默认为True。
        /// </summary>
        /// <param name="val">对象</param>
        /// <returns>Xml数据</returns>
        public static XElement Serialize(object val)
        {
            XmlWriteHelper helper = new XmlWriteHelper();
            XElement doc = ToXml_Document();
            XElement xmlTypeList = FromXml_TypeList(doc);
            XElement xmlObjList = FromXml_ObjectList(doc);
            xmlObjList.Add(SerializeWithHelper(helper, val));
            helper.GenerateTypeList(xmlTypeList);
            return doc;
        }

        /// <summary>
        /// 反序列化对象
        /// </summary>
        /// <param name="xml">Xml数据</param>
        /// <param name="val">对象，为空时自动新建，否则修改字段值</param>
        /// <returns>对象</returns>
        public static void Deserialize(XElement xml, ref object val)
        {
            CheckXelementName(xml, Const_XmlNameDoc_Document);
            XmlReadHelper helper = new XmlReadHelper();
            XElement xmlTypeList = FromXml_TypeList(xml);
            XElement xmlObjList = FromXml_ObjectList(xml);
            helper.InitTypeList(xmlTypeList);
            XElement ObjXml = GetXElement(xmlObjList, Const_XmlNameEle_Object);
            if (val != null) DeserializeWithHelper(helper, ObjXml, ref val);
            DeserializeWithHelper(helper, ObjXml, ref val);
        }

        /// <summary>
        /// 序列化对象
        /// </summary>
        /// <param name="helper">助手</param>
        /// <param name="val">对象</param>
        /// <returns>Xml数据</returns>
        private static XElement SerializeWithHelper_Simple(XmlWriteHelper helper, object val)
        {
            XElement element = ToXml_Object(helper, val);
            if (val != null)
            {
                Type type = val.GetType();
                Type baseType = GetBaseSerializeType(type);
                element.Add(mListSerializeFunc[type].SerializeFunc(helper, null, val));
            }
            return element;
        }

        /// <summary>
        /// 反序列化对象
        /// </summary>
        /// <param name="helper">助手</param>
        /// <param name="xml">Xml数据</param>
        /// <param name="type">对象类型</param>
        /// <returns>对象</returns>
        private static object DeserializeWithHelper_Simple(XmlReadHelper helper, XElement xml, Type type)
        {
            Type baseType = GetBaseSerializeType(type);
            XElement valXml = GetXElement(xml, Const_XmlNameEle_NonSerializeObject);
            object val = null;
            mListSerializeFunc[baseType].DeserializeFunc(helper, valXml, ref val);
            return val;
        }

        /// <summary>
        /// 序列化对象
        /// </summary>
        /// <param name = "helper" > 助手 </ param >
        /// < param name="val">对象</param>
        /// <returns>Xml数据</returns>
        private static XElement SerializeWithHelper(XmlWriteHelper helper, object val)
        {
            Type type = val.GetType();
            Type baseType = GetBaseSerializeType(type);
            if (baseType == null)
            {
                helper.AddObjeInPath(val);
                XElement element = ToXml_Object(helper, val);
                if (val != null)
                {
                    if (!helper.DictSerializeFieldForType.ContainsKey(type))
                    {
                        helper.DictSerializeFieldForType.Add(type, GetSerializableFields(type));
                    }
                    IDictionary<string, FieldAttributeInfo> dict = helper.DictSerializeFieldForType[type];
                    foreach (var item in dict)
                    {
                        helper.PushName(item.Value.Field.Name);
                        if (PrintCommentPath)
                        {
                            XComment comment = new XComment("路径：" + helper.PeekName());
                            element.Add(comment);
                        }
                        if (PrintCustomPath)
                        {
                            XComment comment = new XComment("注释：" + item.Value.Comment); 
                            element.Add(comment);
                        }
                        object obj = item.Value.Field.GetValue(val);
                        Type baseFieldType = GetBaseSerializeType(item.Value.Field.FieldType);
                        if (baseFieldType == null)
                        {
                            XElement field = GenerateXElement(Const_XmlNameEle_SerializableField,
                                GenerateXAttribute(Const_XmlNameAtt_Field, item.Value.Field.Name),
                                obj == null ? null : helper.GetValueXml(obj));
                            element.Add(field);
                        }
                        else
                        {
                            element.Add(mListSerializeFunc[baseFieldType].SerializeFunc(helper, item.Value.Field, obj));
                        }
                        helper.PopName();
                    }
                }
                return element;
            }
            else
            {
                return SerializeWithHelper_Simple(helper, val);
            }
        }

        /// <summary>
        /// 反序列化对象
        /// </summary>
        /// <param name="helper">助手</param>
        /// <param name="xml">Xml数据</param>
        /// <param name="val">对象，为空时自动新建，否则修改字段值</param>
        /// <returns>对象</returns>
        private static void DeserializeWithHelper(XmlReadHelper helper, XElement xml, ref object val)
        {
            XAttribute typeAtt = GetXAttribute(xml, Const_XmlNameAtt_Type);
            Type objType = FromXml_TypeSimple(helper, typeAtt);
            Type baseObjType = GetBaseSerializeType(objType);
            if (baseObjType == null)
            {
                if (val != null && val.GetType() != objType)
                {
                    throw new Exception("给定的对象和xml的变量类型不匹配");
                }
                if (val == null)
                {
                    val = FromXml_Object(helper, xml);
                    // 当前值为空
                    if (val == null) return;
                }
                helper.AddObjeInPath(val);
                if (!helper.DictSerializeFieldForType.ContainsKey(objType))
                {
                    helper.DictSerializeFieldForType.Add(objType, GetSerializableFields(objType));
                }
                IDictionary<string, FieldAttributeInfo> dict = helper.DictSerializeFieldForType[objType];
                IEnumerable<XElement> fieldXml = GetXElements(xml, Const_XmlNameEle_NonSerializedField);
                foreach (XElement field in fieldXml)
                {
                    XAttribute nameAtt = GetXAttribute(field, Const_XmlNameAtt_Field);
                    if (!dict.ContainsKey(nameAtt.Value))
                    {
                        throw new Exception("字段没有标记为可序列化。");
                    }
                    helper.PushName(nameAtt.Value);
                    XAttribute valTypeAtt = GetXAttribute(field, Const_XmlNameAtt_Type);
                    Type valType = FromXml_TypeSimple(helper, valTypeAtt);
                    Type varTypeBase = GetBaseSerializeType(valType);
                    mListSerializeFunc[varTypeBase].DeserializeFunc(helper, field, ref val);
                    helper.PopName();
                }
                fieldXml = GetXElements(xml, Const_XmlNameEle_SerializableField);
                foreach (XElement field in fieldXml)
                {
                    XAttribute nameAtt = GetXAttribute(field, Const_XmlNameAtt_Field);
                    if (!dict.ContainsKey(nameAtt.Value))
                    {
                        throw new Exception("字段没有标记为可序列化。");
                    }
                    helper.PushName(nameAtt.Value);
                    XElement valXml = GetXElement(field, Const_XmlNameEle_Object, true, true);
                    object value = null;
                    if (valXml != null) helper.GetXmlValue(valXml, ref value);
                    dict[nameAtt.Value].Field.SetValue(val, value);
                    helper.PopName();
                }
            }
            else
            {
                val = DeserializeWithHelper_Simple(helper, xml, objType);
            }
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
                Type baseType = item.Value.BaseTypeFunc(type);
                if (baseType != null) return baseType;
            }
            return null;
        }

        /// <summary>
        /// 获取可序列化字段字典
        /// </summary>
        /// <param name="type">处理类型</param>
        /// <returns>处理结果</returns>
        private static IDictionary<string, FieldAttributeInfo> GetSerializableFields(Type type)
        {
            List<FieldInfo> fields = type.GetFields().ToList().Where(r => r.IsDefined(typeof(FrogSerializableAttribute), true)).ToList();
            Dictionary<string, FieldAttributeInfo> dict = new Dictionary<string, FieldAttributeInfo>();
            foreach (FieldInfo field in fields)
            {
                List<FrogSerializableAttribute> atts = field.GetCustomAttributes(true).Where(r => r.GetType() == typeof(FrogSerializableAttribute)).Cast<FrogSerializableAttribute>().ToList();
                if (atts.Count == 0) continue;
                FrogSerializableAttribute att = atts.First();
                dict.Add(field.Name, new FieldAttributeInfo(field, att.Comment));
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
            return GenerateXElement(field != null ? Const_XmlNameEle_NonSerializedField : Const_XmlNameEle_NonSerializeObject,
                field != null ? GenerateXAttribute(Const_XmlNameAtt_Field, field.Name) : null,
                GenerateXAttribute(Const_XmlNameAtt_Value, val),
                ToXml_TypeSimple(helper, field != null ? field.FieldType : val.GetType())
                );
        }

        /// <summary>
        /// 反序列化字段(Bool)
        /// </summary>
        /// <param name="helper">助手</param>
        /// <param name="xml">Xml数据</param>
        /// <param name="val">字段所属对象</param>
        /// <returns>Xml数据</returns>
        private static void FromXml_Bool(XmlReadHelper helper, XElement xml, ref object val)
        {
            XAttribute fieldAtt = GetXAttribute(xml, Const_XmlNameAtt_Field, true, true);
            if (fieldAtt != null)
            {
                string fieldName = fieldAtt.Value;
                FieldInfo field = val.GetType().GetField(fieldName);
                XAttribute valAtt = GetXAttribute(xml, Const_XmlNameAtt_Value);
                field.SetValue(val, (bool)valAtt);
            }
            else
            {
                XAttribute valAtt = GetXAttribute(xml, Const_XmlNameAtt_Value);
                val = (bool)valAtt;
            }
        }

        /// <summary>
        /// 获取基本类型Bool
        /// </summary>
        /// <param name="type">字段所属对象</param>
        /// <returns>基础类型</returns>
        private static Type Type_GetBaseBool(Type type)
        {
            return type == typeof(bool) ? type : null;
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
            return GenerateXElement(field != null ? Const_XmlNameEle_NonSerializedField : Const_XmlNameEle_NonSerializeObject,
                field != null ? GenerateXAttribute(Const_XmlNameAtt_Field, field.Name) : null,
                GenerateXAttribute(Const_XmlNameAtt_Value, val),
                ToXml_TypeSimple(helper, field != null ? field.FieldType : val.GetType())
                );
        }

        /// <summary>
        /// 反序列化字段(Byte)
        /// </summary>
        /// <param name="helper">助手</param>
        /// <param name="xml">Xml数据</param>
        /// <param name="val">字段所属对象</param>
        /// <returns>Xml数据</returns>
        private static void FromXml_Byte(XmlReadHelper helper, XElement xml, ref object val)
        {
            XAttribute fieldAtt = GetXAttribute(xml, Const_XmlNameAtt_Field, true, true);
            if (fieldAtt != null)
            {
                string fieldName = fieldAtt.Value;
                FieldInfo field = val.GetType().GetField(fieldName);
                XAttribute valAtt = GetXAttribute(xml, Const_XmlNameAtt_Value);
                field.SetValue(val, byte.Parse(valAtt.Value));
            }
            else
            {
                XAttribute valAtt = GetXAttribute(xml, Const_XmlNameAtt_Value);
                val = byte.Parse(valAtt.Value);
            }
        }

        /// <summary>
        /// 获取基本类型Byte
        /// </summary>
        /// <param name="type">字段所属对象</param>
        /// <returns>基础类型</returns>
        private static Type Type_GetBaseByte(Type type)
        {
            return type == typeof(byte) ? type : null;
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
            return GenerateXElement(field != null ? Const_XmlNameEle_NonSerializedField : Const_XmlNameEle_NonSerializeObject,
                field != null ? GenerateXAttribute(Const_XmlNameAtt_Field, field.Name) : null,
                GenerateXAttribute(Const_XmlNameAtt_Value, val),
                ToXml_TypeSimple(helper, field != null ? field.FieldType : val.GetType())
                );
        }

        /// <summary>
        /// 反序列化字段(Char)
        /// </summary>
        /// <param name="helper">助手</param>
        /// <param name="xml">Xml数据</param>
        /// <param name="val">字段所属对象</param>
        /// <returns>Xml数据</returns>
        private static void FromXml_Char(XmlReadHelper helper, XElement xml, ref object val)
        {
            XAttribute fieldAtt = GetXAttribute(xml, Const_XmlNameAtt_Field, true, true);
            if (fieldAtt != null)
            {
                string fieldName = fieldAtt.Value;
                FieldInfo field = val.GetType().GetField(fieldName);
                XAttribute valAtt = GetXAttribute(xml, Const_XmlNameAtt_Value);
                field.SetValue(val, char.Parse(valAtt.Value));
            }
            else
            {
                XAttribute valAtt = GetXAttribute(xml, Const_XmlNameAtt_Value);
                val = char.Parse(valAtt.Value);
            }
        }

        /// <summary>
        /// 获取基本类型Char
        /// </summary>
        /// <param name="type">字段所属对象</param>
        /// <returns>基础类型</returns>
        private static Type Type_GetBaseChar(Type type)
        {
            return type == typeof(char) ? type : null;
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
            return GenerateXElement(field != null ? Const_XmlNameEle_NonSerializedField : Const_XmlNameEle_NonSerializeObject,
                field != null ? GenerateXAttribute(Const_XmlNameAtt_Field, field.Name) : null,
                GenerateXAttribute(Const_XmlNameAtt_Value, val),
                ToXml_TypeSimple(helper, field != null ? field.FieldType : val.GetType())
                );
        }

        /// <summary>
        /// 反序列化字段(Decimal)
        /// </summary>
        /// <param name="helper">助手</param>
        /// <param name="xml">Xml数据</param>
        /// <param name="val">字段所属对象</param>
        /// <returns>Xml数据</returns>
        private static void FromXml_Decimal(XmlReadHelper helper, XElement xml, ref object val)
        {
            XAttribute fieldAtt = GetXAttribute(xml, Const_XmlNameAtt_Field, true, true);
            if (fieldAtt != null)
            {
                string fieldName = fieldAtt.Value;
                FieldInfo field = val.GetType().GetField(fieldName);
                XAttribute valAtt = GetXAttribute(xml, Const_XmlNameAtt_Value);
                field.SetValue(val, decimal.Parse(valAtt.Value));
            }
            else
            {
                XAttribute valAtt = GetXAttribute(xml, Const_XmlNameAtt_Value);
                val = decimal.Parse(valAtt.Value);
            }
        }

        /// <summary>
        /// 获取基本类型Decimal
        /// </summary>
        /// <param name="type">字段所属对象</param>
        /// <returns>基础类型</returns>
        private static Type Type_GetBaseDecimal(Type type)
        {
            return type == typeof(decimal) ? type : null;
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
            return GenerateXElement(field != null ? Const_XmlNameEle_NonSerializedField : Const_XmlNameEle_NonSerializeObject,
                field != null ? GenerateXAttribute(Const_XmlNameAtt_Field, field.Name) : null,
                GenerateXAttribute(Const_XmlNameAtt_Value, val),
                ToXml_TypeSimple(helper, field != null ? field.FieldType : val.GetType())
                );
        }

        /// <summary>
        /// 反序列化字段(Double)
        /// </summary>
        /// <param name="helper">助手</param>
        /// <param name="xml">Xml数据</param>
        /// <param name="val">字段所属对象</param>
        /// <returns>Xml数据</returns>
        private static void FromXml_Double(XmlReadHelper helper, XElement xml, ref object val)
        {
            XAttribute fieldAtt = GetXAttribute(xml, Const_XmlNameAtt_Field, true, true);
            if (fieldAtt != null)
            {
                string fieldName = fieldAtt.Value;
                FieldInfo field = val.GetType().GetField(fieldName);
                XAttribute valAtt = GetXAttribute(xml, Const_XmlNameAtt_Value);
                field.SetValue(val, (double)valAtt);
            }
            else
            {
                XAttribute valAtt = GetXAttribute(xml, Const_XmlNameAtt_Value);
                val = (double)valAtt;
            }
        }

        /// <summary>
        /// 获取基本类型Double
        /// </summary>
        /// <param name="type">字段所属对象</param>
        /// <returns>基础类型</returns>
        private static Type Type_GetBaseDouble(Type type)
        {
            return type == typeof(double) ? type : null;
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
            return GenerateXElement(field != null ? Const_XmlNameEle_NonSerializedField : Const_XmlNameEle_NonSerializeObject,
                field != null ? GenerateXAttribute(Const_XmlNameAtt_Field, field.Name) : null,
                GenerateXAttribute(Const_XmlNameAtt_Value, Enum.GetName(val.GetType(), val)),
                ToXml_TypeSimple(helper, field != null ? field.FieldType : val.GetType())
                );
        }

        /// <summary>
        /// 反序列化字段(Enum)
        /// </summary>
        /// <param name="helper">助手</param>
        /// <param name="xml">Xml数据</param>
        /// <param name="val">字段所属对象</param>
        /// <returns>Xml数据</returns>
        private static void FromXml_Enum(XmlReadHelper helper, XElement xml, ref object val)
        {
            XAttribute fieldAtt = GetXAttribute(xml, Const_XmlNameAtt_Field, true, true);
            if (fieldAtt != null)
            {
                string fieldName = fieldAtt.Value;
                FieldInfo field = val.GetType().GetField(fieldName);
                XAttribute valAtt = GetXAttribute(xml, Const_XmlNameAtt_Value);
                XAttribute typeXml = GetXAttribute(xml, Const_XmlNameAtt_Type);
                Type type = FromXml_TypeSimple(helper, typeXml);
                field.SetValue(val, Enum.Parse(type, valAtt.Value));
            }
            else
            {
                XAttribute valAtt = GetXAttribute(xml, Const_XmlNameAtt_Value);
                XAttribute typeXml = GetXAttribute(xml, Const_XmlNameAtt_Type);
                Type type = FromXml_TypeSimple(helper, typeXml);
                val = Enum.Parse(type, valAtt.Value);
            }
        }

        /// <summary>
        /// 获取基本类型Enum
        /// </summary>
        /// <param name="type">字段所属对象</param>
        /// <returns>基础类型</returns>
        private static Type Type_GetBaseEnum(Type type)
        {
            return typeof(Enum).IsAssignableFrom(type) ? typeof(Enum) : null;
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
            return GenerateXElement(field != null ? Const_XmlNameEle_NonSerializedField : Const_XmlNameEle_NonSerializeObject,
                field != null ? GenerateXAttribute(Const_XmlNameAtt_Field, field.Name) : null,
                GenerateXAttribute(Const_XmlNameAtt_Value, val),
                ToXml_TypeSimple(helper, field != null ? field.FieldType : val.GetType())
                );
        }

        /// <summary>
        /// 反序列化字段(Float)
        /// </summary>
        /// <param name="helper">助手</param>
        /// <param name="xml">Xml数据</param>
        /// <param name="val">字段所属对象</param>
        /// <returns>Xml数据</returns>
        private static void FromXml_Float(XmlReadHelper helper, XElement xml, ref object val)
        {
            XAttribute fieldAtt = GetXAttribute(xml, Const_XmlNameAtt_Field, true, true);
            if (fieldAtt != null)
            {
                string fieldName = fieldAtt.Value;
                FieldInfo field = val.GetType().GetField(fieldName);
                XAttribute valAtt = GetXAttribute(xml, Const_XmlNameAtt_Value);
                field.SetValue(val, (float)valAtt);
            }
            else
            {
                XAttribute valAtt = GetXAttribute(xml, Const_XmlNameAtt_Value);
                val = (float)valAtt;
            }
        }

        /// <summary>
        /// 获取基本类型Float
        /// </summary>
        /// <param name="type">字段所属对象</param>
        /// <returns>基础类型</returns>
        private static Type Type_GetBaseFloat(Type type)
        {
            return type == typeof(float) ? type : null;
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
            return GenerateXElement(field != null ? Const_XmlNameEle_NonSerializedField : Const_XmlNameEle_NonSerializeObject,
                field != null ? GenerateXAttribute(Const_XmlNameAtt_Field, field.Name) : null,
                GenerateXAttribute(Const_XmlNameAtt_Value, val),
                ToXml_TypeSimple(helper, field != null ? field.FieldType : val.GetType())
                );
        }

        /// <summary>
        /// 反序列化字段(Int)
        /// </summary>
        /// <param name="helper">助手</param>
        /// <param name="xml">Xml数据</param>
        /// <param name="val">字段所属对象</param>
        /// <returns>Xml数据</returns>
        private static void FromXml_Int(XmlReadHelper helper, XElement xml, ref object val)
        {
            XAttribute fieldAtt = GetXAttribute(xml, Const_XmlNameAtt_Field, true, true);
            if (fieldAtt != null)
            {
                string fieldName = fieldAtt.Value;
                FieldInfo field = val.GetType().GetField(fieldName);
                XAttribute valAtt = GetXAttribute(xml, Const_XmlNameAtt_Value);
                field.SetValue(val, (int)valAtt);
            }
            else
            {
                XAttribute valAtt = GetXAttribute(xml, Const_XmlNameAtt_Value);
                val = (int)valAtt;
            }
        }

        /// <summary>
        /// 获取基本类型Int
        /// </summary>
        /// <param name="type">字段所属对象</param>
        /// <returns>基础类型</returns>
        private static Type Type_GetBaseInt(Type type)
        {
            return type == typeof(int) ? type : null;
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
            return GenerateXElement(field != null ? Const_XmlNameEle_NonSerializedField : Const_XmlNameEle_NonSerializeObject,
                field != null ? GenerateXAttribute(Const_XmlNameAtt_Field, field.Name) : null,
                GenerateXAttribute(Const_XmlNameAtt_Value, val),
                ToXml_TypeSimple(helper, field != null ? field.FieldType : val.GetType())
                );
        }

        /// <summary>
        /// 反序列化字段(Long)
        /// </summary>
        /// <param name="helper">助手</param>
        /// <param name="xml">Xml数据</param>
        /// <param name="val">字段所属对象</param>
        /// <returns>Xml数据</returns>
        private static void FromXml_Long(XmlReadHelper helper, XElement xml, ref object val)
        {
            XAttribute fieldAtt = GetXAttribute(xml, Const_XmlNameAtt_Field, true, true);
            if (fieldAtt != null)
            {
                string fieldName = fieldAtt.Value;
                FieldInfo field = val.GetType().GetField(fieldName);
                XAttribute valAtt = GetXAttribute(xml, Const_XmlNameAtt_Value);
                field.SetValue(val, (long)valAtt);
            }
            else
            {
                XAttribute valAtt = GetXAttribute(xml, Const_XmlNameAtt_Value);
                val = (long)valAtt;
            }
        }

        /// <summary>
        /// 获取基本类型Long
        /// </summary>
        /// <param name="type">字段所属对象</param>
        /// <returns>基础类型</returns>
        private static Type Type_GetBaseLong(Type type)
        {
            return type == typeof(long) ? type : null;
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
            return GenerateXElement(field != null ? Const_XmlNameEle_NonSerializedField : Const_XmlNameEle_NonSerializeObject,
                field != null ? GenerateXAttribute(Const_XmlNameAtt_Field, field.Name) : null,
                GenerateXAttribute(Const_XmlNameAtt_Value, val),
                ToXml_TypeSimple(helper, field != null ? field.FieldType : val.GetType())
                );
        }

        /// <summary>
        /// 反序列化字段(SByte)
        /// </summary>
        /// <param name="helper">助手</param>
        /// <param name="xml">Xml数据</param>
        /// <param name="val">字段所属对象</param>
        /// <returns>Xml数据</returns>
        private static void FromXml_SByte(XmlReadHelper helper, XElement xml, ref object val)
        {
            XAttribute fieldAtt = GetXAttribute(xml, Const_XmlNameAtt_Field, true, true);
            if (fieldAtt != null)
            {
                string fieldName = fieldAtt.Value;
                FieldInfo field = val.GetType().GetField(fieldName);
                XAttribute valAtt = GetXAttribute(xml, Const_XmlNameAtt_Value);
                field.SetValue(val, (sbyte)valAtt);
            }
            else
            {
                XAttribute valAtt = GetXAttribute(xml, Const_XmlNameAtt_Value);
                val = (sbyte)valAtt;
            }
        }

        /// <summary>
        /// 获取基本类型SByte
        /// </summary>
        /// <param name="type">字段所属对象</param>
        /// <returns>基础类型</returns>
        private static Type Type_GetBaseSByte(Type type)
        {
            return type == typeof(sbyte) ? type : null;
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
            return GenerateXElement(field != null ? Const_XmlNameEle_NonSerializedField : Const_XmlNameEle_NonSerializeObject,
                field != null ? GenerateXAttribute(Const_XmlNameAtt_Field, field.Name) : null,
                GenerateXAttribute(Const_XmlNameAtt_Value, val),
                ToXml_TypeSimple(helper, field != null ? field.FieldType : val.GetType())
                );
        }

        /// <summary>
        /// 反序列化字段(Short)
        /// </summary>
        /// <param name="helper">助手</param>
        /// <param name="xml">Xml数据</param>
        /// <param name="val">字段所属对象</param>
        /// <returns>Xml数据</returns>
        private static void FromXml_Short(XmlReadHelper helper, XElement xml, ref object val)
        {
            XAttribute fieldAtt = GetXAttribute(xml, Const_XmlNameAtt_Field, true, true);
            if (fieldAtt != null)
            {
                string fieldName = fieldAtt.Value;
                FieldInfo field = val.GetType().GetField(fieldName);
                XAttribute valAtt = GetXAttribute(xml, Const_XmlNameAtt_Value);
                field.SetValue(val, (short)valAtt);
            }
            else
            {
                XAttribute valAtt = GetXAttribute(xml, Const_XmlNameAtt_Value);
                val = (short)valAtt;
            }
        }

        /// <summary>
        /// 获取基本类型Short
        /// </summary>
        /// <param name="type">字段所属对象</param>
        /// <returns>基础类型</returns>
        private static Type Type_GetBaseShort(Type type)
        {
            return type == typeof(short) ? type : null;
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
            return GenerateXElement(field != null ? Const_XmlNameEle_NonSerializedField : Const_XmlNameEle_NonSerializeObject,
                field != null ? GenerateXAttribute(Const_XmlNameAtt_Field, field.Name) : null,
                val != null ? GenerateXAttribute(Const_XmlNameAtt_Value, val) : null,
                ToXml_TypeSimple(helper, val != null ? val.GetType() : field.FieldType)
                );
        }

        /// <summary>
        /// 反序列化字段(String)
        /// </summary>
        /// <param name="helper">助手</param>
        /// <param name="xml">Xml数据</param>
        /// <param name="val">字段所属对象</param>
        /// <returns>Xml数据</returns>
        private static void FromXml_String(XmlReadHelper helper, XElement xml, ref object val)
        {
            XAttribute fieldAtt = GetXAttribute(xml, Const_XmlNameAtt_Field, true, true);
            if (fieldAtt != null)
            {
                string fieldName = fieldAtt.Value;
                FieldInfo field = val.GetType().GetField(fieldName);
                XAttribute valAtt = GetXAttribute(xml, Const_XmlNameAtt_Value, true, true);
                field.SetValue(val, valAtt== null ?null :valAtt.Value);
            }
            else
            {
                XAttribute valAtt = GetXAttribute(xml, Const_XmlNameAtt_Value);
                val = valAtt.Value;
            }
        }

        /// <summary>
        /// 获取基本类型String
        /// </summary>
        /// <param name="type">字段所属对象</param>
        /// <returns>基础类型</returns>
        private static Type Type_GetBaseString(Type type)
        {
            return type == typeof(string) ? type : null;
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
            return GenerateXElement(field != null ? Const_XmlNameEle_NonSerializedField : Const_XmlNameEle_NonSerializeObject,
                field != null ? GenerateXAttribute(Const_XmlNameAtt_Field, field.Name) : null,
                GenerateXAttribute(Const_XmlNameAtt_Value, val),
                ToXml_TypeSimple(helper, field != null ? field.FieldType : val.GetType())
                );
        }

        /// <summary>
        /// 反序列化字段(UInt)
        /// </summary>
        /// <param name="helper">助手</param>
        /// <param name="xml">Xml数据</param>
        /// <param name="val">字段所属对象</param>
        /// <returns>Xml数据</returns>
        private static void FromXml_UInt(XmlReadHelper helper, XElement xml, ref object val)
        {
            XAttribute fieldAtt = GetXAttribute(xml, Const_XmlNameAtt_Field, true, true);
            if (fieldAtt != null)
            {
                string fieldName = fieldAtt.Value;
                FieldInfo field = val.GetType().GetField(fieldName);
                XAttribute valAtt = GetXAttribute(xml, Const_XmlNameAtt_Value);
                field.SetValue(val, (uint)valAtt);
            }
            else
            {
                XAttribute valAtt = GetXAttribute(xml, Const_XmlNameAtt_Value);
                val = (uint)valAtt;
            }
        }

        /// <summary>
        /// 获取基本类型UInt
        /// </summary>
        /// <param name="type">字段所属对象</param>
        /// <returns>基础类型</returns>
        private static Type Type_GetBaseUInt(Type type)
        {
            return type == typeof(uint) ? type : null;
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
            return GenerateXElement(field != null ? Const_XmlNameEle_NonSerializedField : Const_XmlNameEle_NonSerializeObject,
                field != null ? GenerateXAttribute(Const_XmlNameAtt_Field, field.Name) : null,
                GenerateXAttribute(Const_XmlNameAtt_Value, val),
                ToXml_TypeSimple(helper, field != null ? field.FieldType : val.GetType())
                );
        }

        /// <summary>
        /// 反序列化字段(ULong)
        /// </summary>
        /// <param name="helper">助手</param>
        /// <param name="xml">Xml数据</param>
        /// <param name="val">字段所属对象</param>
        /// <returns>Xml数据</returns>
        private static void FromXml_ULong(XmlReadHelper helper, XElement xml, ref object val)
        {
            XAttribute fieldAtt = GetXAttribute(xml, Const_XmlNameAtt_Field, true, true);
            if (fieldAtt != null)
            {
                string fieldName = fieldAtt.Value;
                FieldInfo field = val.GetType().GetField(fieldName);
                XAttribute valAtt = GetXAttribute(xml, Const_XmlNameAtt_Value);
                field.SetValue(val, (ulong)valAtt);
            }
            else
            {
                XAttribute valAtt = GetXAttribute(xml, Const_XmlNameAtt_Value);
                val = (ulong)valAtt;
            }
        }

        /// <summary>
        /// 获取基本类型ULong
        /// </summary>
        /// <param name="type">字段所属对象</param>
        /// <returns>基础类型</returns>
        private static Type Type_GetBaseULong(Type type)
        {
            return type == typeof(ulong) ? type : null;
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
            return GenerateXElement(field != null ? Const_XmlNameEle_NonSerializedField : Const_XmlNameEle_NonSerializeObject,
                field != null ? GenerateXAttribute(Const_XmlNameAtt_Field, field.Name) : null,
                GenerateXAttribute(Const_XmlNameAtt_Value, val),
                ToXml_TypeSimple(helper, field != null ? field.FieldType : val.GetType())
                );
        }

        /// <summary>
        /// 反序列化字段(UShort)
        /// </summary>
        /// <param name="helper">助手</param>
        /// <param name="xml">Xml数据</param>
        /// <param name="val">字段所属对象</param>
        /// <returns>Xml数据</returns>
        private static void FromXml_UShort(XmlReadHelper helper, XElement xml, ref object val)
        {
            XAttribute fieldAtt = GetXAttribute(xml, Const_XmlNameAtt_Field, true, true);
            if (fieldAtt != null)
            {
                string fieldName = fieldAtt.Value;
                FieldInfo field = val.GetType().GetField(fieldName);
                XAttribute valAtt = GetXAttribute(xml, Const_XmlNameAtt_Value);
                field.SetValue(val, ushort.Parse(valAtt.Value));
            }
            else
            {
                XAttribute valAtt = GetXAttribute(xml, Const_XmlNameAtt_Value);
                val = ushort.Parse(valAtt.Value);
            }
        }

        /// <summary>
        /// 获取基本类型UShort
        /// </summary>
        /// <param name="type">字段所属对象</param>
        /// <returns>基础类型</returns>
        private static Type Type_GetBaseUShort(Type type)
        {
            return type == typeof(ushort) ? type : null;
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
            if (val == null)
            {
                return GenerateXElement(field != null ? Const_XmlNameEle_NonSerializedField : Const_XmlNameEle_NonSerializeObject,
                   field != null ? GenerateXAttribute(Const_XmlNameAtt_Field, field.Name) : null,
                   ToXml_TypeSimple(helper, field != null ? field.FieldType : val.GetType())
                   );
            }
            if (!(val is UnityEngine.Object))
            {
                throw new Exception("错误的资源类型");
            }
            UnityEngine.Object obj = val as UnityEngine.Object;
            string path = AssetDatabase.GetAssetPath(obj);
            return GenerateXElement(field != null ? Const_XmlNameEle_NonSerializedField : Const_XmlNameEle_NonSerializeObject,
                field != null ? GenerateXAttribute(Const_XmlNameAtt_Field, field.Name) : null,
                GenerateXAttribute(Const_XmlNameAtt_Value, path),
                ToXml_TypeSimple(helper, field != null ? field.FieldType : val.GetType())
                );
        }

        /// <summary>
        /// 反序列化字段(Asset)
        /// </summary>
        /// <param name="helper">助手</param>
        /// <param name="xml">Xml数据</param>
        /// <param name="val">字段所属对象</param>
        /// <returns>Xml数据</returns>
        private static void FromXml_Asset(XmlReadHelper helper, XElement xml, ref object val)
        {
            XAttribute fieldAtt = GetXAttribute(xml, Const_XmlNameAtt_Field, true, true);
            if (fieldAtt != null)
            {
                string fieldName = fieldAtt.Value;
                FieldInfo field = val.GetType().GetField(fieldName);
                XAttribute valAtt = GetXAttribute(xml, Const_XmlNameAtt_Value, true, true);
                if (valAtt == null)
                {
                    field.SetValue(val, null);
                }
                else
                {
                    UnityEngine.Object asset = AssetDatabase.LoadMainAssetAtPath(valAtt.Value);
                    field.SetValue(val, asset);
                }
            }
            else
            {
                XAttribute valAtt = GetXAttribute(xml, Const_XmlNameAtt_Value, true, true);
                val = valAtt == null ? null : AssetDatabase.LoadMainAssetAtPath(valAtt.Value);
            }
        }

        /// <summary>
        /// 获取基本类型Asset
        /// </summary>
        /// <param name="type">字段所属对象</param>
        /// <returns>基础类型</returns>
        private static Type Type_GetBaseAsset(Type type)
        {
            foreach (Type assetType in Const_ListAssetType)
            {
                if (assetType == type)
                {
                    return typeof(UnityEngine.Object);
                }
            }
            return null;
        }

        #endregion Asset

        #region Array

        /// <summary>
        /// 序列化字段(Array)
        /// </summary>
        /// <param name="helper">助手</param>
        /// <param name="field">字段</param>
        /// <param name="val">值</param>
        /// <returns>Xml数据</returns>
        private static XElement ToXml_Array(XmlWriteHelper helper, FieldInfo field, object val)
        {
            if (val == null)
            {
                return GenerateXElement(field != null ? Const_XmlNameEle_NonSerializedField : Const_XmlNameEle_NonSerializeObject,
                    field != null ? GenerateXAttribute(Const_XmlNameAtt_Field, field.Name) : null,
                    ToXml_TypeSimple(helper, field != null ? field.FieldType : val.GetType())
                    );
            }
            if (!(val is Array))
            {
                throw new Exception("错误的Array类型");
            }
            Array array = val as Array;
            XElement element = ToXml_Object(helper, val);
            XAttribute countAtt = GenerateXAttribute(Const_XmlNameAtt_Count, array.Length);
            element.Add(countAtt);
            int index = 1;
            foreach (object child in array)
            {
                XName name = XName.Get(Const_XmlNameOth_Array, index.ToString());
                helper.PushName(name);
                if (PrintCommentPath)
                {
                    XComment comment = new XComment("路径：" + helper.PeekName());
                    element.Add(comment);
                }
                element.Add(helper.GetValueXml(child));
                helper.PopName();
                index++;
            }
            return GenerateXElement(field != null ? Const_XmlNameEle_NonSerializedField : Const_XmlNameEle_NonSerializeObject,
                field != null ? GenerateXAttribute(Const_XmlNameAtt_Field, field.Name) : null,
                element,
                ToXml_TypeSimple(helper, field != null ? field.FieldType : val.GetType())
                );
        }

        /// <summary>
        /// 创建Array对象
        /// </summary>
        /// <param name="helper">助手</param>
        /// <param name="xml">Xml数据</param>
        /// <returns>List对象</returns>
        private static object FromXml_ArrayCreate(XmlReadHelper helper, XElement xml)
        {
            XAttribute countAtt = GetXAttribute(xml, Const_XmlNameAtt_Count);
            Array array = FromXml_Object(helper, xml, (int)countAtt) as Array;
            if (array == null)
            {
                throw new Exception("数据不是Array类型");
            }
            IEnumerable<XElement> childrenXml = GetXElements(xml, Const_XmlNameEle_Object);
            int index = 1;
            foreach (XElement childXml in childrenXml)
            {
                helper.PushName(XName.Get(Const_XmlNameOth_Array, index.ToString()));
                object child = null;
                helper.GetXmlValue(childXml, ref child);
                array.SetValue(child, index - 1);
                helper.PopName();
                index++;
            }
            return array;
        }

        /// <summary>
        /// 反序列化字段(Array)
        /// </summary>
        /// <param name="helper">助手</param>
        /// <param name="xml">Xml数据</param>
        /// <param name="val">字段所属对象</param>
        /// <returns>Xml数据</returns>
        private static void FromXml_Array(XmlReadHelper helper, XElement xml, ref object val)
        {
            XAttribute fieldAtt = GetXAttribute(xml, Const_XmlNameAtt_Field, true, true);
            XElement arrayXml = GetXElement(xml, Const_XmlNameEle_Object, true, true);
            if (fieldAtt != null)
            {
                string fieldName = fieldAtt.Value;
                FieldInfo field = val.GetType().GetField(fieldName);
                field.SetValue(val, arrayXml == null ? null : FromXml_ArrayCreate(helper, arrayXml));
            }
            else
            {
                val = arrayXml == null ? null : FromXml_ArrayCreate(helper, arrayXml);
            }
        }

        /// <summary>
        /// 获取基本类型Array
        /// </summary>
        /// <param name="type">字段所属对象</param>
        /// <returns>基础类型</returns>
        private static Type Type_GetBaseArray(Type type)
        {
            return typeof(Array).IsAssignableFrom(type) ? typeof(Array) : null;
        }

        #endregion Array

        #region List

        /// <summary>
        /// 序列化字段(List)
        /// </summary>
        /// <param name="helper">助手</param>
        /// <param name="field">字段</param>
        /// <param name="val">值</param>
        /// <returns>Xml数据</returns>
        private static XElement ToXml_List(XmlWriteHelper helper, FieldInfo field, object val)
        {
            if (val == null)
            {
                return GenerateXElement(field != null ? Const_XmlNameEle_NonSerializedField : Const_XmlNameEle_NonSerializeObject,
                    field != null ? GenerateXAttribute(Const_XmlNameAtt_Field, field.Name) : null,
                    ToXml_TypeSimple(helper, field != null ? field.FieldType : val.GetType())
                    );
            }
            if (!(val is IList))
            {
                throw new Exception("错误的List类型");
            }
            IList list = val as IList;
            XElement element = ToXml_Object(helper, val);
            int index = 1;
            foreach (object child in list)
            {
                XName name = XName.Get(Const_XmlNameOth_List, index.ToString());
                helper.PushName(name);
                if (PrintCommentPath)
                {
                    XComment comment = new XComment("路径：" + helper.PeekName());
                    element.Add(comment);
                }
                element.Add(helper.GetValueXml(child));
                helper.PopName();
                index++;
            }
            return GenerateXElement(field != null ? Const_XmlNameEle_NonSerializedField : Const_XmlNameEle_NonSerializeObject,
                field != null ? GenerateXAttribute(Const_XmlNameAtt_Field, field.Name) : null,
                element,
                ToXml_TypeSimple(helper, field != null ? field.FieldType : val.GetType())
                );
        }

        /// <summary>
        /// 创建List对象
        /// </summary>
        /// <param name="helper">助手</param>
        /// <param name="xml">Xml数据</param>
        /// <returns>List对象</returns>
        private static object FromXml_ListCreate(XmlReadHelper helper, XElement xml)
        {
            IList list = FromXml_Object(helper, xml) as IList;
            if (list == null)
            {
                throw new Exception("数据不是IList类型");
            }
            IEnumerable<XElement> childrenXml = GetXElements(xml, Const_XmlNameEle_Object);
            int index = 1;
            foreach (XElement childXml in childrenXml)
            {
                helper.PushName(XName.Get(Const_XmlNameOth_List, index.ToString()));
                object child = null;
                helper.GetXmlValue(childXml, ref child);
                list.Add(child);
                helper.PopName();
                index++;
            }
            return list;
        }

        /// <summary>
        /// 反序列化字段(List)
        /// </summary>
        /// <param name="helper">助手</param>
        /// <param name="xml">Xml数据</param>
        /// <param name="val">字段所属对象</param>
        /// <returns>Xml数据</returns>
        private static void FromXml_List(XmlReadHelper helper, XElement xml, ref object val)
        {
            XAttribute fieldAtt = GetXAttribute(xml, Const_XmlNameAtt_Field, true, true);
            XElement listXml = GetXElement(xml, Const_XmlNameEle_Object, true, true);
            if (fieldAtt != null)
            {
                string fieldName = fieldAtt.Value;
                FieldInfo field = val.GetType().GetField(fieldName);
                field.SetValue(val, listXml == null ? null : FromXml_ListCreate(helper, listXml));
            }
            else
            {
                val = listXml == null ? null : FromXml_ListCreate(helper, listXml);
            }
        }

        /// <summary>
        /// 获取基本类型List
        /// </summary>
        /// <param name="type">字段所属对象</param>
        /// <returns>基础类型</returns>
        private static Type Type_GetBaseList(Type type)
        {
            if (!type.IsGenericType) return null;
            Type baseType = type.GetGenericTypeDefinition();
            return typeof(IList).IsAssignableFrom(baseType) ? typeof(IList) : null;
        }

        #endregion List

        #region Dictionary

        /// <summary>
        /// 序列化字段(Dictionary)
        /// </summary>
        /// <param name="helper">助手</param>
        /// <param name="field">字段</param>
        /// <param name="val">值</param>
        /// <returns>Xml数据</returns>
        private static XElement ToXml_Dictionary(XmlWriteHelper helper, FieldInfo field, object val)
        {
            if (val == null)
            {
                return GenerateXElement(field != null ? Const_XmlNameEle_NonSerializedField : Const_XmlNameEle_NonSerializeObject,
                    field != null ? GenerateXAttribute(Const_XmlNameAtt_Field, field.Name) : null,
                    ToXml_TypeSimple(helper, field != null ? field.FieldType : val.GetType())
                    );
            }
            if (!(val is IDictionary))
            {
                throw new Exception("错误的List类型");
            }
            IDictionary dict = val as IDictionary;
            XElement element = ToXml_Object(helper, val);
            int index = 1;
            foreach (DictionaryEntry select in dict)
            {
                element.Add(ToXml_KeyValuePair(helper, select.Key, select.Value, index));
                index++;
            }
            return GenerateXElement(field != null ? Const_XmlNameEle_NonSerializedField : Const_XmlNameEle_NonSerializeObject,
                field != null ? GenerateXAttribute(Const_XmlNameAtt_Field, field.Name) : null,
                element,
                ToXml_TypeSimple(helper, field != null ? field.FieldType : val.GetType())
                );
        }

        /// <summary>
        /// 创建Dictionary对象
        /// </summary>
        /// <param name="helper">助手</param>
        /// <param name="xml">Xml数据</param>
        /// <returns>Dictionary对象</returns>
        private static object FromXml_DictionaryCreate(XmlReadHelper helper, XElement xml)
        {
            IDictionary dict = FromXml_Object(helper, xml) as IDictionary;
            if (dict == null)
            {
                throw new Exception("数据不是IDictionary类型");
            }
            IEnumerable<XElement> childrenXml = GetXElements(xml, Const_XmlNameEle_KeyValuePair);
            int index = 1;
            foreach (XElement childXml in childrenXml)
            {
                FromXml_KeyValue(helper, childXml, dict, index);
                index++;
            }
            return dict;
        }

        /// <summary>
        /// 反序列化字段(Dictionary)
        /// </summary>
        /// <param name="helper">助手</param>
        /// <param name="xml">Xml数据</param>
        /// <param name="val">字段所属对象</param>
        /// <returns>Xml数据</returns>
        private static void FromXml_Dictionary(XmlReadHelper helper, XElement xml, ref object val)
        {
            XAttribute fieldAtt = GetXAttribute(xml, Const_XmlNameAtt_Field, true, true);
            XElement dictXml = GetXElement(xml, Const_XmlNameEle_Object, true, true);
            if (fieldAtt != null)
            {
                string fieldName = fieldAtt.Value;
                FieldInfo field = val.GetType().GetField(fieldName);
                field.SetValue(val, dictXml == null ? null : FromXml_DictionaryCreate(helper, dictXml));
            }
            else
            {
                val = dictXml == null ? null : FromXml_ListCreate(helper, dictXml);
            }
        }

        /// <summary>
        /// 获取基本类型Dictionary
        /// </summary>
        /// <param name="type">字段所属对象</param>
        /// <returns>基础类型</returns>
        private static Type Type_GetBaseDictionary(Type type)
        {
            if (!type.IsGenericType) return null;
            Type baseType = type.GetGenericTypeDefinition();
            return typeof(IDictionary).IsAssignableFrom(baseType) ? typeof(IDictionary) : null;
        }

        #endregion Dictionary

        #endregion 类型处理方法

        #region 序列化支持

        #region 反射相关

        /// <summary>
        /// 获取带有程序集名的类型全称
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>全称</returns>
        private static string TypeFullNameWithAssembly(Type type)
        {
            return type.FullName + "," + type.Assembly.FullName;
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
            object[] temp = content.Where(r => r != null).ToArray();
            return new XElement(name, temp);
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
        /// <param name="att">类型Xml</param>
        /// <returns>类型</returns>
        private static Type FromXml_TypeSimple(XmlReadHelper helper, XAttribute att)
        {
            return helper.GetType(att.Value);
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
        /// <param name="val">对象</param>
        /// <returns>XEl对象</returns>
        private static XElement ToXml_Object(XmlWriteHelper helper, object val)
        {
            if (val == null) return GenerateXElement(Const_XmlNameEle_Null);
            Type type = val.GetType();
            return GenerateXElement(Const_XmlNameEle_Object,
                ToXml_TypeSimple(helper, type)
                );
        }

        /// <summary>
        /// 实例化类
        /// </summary>
        /// <param name="helper">助手</param>
        /// <param name="xml">配置数据</param>
        /// <param name="args">构造函数参数</param>
        /// <returns>实例对象</returns>
        private static object FromXml_Object(XmlReadHelper helper, XElement xml, params object[] args)
        {
            if (xml.Name == Const_XmlNameEle_Null) return null;
            CheckXelementName(xml, Const_XmlNameEle_Object);
            XAttribute typeXML = GetXAttribute(xml, Const_XmlNameAtt_Type);
            Type type = FromXml_TypeSimple(helper, typeXML);
            return Activator.CreateInstance(type, args ?? Const_NoElementObjectArray);
        }

        #endregion Object

        #region Document

        /// <summary>
        /// 获取文档XElement
        /// </summary>
        /// <returns>XEl对象</returns>
        public static XElement ToXml_Document()
        {
            return GenerateXElement(Const_XmlNameDoc_Document,
                new XComment("类型记录"),
                GenerateXElement(Const_XmlNameDoc_TypeList),
                new XComment("数据记录"),
                GenerateXElement(Const_XmlNameDoc_ObjectList)
                );;
        }

        /// <summary>
        /// 获取类型列表Xml
        /// </summary>
        /// <param name="xml">文档Xml</param>
        /// <returns>列表Xml</returns>
        public static XElement FromXml_TypeList(XElement xml)
        {
            CheckXelementName(xml, Const_XmlNameDoc_Document);
            return GetXElement(xml, Const_XmlNameDoc_TypeList);
        }

        /// <summary>
        /// 获取类型对象Xml
        /// </summary>
        /// <param name="xml">文档Xml</param>
        /// <returns>对象Xml</returns>
        public static XElement FromXml_ObjectList(XElement xml)
        {
            CheckXelementName(xml, Const_XmlNameDoc_Document);
            return GetXElement(xml, Const_XmlNameDoc_ObjectList);
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
        private static XmlTypeInfo FromXml_TypeInfo(XElement xml)
        {
            XAttribute nameAtt = GetXAttribute(xml, Const_XmlNameAtt_Name);
            XElement typeXml = GetXElement(xml, Const_XmlNameEle_Type);
            Type type = FromXml_Type(typeXml);
            return new XmlTypeInfo(nameAtt.Value, type);
        }

        #endregion Document

        #region KeyValuePair

        /// <summary>
        /// 获取KeyValue对XElement
        /// </summary>
        /// <param name="helper">助手</param>
        /// <param name="key">Key值</param>
        /// <param name="value">Value值</param>
        /// <param name="index">序号</param>
        /// <returns>XElement对象</returns>
        private static XElement ToXml_KeyValuePair(XmlWriteHelper helper, object key, object value, int index)
        {
            helper.PushName(XName.Get(Const_XmlNameOth_Dictionary + "_" + Const_XmlNameAtt_Key, index.ToString()));
            XComment commentKey = null;
            if (PrintCommentPath)
            {
                commentKey = new XComment("路径：" + helper.PeekName());
            }
            XElement xmlKey = helper.GetValueXml(key);
            helper.PopName();
            helper.PushName(XName.Get(Const_XmlNameOth_Dictionary + "_" + Const_XmlNameAtt_Value, index.ToString()));
            XComment commentValue = null;
            if (PrintCommentPath)
            {
                commentValue = new XComment("路径：" + helper.PeekName());
            }
            XElement xmlValue = helper.GetValueXml(value);
            helper.PopName();
            return GenerateXElement(Const_XmlNameEle_KeyValuePair,
                commentKey,
                GenerateXElement(Const_XmlNameAtt_Key, xmlKey),
                commentValue,
                GenerateXElement(Const_XmlNameAtt_Value, xmlValue)
                );
        }

        /// <summary>
        /// 加入KeyValuePair书架
        /// </summary>
        /// <typeparam name="TKey">Key值</typeparam>
        /// <typeparam name="TValue">Value值</typeparam>
        /// <param name="helper">助手</param>
        /// <param name="xml">配置数据</param>
        /// <param name="dict">数据字典</param>
        /// <param name="index">序号</param>
        private static void FromXml_KeyValue(XmlReadHelper helper, XElement xml, IDictionary dict, int index)
        {
            CheckXelementName(xml, Const_XmlNameEle_KeyValuePair);
            XElement key = GetXElement(xml, Const_XmlNameAtt_Key);
            if (key.Elements().Count() != 1)
            {
                throw new Exception("Key中数据数量异常");
            }
            key = GetOnlyValue(key);
            helper.PushName(XName.Get(Const_XmlNameOth_Dictionary + "_" + Const_XmlNameAtt_Key, index.ToString()));
            object keyObj = null;
            helper.GetXmlValue(key, ref keyObj);
            helper.PopName();
            XElement value = GetXElement(xml, Const_XmlNameAtt_Value);
            if (value.Elements().Count() != 1)
            {
                throw new Exception("Value中数据数量异常");
            }
            value = GetOnlyValue(value);
            helper.PushName(XName.Get(Const_XmlNameOth_Dictionary + "_" + Const_XmlNameAtt_Value, index.ToString()));
            object valueObj = null;
            helper.GetXmlValue(value, ref valueObj);
            helper.PopName();
            dict.Add(keyObj, valueObj);
        }

        #endregion KeyValuePair

        #endregion 序列化支持

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
    public sealed class FrogSerializableAttribute : Attribute
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
        public FrogSerializableAttribute()
        {
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
