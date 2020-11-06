using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
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

        #endregion 元素名称

        #region 属性名称

        public const string Const_XmlNameAtt_Field = "Field";
        public const string Const_XmlNameAtt_Name = "Name";
        public const string Const_XmlNameAtt_Reference = "Reference";
        public const string Const_XmlNameAtt_Type = "Type";
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
        public static readonly Type[] Const_NoElementTypeArray = Array.Empty<Type>();

        /// <summary>
        /// 无元素object数组
        /// </summary>
        public static readonly object[] Const_NoElementObjectArray = Array.Empty<object>();

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

            public Dictionary<Type, IDictionary<string, (FieldInfo Field, string Comment)>> DictSerializeFieldForType { get; } = new Dictionary<Type, IDictionary<string, (FieldInfo Field, string Comment)>>();

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
            /// <param name="obj">对象，为空时自动新建，否则修改字段值</param>
            /// <remarks>引用的路径值为空时，对象为null。</remarks>
            public void GetXmlValue(XElement xml, ref object obj)
            {
                XAttribute attRef = FromXml_Reference(xml);
                if (attRef != null)
                {
                    // 值是一个引用
                    if (string.IsNullOrEmpty(attRef.Value))
                    {
                        obj = null;
                    }
                    else
                    {
                        if (!mDictValuePath.ContainsKey(attRef.Value))
                        {
                            throw new Exception("引用对象不存在或尚未解析。");
                        }
                        obj = mDictValuePath[attRef.Value];
                    }
                }
                else
                {
                    // 值为IInterface_GdsValue
                    if (obj == null) obj = FromXml_Object(this, xml);
                    mDictValuePath[StackCurrentPath.Peek()] = obj;
                    DeserializeWithHelper(this, xml, ref obj);
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
        private delegate void Delegate_DeserializeField(XmlReadHelper helper, XElement xml, ref object obj);

        #endregion 委托

        #endregion 内部声明

        #region 属性字段

        #region 静态属性

        #endregion 静态属性

        #region 属性

        /// <summary>
        /// 打印路径注释
        /// </summary>
        public static bool PrintCommentPath { set; get; } = false;

        /// <summary>
        /// 打印自定义注释
        /// </summary>
        public static bool PrintCustomPath { set; get; } = true;

        #endregion 属性

        #region 字段

        /// <summary>
        /// 基本类型序列化方法
        /// </summary>
        private static readonly Dictionary<Type, (Type ValueType, Delegate_SerializeField SerializeFunc, Delegate_DeserializeField DeserializeFunc)> mListSerializeFunc =
            new Dictionary<Type, (Type ValueType, Delegate_SerializeField SerializeFunc, Delegate_DeserializeField DeserializeFunc)>()
            {
                { typeof(bool), (typeof(bool), ToXml_Bool, FromXml_Bool) },
                { typeof(byte), (typeof(byte), ToXml_Byte, FromXml_Byte) },
                { typeof(char), (typeof(char), ToXml_Char, FromXml_Char) },
                { typeof(decimal), (typeof(decimal), ToXml_Decimal, FromXml_Decimal) },
                { typeof(double), (typeof(double), ToXml_Double, FromXml_Double) },
                { typeof(Enum), (typeof(Enum), ToXml_Enum, FromXml_Enum) },
                { typeof(float), (typeof(float), ToXml_Float, FromXml_Float) },
                { typeof(int), (typeof(int), ToXml_Int, FromXml_Int) },
                { typeof(long), (typeof(long), ToXml_Long, FromXml_Long) },
                { typeof(sbyte), (typeof(sbyte), ToXml_SByte, FromXml_SByte) },
                { typeof(short), (typeof(short), ToXml_Short, FromXml_Short) },
                { typeof(string), (typeof(string), ToXml_String, FromXml_String) },
                { typeof(uint), (typeof(uint), ToXml_UInt, FromXml_UInt) },
                { typeof(ulong), (typeof(ulong), ToXml_ULong, FromXml_ULong) },
                { typeof(ushort), (typeof(ushort), ToXml_UShort, FromXml_UShort) },
                { typeof(UnityEngine.Object), (typeof(UnityEngine.Object), ToXml_Asset, FromXml_Asset) },
                { typeof(IList), (typeof(IList), ToXml_List, FromXml_List) },
                { typeof(Array), (typeof(Array), ToXml_Array, FromXml_Array) },
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
        /// <param name="obj">对象</param>
        /// <returns>Xml数据</returns>
        public static XElement Serialize(object obj)
        {
            XmlWriteHelper helper = new XmlWriteHelper();
            XElement doc = ToXml_Document();
            XElement xmlTypeList = FromXml_TypeList(doc);
            XElement xmlObjList = FromXml_ObjectList(doc);
            xmlObjList.Add(SerializeWithHelper(helper, obj));
            helper.GenerateTypeList(xmlTypeList);
            return doc;
        }

        /// <summary>
        /// 反序列化对象
        /// </summary>
        /// <param name="xml">Xml数据</param>
        /// <param name="obj">对象，为空时自动新建，否则修改字段值</param>
        /// <returns>对象</returns>
        public static void Deserialize(XElement xml, ref object obj)
        {
            CheckXelementName(xml, Const_XmlNameDoc_Document);
            XmlReadHelper helper = new XmlReadHelper();
            XElement xmlTypeList = FromXml_TypeList(xml);
            XElement xmlObjList = FromXml_ObjectList(xml);
            helper.InitTypeList(xmlTypeList);
            XElement ObjXml = GetXElement(xmlObjList, Const_XmlNameEle_Object);
            if (obj != null) DeserializeWithHelper(helper, ObjXml, ref obj);
            DeserializeWithHelper(helper, ObjXml, ref obj);
        }

        /// <summary>
        /// 序列化对象
        /// </summary>
        /// <param name="helper">助手</param>
        /// <param name="obj">对象</param>
        /// <returns>Xml数据</returns>
        private static XElement SerializeWithHelper_Simple(XmlWriteHelper helper, object obj)
        {
            XElement element = ToXml_Object(helper, obj);
            if (obj != null)
            {
                Type type = obj.GetType();
                Type baseType = GetBaseSerializeType(type);
                element.Add(mListSerializeFunc[type].SerializeFunc(helper, null, obj));
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
        /// < param name="obj">对象</param>
        /// <returns>Xml数据</returns>
        private static XElement SerializeWithHelper(XmlWriteHelper helper, object obj)
        {
            Type type = obj.GetType();
            if (IsSerializableType(type))
            {
                helper.AddObjeInPath(obj);
                XElement element = ToXml_Object(helper, obj);
                if (obj != null)
                {
                    if (!helper.DictSerializeFieldForType.ContainsKey(type))
                    {
                        helper.DictSerializeFieldForType.Add(type, GetSerializableFields(type));
                    }
                    IDictionary<string, (FieldInfo Field, string Comment)> dict = helper.DictSerializeFieldForType[type];
                    foreach (var item in dict)
                    {
                        helper.PushName(item.Value.Field.Name);
                        if (PrintCommentPath)
                        {
                            XComment comment = new XComment($"路径：{helper.PeekName()}");
                            element.Add(comment);
                        }
                        if (PrintCustomPath)
                        {
                            XComment comment = new XComment($"注释：{item.Value.Comment}");
                            element.Add(comment);
                        }
                        object val = item.Value.Field.GetValue(obj);
                        if (IsSerializableType(item.Value.Field.FieldType))
                        {
                            XElement field = GenerateXElement(Const_XmlNameEle_SerializableField,
                                GenerateXAttribute(Const_XmlNameAtt_Field, item.Value.Field.Name),
                                val == null ? null : helper.GetValueXml(val));
                            element.Add(field);
                        }
                        else
                        {
                            Type baseType = GetBaseSerializeType(item.Value.Field.FieldType);
                            element.Add(mListSerializeFunc[baseType].SerializeFunc(helper, item.Value.Field, val));
                        }
                        helper.PopName();
                    }
                }
                return element;
            }
            else
            {
                return SerializeWithHelper_Simple(helper, obj);
            }
        }

        /// <summary>
        /// 反序列化对象
        /// </summary>
        /// <param name="helper">助手</param>
        /// <param name="xml">Xml数据</param>
        /// <param name="obj">对象，为空时自动新建，否则修改字段值</param>
        /// <returns>对象</returns>
        private static void DeserializeWithHelper(XmlReadHelper helper, XElement xml, ref object obj)
        {
            XAttribute typeAtt = GetXAttribute(xml, Const_XmlNameAtt_Type);
            Type objType = FromXml_TypeSimple(helper, typeAtt);
            if (IsSerializableType(objType))
            {
                if (obj != null && obj.GetType() != objType)
                {
                    throw new Exception("给定的对象和xml的变量类型不匹配");
                }
                if (obj == null)
                {
                    obj = FromXml_Object(helper, xml);
                    // 当前值为空
                    if (obj == null) return;
                }
                helper.AddObjeInPath(obj);
                if (!helper.DictSerializeFieldForType.ContainsKey(objType))
                {
                    helper.DictSerializeFieldForType.Add(objType, GetSerializableFields(objType));
                }
                IDictionary<string, (FieldInfo Field, string Comment)> dict = helper.DictSerializeFieldForType[objType];
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
                    mListSerializeFunc[varTypeBase].DeserializeFunc(helper, field, ref obj);
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
                    dict[nameAtt.Value].Field.SetValue(obj, value);
                    helper.PopName();
                }
            }
            else
            {
                obj = DeserializeWithHelper_Simple(helper, xml, objType);
            }
        }

        /// <summary>
        /// 是否为可序列化类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static bool IsSerializableType(Type type)
        {
            Type baseType = type;
            if (typeof(Array).IsAssignableFrom(baseType))
            {
                baseType = typeof(Array);
            }
            else if (baseType.IsGenericType)
            {
                baseType = baseType.GetGenericTypeDefinition();
                if (typeof(IList).IsAssignableFrom(baseType))
                {
                    baseType = typeof(IList);
                }
                else if (typeof(IDictionary).IsAssignableFrom(baseType))
                {
                    baseType = typeof(IDictionary);
                }
            }
            else if (typeof(Enum).IsAssignableFrom(baseType))
            {
                baseType = typeof(Enum);
            }
            else
            {
                foreach (Type assetType in Const_ListAssetType)
                {
                    if (assetType == baseType)
                    {
                        baseType = typeof(UnityEngine.Object);
                    }
                }
            }
            return !mListSerializeFunc.ContainsKey(baseType);
        }

        /// <summary>
        /// 获取序列化基础变量类型，此类型决定了具体的处理方式
        /// </summary>
        /// <param name="type">原始类型</param>
        /// <returns>转换类型</returns>
        private static Type GetBaseSerializeType(Type type)
        {
            Type baseType = type;
            if (typeof(Array).IsAssignableFrom(baseType))
            {
                baseType = typeof(Array);
            }
            else if (baseType.IsGenericType)
            {
                baseType = baseType.GetGenericTypeDefinition();
                if (typeof(IList).IsAssignableFrom(baseType))
                {
                    baseType = typeof(IList);
                }
                else if (typeof(IDictionary).IsAssignableFrom(baseType))
                {
                    baseType = typeof(IDictionary);
                }
            }
            else if (typeof(Enum).IsAssignableFrom(baseType))
            {
                baseType = typeof(Enum);
            }
            else
            {
                foreach (Type assetType in Const_ListAssetType)
                {
                    if (assetType == baseType)
                    {
                        baseType = typeof(UnityEngine.Object);
                    }
                }
            }
            foreach (var item in mListSerializeFunc)
            {
                if (baseType == item.Key)
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
        public static IDictionary<string, (FieldInfo Field, string Comment)> GetSerializableFields(Type type)
        {
            List<FieldInfo> fields = type.GetFields().ToList().Where(r => r.IsDefined(typeof(FrogSerializableAttribute), true)).ToList();
            Dictionary<string, (FieldInfo Field, string Comment)> dict = new Dictionary<string, (FieldInfo Field, string Comment)>();
            foreach (FieldInfo field in fields)
            {
                List<FrogSerializableAttribute> atts = field.GetCustomAttributes(true).Where(r => r.GetType() == typeof(FrogSerializableAttribute)).Cast<FrogSerializableAttribute>().ToList();
                if (atts.Count == 0) continue;
                FrogSerializableAttribute att = atts.First();
                dict.Add(field.Name, (field, att.Comment));
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
                ToXml_TypeSimple(helper, field.FieldType)
                );
        }

        /// <summary>
        /// 反序列化字段(Bool)
        /// </summary>
        /// <param name="helper">助手</param>
        /// <param name="xml">Xml数据</param>
        /// <param name="obj">字段所属对象</param>
        /// <returns>Xml数据</returns>
        private static void FromXml_Bool(XmlReadHelper helper, XElement xml, ref object obj)
        {
            XAttribute fieldAtt = GetXAttribute(xml, Const_XmlNameAtt_Field, true, true);
            if (fieldAtt != null)
            {
                string fieldName = fieldAtt.Value;
                FieldInfo field = obj.GetType().GetField(fieldName);
                XAttribute valAtt = GetXAttribute(xml, Const_XmlNameAtt_Value);
                field.SetValue(obj, (bool)valAtt);
            }
            else
            {
                XAttribute valAtt = GetXAttribute(xml, Const_XmlNameAtt_Value);
                obj = (bool)valAtt;
            }
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
                ToXml_TypeSimple(helper, field.FieldType)
                );
        }

        /// <summary>
        /// 反序列化字段(Byte)
        /// </summary>
        /// <param name="helper">助手</param>
        /// <param name="xml">Xml数据</param>
        /// <param name="obj">字段所属对象</param>
        /// <returns>Xml数据</returns>
        private static void FromXml_Byte(XmlReadHelper helper, XElement xml, ref object obj)
        {
            XAttribute fieldAtt = GetXAttribute(xml, Const_XmlNameAtt_Field, true, true);
            if (fieldAtt != null)
            {
                string fieldName = fieldAtt.Value;
                FieldInfo field = obj.GetType().GetField(fieldName);
                XAttribute valAtt = GetXAttribute(xml, Const_XmlNameAtt_Value);
                field.SetValue(obj, byte.Parse(valAtt.Value));
            }
            else
            {
                XAttribute valAtt = GetXAttribute(xml, Const_XmlNameAtt_Value);
                obj = byte.Parse(valAtt.Value);
            }
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
                ToXml_TypeSimple(helper, field.FieldType)
                );
        }

        /// <summary>
        /// 反序列化字段(Char)
        /// </summary>
        /// <param name="helper">助手</param>
        /// <param name="xml">Xml数据</param>
        /// <param name="obj">字段所属对象</param>
        /// <returns>Xml数据</returns>
        private static void FromXml_Char(XmlReadHelper helper, XElement xml, ref object obj)
        {
            XAttribute fieldAtt = GetXAttribute(xml, Const_XmlNameAtt_Field, true, true);
            if (fieldAtt != null)
            {
                string fieldName = fieldAtt.Value;
                FieldInfo field = obj.GetType().GetField(fieldName);
                XAttribute valAtt = GetXAttribute(xml, Const_XmlNameAtt_Value);
                field.SetValue(obj, char.Parse(valAtt.Value));
            }
            else
            {
                XAttribute valAtt = GetXAttribute(xml, Const_XmlNameAtt_Value);
                obj = char.Parse(valAtt.Value);
            }
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
                ToXml_TypeSimple(helper, field.FieldType)
                );
        }

        /// <summary>
        /// 反序列化字段(Decimal)
        /// </summary>
        /// <param name="helper">助手</param>
        /// <param name="xml">Xml数据</param>
        /// <param name="obj">字段所属对象</param>
        /// <returns>Xml数据</returns>
        private static void FromXml_Decimal(XmlReadHelper helper, XElement xml, ref object obj)
        {
            XAttribute fieldAtt = GetXAttribute(xml, Const_XmlNameAtt_Field, true, true);
            if (fieldAtt != null)
            {
                string fieldName = fieldAtt.Value;
                FieldInfo field = obj.GetType().GetField(fieldName);
                XAttribute valAtt = GetXAttribute(xml, Const_XmlNameAtt_Value);
                field.SetValue(obj, decimal.Parse(valAtt.Value));
            }
            else
            {
                XAttribute valAtt = GetXAttribute(xml, Const_XmlNameAtt_Value);
                obj = decimal.Parse(valAtt.Value);
            }
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
                ToXml_TypeSimple(helper, field.FieldType)
                );
        }

        /// <summary>
        /// 反序列化字段(Double)
        /// </summary>
        /// <param name="helper">助手</param>
        /// <param name="xml">Xml数据</param>
        /// <param name="obj">字段所属对象</param>
        /// <returns>Xml数据</returns>
        private static void FromXml_Double(XmlReadHelper helper, XElement xml, ref object obj)
        {
            XAttribute fieldAtt = GetXAttribute(xml, Const_XmlNameAtt_Field, true, true);
            if (fieldAtt != null)
            {
                string fieldName = fieldAtt.Value;
                FieldInfo field = obj.GetType().GetField(fieldName);
                XAttribute valAtt = GetXAttribute(xml, Const_XmlNameAtt_Value);
                field.SetValue(obj, (double)valAtt);
            }
            else
            {
                XAttribute valAtt = GetXAttribute(xml, Const_XmlNameAtt_Value);
                obj = (double)valAtt;
            }
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
                ToXml_TypeSimple(helper, field.FieldType)
                );
        }

        /// <summary>
        /// 反序列化字段(Enum)
        /// </summary>
        /// <param name="helper">助手</param>
        /// <param name="xml">Xml数据</param>
        /// <param name="obj">字段所属对象</param>
        /// <returns>Xml数据</returns>
        private static void FromXml_Enum(XmlReadHelper helper, XElement xml, ref object obj)
        {
            XAttribute fieldAtt = GetXAttribute(xml, Const_XmlNameAtt_Field, true, true);
            if (fieldAtt != null)
            {
                string fieldName = fieldAtt.Value;
                FieldInfo field = obj.GetType().GetField(fieldName);
                XAttribute valAtt = GetXAttribute(xml, Const_XmlNameAtt_Value);
                XAttribute typeXml = GetXAttribute(xml, Const_XmlNameAtt_Type);
                Type type = FromXml_TypeSimple(helper, typeXml);
                field.SetValue(obj, Enum.Parse(type, valAtt.Value));
            }
            else
            {
                XAttribute valAtt = GetXAttribute(xml, Const_XmlNameAtt_Value);
                XAttribute typeXml = GetXAttribute(xml, Const_XmlNameAtt_Type);
                Type type = FromXml_TypeSimple(helper, typeXml);
                obj = Enum.Parse(type, valAtt.Value);
            }
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
                ToXml_TypeSimple(helper, field.FieldType)
                );
        }

        /// <summary>
        /// 反序列化字段(Float)
        /// </summary>
        /// <param name="helper">助手</param>
        /// <param name="xml">Xml数据</param>
        /// <param name="obj">字段所属对象</param>
        /// <returns>Xml数据</returns>
        private static void FromXml_Float(XmlReadHelper helper, XElement xml, ref object obj)
        {
            XAttribute fieldAtt = GetXAttribute(xml, Const_XmlNameAtt_Field, true, true);
            if (fieldAtt != null)
            {
                string fieldName = fieldAtt.Value;
                FieldInfo field = obj.GetType().GetField(fieldName);
                XAttribute valAtt = GetXAttribute(xml, Const_XmlNameAtt_Value);
                field.SetValue(obj, (float)valAtt);
            }
            else
            {
                XAttribute valAtt = GetXAttribute(xml, Const_XmlNameAtt_Value);
                obj = (float)valAtt;
            }
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
                ToXml_TypeSimple(helper, field.FieldType)
                );
        }

        /// <summary>
        /// 反序列化字段(Int)
        /// </summary>
        /// <param name="helper">助手</param>
        /// <param name="xml">Xml数据</param>
        /// <param name="obj">字段所属对象</param>
        /// <returns>Xml数据</returns>
        private static void FromXml_Int(XmlReadHelper helper, XElement xml, ref object obj)
        {
            XAttribute fieldAtt = GetXAttribute(xml, Const_XmlNameAtt_Field, true, true);
            if (fieldAtt != null)
            {
                string fieldName = fieldAtt.Value;
                FieldInfo field = obj.GetType().GetField(fieldName);
                XAttribute valAtt = GetXAttribute(xml, Const_XmlNameAtt_Value);
                field.SetValue(obj, (int)valAtt);
            }
            else
            {
                XAttribute valAtt = GetXAttribute(xml, Const_XmlNameAtt_Value);
                obj = (int)valAtt;
            }
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
                ToXml_TypeSimple(helper, field.FieldType)
                );
        }

        /// <summary>
        /// 反序列化字段(Long)
        /// </summary>
        /// <param name="helper">助手</param>
        /// <param name="xml">Xml数据</param>
        /// <param name="obj">字段所属对象</param>
        /// <returns>Xml数据</returns>
        private static void FromXml_Long(XmlReadHelper helper, XElement xml, ref object obj)
        {
            XAttribute fieldAtt = GetXAttribute(xml, Const_XmlNameAtt_Field, true, true);
            if (fieldAtt != null)
            {
                string fieldName = fieldAtt.Value;
                FieldInfo field = obj.GetType().GetField(fieldName);
                XAttribute valAtt = GetXAttribute(xml, Const_XmlNameAtt_Value);
                field.SetValue(obj, (long)valAtt);
            }
            else
            {
                XAttribute valAtt = GetXAttribute(xml, Const_XmlNameAtt_Value);
                obj = (long)valAtt;
            }
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
                ToXml_TypeSimple(helper, field.FieldType)
                );
        }

        /// <summary>
        /// 反序列化字段(SByte)
        /// </summary>
        /// <param name="helper">助手</param>
        /// <param name="xml">Xml数据</param>
        /// <param name="obj">字段所属对象</param>
        /// <returns>Xml数据</returns>
        private static void FromXml_SByte(XmlReadHelper helper, XElement xml, ref object obj)
        {
            XAttribute fieldAtt = GetXAttribute(xml, Const_XmlNameAtt_Field, true, true);
            if (fieldAtt != null)
            {
                string fieldName = fieldAtt.Value;
                FieldInfo field = obj.GetType().GetField(fieldName);
                XAttribute valAtt = GetXAttribute(xml, Const_XmlNameAtt_Value);
                field.SetValue(obj, (sbyte)valAtt);
            }
            else
            {
                XAttribute valAtt = GetXAttribute(xml, Const_XmlNameAtt_Value);
                obj = (sbyte)valAtt;
            }
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
                ToXml_TypeSimple(helper, field.FieldType)
                );
        }

        /// <summary>
        /// 反序列化字段(Short)
        /// </summary>
        /// <param name="helper">助手</param>
        /// <param name="xml">Xml数据</param>
        /// <param name="obj">字段所属对象</param>
        /// <returns>Xml数据</returns>
        private static void FromXml_Short(XmlReadHelper helper, XElement xml, ref object obj)
        {
            XAttribute fieldAtt = GetXAttribute(xml, Const_XmlNameAtt_Field, true, true);
            if (fieldAtt != null)
            {
                string fieldName = fieldAtt.Value;
                FieldInfo field = obj.GetType().GetField(fieldName);
                XAttribute valAtt = GetXAttribute(xml, Const_XmlNameAtt_Value);
                field.SetValue(obj, (short)valAtt);
            }
            else
            {
                XAttribute valAtt = GetXAttribute(xml, Const_XmlNameAtt_Value);
                obj = (short)valAtt;
            }
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
        /// <param name="obj">字段所属对象</param>
        /// <returns>Xml数据</returns>
        private static void FromXml_String(XmlReadHelper helper, XElement xml, ref object obj)
        {
            XAttribute fieldAtt = GetXAttribute(xml, Const_XmlNameAtt_Field, true, true);
            if (fieldAtt != null)
            {
                string fieldName = fieldAtt.Value;
                FieldInfo field = obj.GetType().GetField(fieldName);
                XAttribute valAtt = GetXAttribute(xml, Const_XmlNameAtt_Value, true, true);
                field.SetValue(obj, valAtt?.Value);
            }
            else
            {
                XAttribute valAtt = GetXAttribute(xml, Const_XmlNameAtt_Value);
                obj = valAtt.Value;
            }
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
                ToXml_TypeSimple(helper, field.FieldType)
                );
        }

        /// <summary>
        /// 反序列化字段(UInt)
        /// </summary>
        /// <param name="helper">助手</param>
        /// <param name="xml">Xml数据</param>
        /// <param name="obj">字段所属对象</param>
        /// <returns>Xml数据</returns>
        private static void FromXml_UInt(XmlReadHelper helper, XElement xml, ref object obj)
        {
            XAttribute fieldAtt = GetXAttribute(xml, Const_XmlNameAtt_Field, true, true);
            if (fieldAtt != null)
            {
                string fieldName = fieldAtt.Value;
                FieldInfo field = obj.GetType().GetField(fieldName);
                XAttribute valAtt = GetXAttribute(xml, Const_XmlNameAtt_Value);
                field.SetValue(obj, (uint)valAtt);
            }
            else
            {
                XAttribute valAtt = GetXAttribute(xml, Const_XmlNameAtt_Value);
                obj = (uint)valAtt;
            }
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
                ToXml_TypeSimple(helper, field.FieldType)
                );
        }

        /// <summary>
        /// 反序列化字段(ULong)
        /// </summary>
        /// <param name="helper">助手</param>
        /// <param name="xml">Xml数据</param>
        /// <param name="obj">字段所属对象</param>
        /// <returns>Xml数据</returns>
        private static void FromXml_ULong(XmlReadHelper helper, XElement xml, ref object obj)
        {
            XAttribute fieldAtt = GetXAttribute(xml, Const_XmlNameAtt_Field, true, true);
            if (fieldAtt != null)
            {
                string fieldName = fieldAtt.Value;
                FieldInfo field = obj.GetType().GetField(fieldName);
                XAttribute valAtt = GetXAttribute(xml, Const_XmlNameAtt_Value);
                field.SetValue(obj, (ulong)valAtt);
            }
            else
            {
                XAttribute valAtt = GetXAttribute(xml, Const_XmlNameAtt_Value);
                obj = (ulong)valAtt;
            }
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
                ToXml_TypeSimple(helper, field.FieldType)
                );
        }

        /// <summary>
        /// 反序列化字段(UShort)
        /// </summary>
        /// <param name="helper">助手</param>
        /// <param name="xml">Xml数据</param>
        /// <param name="obj">字段所属对象</param>
        /// <returns>Xml数据</returns>
        private static void FromXml_UShort(XmlReadHelper helper, XElement xml, ref object obj)
        {
            XAttribute fieldAtt = GetXAttribute(xml, Const_XmlNameAtt_Field, true, true);
            if (fieldAtt != null)
            {
                string fieldName = fieldAtt.Value;
                FieldInfo field = obj.GetType().GetField(fieldName);
                XAttribute valAtt = GetXAttribute(xml, Const_XmlNameAtt_Value);
                field.SetValue(obj, ushort.Parse(valAtt.Value));
            }
            else
            {
                XAttribute valAtt = GetXAttribute(xml, Const_XmlNameAtt_Value);
                obj = ushort.Parse(valAtt.Value);
            }
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
                   ToXml_TypeSimple(helper, field.FieldType)
                   );
            }
            if (!(val is UnityEngine.Object obj))
            {
                throw new Exception("错误的资源类型");
            }
            string path = AssetDatabase.GetAssetPath(obj);
            return GenerateXElement(field != null ? Const_XmlNameEle_NonSerializedField : Const_XmlNameEle_NonSerializeObject,
                field != null ? GenerateXAttribute(Const_XmlNameAtt_Field, field.Name) : null,
                GenerateXAttribute(Const_XmlNameAtt_Value, path),
                ToXml_TypeSimple(helper, field.FieldType)
                );
        }

        /// <summary>
        /// 反序列化字段(Asset)
        /// </summary>
        /// <param name="helper">助手</param>
        /// <param name="xml">Xml数据</param>
        /// <param name="obj">字段所属对象</param>
        /// <returns>Xml数据</returns>
        private static void FromXml_Asset(XmlReadHelper helper, XElement xml, ref object obj)
        {
            XAttribute fieldAtt = GetXAttribute(xml, Const_XmlNameAtt_Field, true, true);
            if (fieldAtt != null)
            {
                string fieldName = fieldAtt.Value;
                FieldInfo field = obj.GetType().GetField(fieldName);
                XAttribute valAtt = GetXAttribute(xml, Const_XmlNameAtt_Value, true, true);
                if (valAtt == null)
                {
                    field.SetValue(obj, null);
                }
                else
                {
                    UnityEngine.Object asset = AssetDatabase.LoadMainAssetAtPath(valAtt.Value);
                    field.SetValue(obj, asset);
                }
            }
            else
            {
                XAttribute valAtt = GetXAttribute(xml, Const_XmlNameAtt_Value, true, true);
                obj = valAtt == null ? null : AssetDatabase.LoadMainAssetAtPath(valAtt.Value);
            }
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
                    ToXml_TypeSimple(helper, field.FieldType)
                    );
            }
            if (!(val is Array array))
            {
                throw new Exception("错误的Array类型");
            }
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
                    XComment comment = new XComment($"路径：{helper.PeekName()}");
                    element.Add(comment);
                }
                element.Add(helper.GetValueXml(child));
                helper.PopName();
                index++;
            }
            return GenerateXElement(field != null ? Const_XmlNameEle_NonSerializedField : Const_XmlNameEle_NonSerializeObject,
                field != null ? GenerateXAttribute(Const_XmlNameAtt_Field, field.Name) : null,
                element,
                ToXml_TypeSimple(helper, field.FieldType)
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
        /// <param name="obj">字段所属对象</param>
        /// <returns>Xml数据</returns>
        private static void FromXml_Array(XmlReadHelper helper, XElement xml, ref object obj)
        {
            XAttribute fieldAtt = GetXAttribute(xml, Const_XmlNameAtt_Field, true, true);
            XElement arrayXml = GetXElement(xml, Const_XmlNameEle_Object, true, true);
            if (fieldAtt != null)
            {
                string fieldName = fieldAtt.Value;
                FieldInfo field = obj.GetType().GetField(fieldName);
                field.SetValue(obj, arrayXml == null ? null : FromXml_ArrayCreate(helper, arrayXml));
            }
            else
            {
                obj = arrayXml == null ? null : FromXml_ArrayCreate(helper, arrayXml);
            }
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
                    ToXml_TypeSimple(helper, field.FieldType)
                    );
            }
            if (!(val is IList list))
            {
                throw new Exception("错误的List类型");
            }
            XElement element = ToXml_Object(helper, val);
            int index = 1;
            foreach (object child in list)
            {
                XName name = XName.Get(Const_XmlNameOth_List, index.ToString());
                helper.PushName(name);
                if (PrintCommentPath)
                {
                    XComment comment = new XComment($"路径：{helper.PeekName()}");
                    element.Add(comment);
                }
                element.Add(helper.GetValueXml(child));
                helper.PopName();
                index++;
            }
            return GenerateXElement(field != null ? Const_XmlNameEle_NonSerializedField : Const_XmlNameEle_NonSerializeObject,
                field != null ? GenerateXAttribute(Const_XmlNameAtt_Field, field.Name) : null,
                element,
                ToXml_TypeSimple(helper, field.FieldType)
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
        /// <param name="obj">字段所属对象</param>
        /// <returns>Xml数据</returns>
        private static void FromXml_List(XmlReadHelper helper, XElement xml, ref object obj)
        {
            XAttribute fieldAtt = GetXAttribute(xml, Const_XmlNameAtt_Field, true, true);
            XElement listXml = GetXElement(xml, Const_XmlNameEle_Object, true, true);
            if (fieldAtt != null)
            {
                string fieldName = fieldAtt.Value;
                FieldInfo field = obj.GetType().GetField(fieldName);
                field.SetValue(obj, listXml == null ? null : FromXml_ListCreate(helper, listXml));
            }
            else
            {
                obj = listXml == null ? null : FromXml_ListCreate(helper, listXml);
            }
        }

        #endregion List

        //#region Dictionary

        ///// <summary>
        ///// 序列化字段(Dictionary)
        ///// </summary>
        ///// <param name="helper">助手</param>
        ///// <param name="field">字段</param>
        ///// <param name="val">值</param>
        ///// <returns>Xml数据</returns>
        //private static XElement ToXml_Dictionary(XmlWriteHelper helper, FieldInfo field, object val)
        //{
        //    if (!(val is UnityEngine.Object obj))
        //    {
        //        throw new Exception("错误的资源类型");
        //    }
        //    string path = DictionaryDatabase.GetDictionaryPath(obj);
        //    return GenerateXElement(field != null ? Const_XmlNameEle_NonSerializedField : Const_XmlNameEle_NonSerializeObject,
        //        field != null ? GenerateXAttribute(Const_XmlNameAtt_Field, field.Name) : null,
        //        GenerateXAttribute(Const_XmlNameAtt_Value, path),
        //        ToXml_TypeSimple(helper, field.FieldType)
        //        );
        //}

        ///// <summary>
        ///// 反序列化字段(Dictionary)
        ///// </summary>
        ///// <param name="helper">助手</param>
        ///// <param name="xml">Xml数据</param>
        ///// <param name="obj">字段所属对象</param>
        ///// <returns>Xml数据</returns>
        //private static void FromXml_Dictionary(XmlReadHelper helper, XElement xml, ref object obj)
        //{
        //    string fieldName = GetXAttribute(xml, Const_XmlNameAtt_Field).Value;
        //    FieldInfo field = obj.GetType().GetField(fieldName);
        //    XAttribute valAtt = GetXAttribute(xml, Const_XmlNameAtt_Value);
        //    UnityEngine.Object Dictionary = DictionaryDatabase.LoadMainDictionaryAtPath(valAtt.Value);
        //    field.SetValue(obj, Dictionary);
        //}

        //#endregion Dictionary

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
        /// <param name="obj">对象</param>
        /// <returns>XEl对象</returns>
        private static XElement ToXml_Object(XmlWriteHelper helper, object obj)
        {
            if (obj == null) return GenerateXElement(Const_XmlNameEle_Null);
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
        /// <param name="args">构造函数参数</param>
        /// <returns>实例对象</returns>
        private static object FromXml_Object(XmlReadHelper helper, XElement xml, params object[] args)
        {
            if (xml.Name == Const_XmlNameEle_Null) return null;
            CheckXelementName(xml, Const_XmlNameEle_Object);
            XAttribute typeXML = GetXAttribute(xml, Const_XmlNameAtt_Type);
            Type type = FromXml_TypeSimple(helper, typeXML);
            return Activator.CreateInstance(type, args == null ? Const_NoElementObjectArray : args);
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
                GenerateXElement(Const_XmlNameDoc_TypeList),
                GenerateXElement(Const_XmlNameDoc_ObjectList)
                );
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
        private static (string Name, Type Type) FromXml_TypeInfo(XElement xml)
        {
            XAttribute nameAtt = GetXAttribute(xml, Const_XmlNameAtt_Name);
            XElement typeXml = GetXElement(xml, Const_XmlNameEle_Type);
            Type type = FromXml_Type(typeXml);
            return (nameAtt.Value, type);
        }

        #endregion Document

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
