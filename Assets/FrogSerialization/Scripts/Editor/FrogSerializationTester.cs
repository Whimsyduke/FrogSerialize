using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using UnityEngine;
using UnityEditor;

namespace FrogSerialization
{
    #region 测试器

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
    /// 测试器
    /// </summary>
    public static class Const_TestEditorTool_Tester
    {
        #region 内部声明

        #region 常量

        #endregion 常量

        #region 枚举

        #endregion 枚举

        #region 定义
        /// <summary>
        /// 测试类基础
        /// </summary>
        private abstract class Test_ToXmlBase
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

            #endregion 属性

            #region 字段

            /// <summary>
            /// 父类字段
            /// </summary>
            [FrogSerializable(Comment = "Parent Field")]
            public int ParentField = Test_ToXml.IntValDefault;

            /// <summary>
            /// 已随机
            /// </summary>
            public bool HasRandom = false;

            #endregion 字段

            #region 事件

            #endregion 事件

            #endregion 属性字段

            #region 构造函数

            #endregion 构造函数

            #region 方法

            #region 通用方法

            /// <summary>
            /// 随机字段
            /// </summary>
            public abstract void RandomField();

            #endregion 通用方法

            #region 重写方法

            #endregion 重写方法

            #region 事件方法

            #endregion 事件方法 

            #endregion 方法

        }


        /// <summary>
        /// 测试类继承
        /// </summary>
        private class Test_ToXml : Test_ToXmlBase
        {
            #region 内部声明

            #region 常量

            private const string Const_PathMaterialA = "Assets/FrogSerialization/Material/MaterialA.mat";
            private const string Const_PathMaterialB = "Assets/FrogSerialization/Material/MaterialB.mat";
            private const string Const_PathMaterialC = "Assets/FrogSerialization/Material/MaterialC.mat";

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

            #endregion 属性

            #region 字段

            #region 测试字段

            #region Dictionary

            /// <summary>
            /// 列表Val
            /// </summary>
            [FrogSerializable(Comment = "Dictionary")]
            public Dictionary<int, Test_ToXmlOther> DictionaryVal;

            #endregion Dictionary

            #region Name

            /// <summary>
            /// 标记类的名称的字段
            /// </summary>
            [FrogSerializable(Comment = "Name")]
            public string Name;

            #endregion Name

            #region Null

            /// <summary>
            /// 空值
            /// </summary>
            [FrogSerializable(Comment = "Null")]
            public string NullVal = null;

            #endregion Null

            #region Bool

            /// <summary>
            /// bool默认常量值
            /// </summary>
            public const bool BoolValDefault = true;

            /// <summary>
            /// bool字段
            /// </summary>
            [FrogSerializable(Comment = "Bool")]
            public bool BoolVal = BoolValDefault;

            #endregion Bool

            #region Byte

            /// <summary>
            /// Byte默认常量值
            /// </summary>
            public const byte ByteValDefault = 127;

            /// <summary>
            /// Byte字段
            /// </summary>
            [FrogSerializable(Comment = "Byte")]
            public byte ByteVal = ByteValDefault;

            #endregion Byte

            #region Char

            /// <summary>
            /// Char默认常量值
            /// </summary>
            public const char CharValDefault = ' ';

            /// <summary>
            /// Char字段
            /// </summary>
            [FrogSerializable(Comment = "Char")]
            public char CharVal = CharValDefault;

            #endregion Char

            #region Decimal

            /// <summary>
            /// Decimal默认常量值
            /// </summary>
            public const decimal DecimalValDefault = 1.23456789m;

            /// <summary>
            /// Decimal字段
            /// </summary>
            [FrogSerializable(Comment = "Decimal")]
            public decimal DecimalVal = DecimalValDefault;

            #endregion Decimal

            #region Double

            /// <summary>
            /// Double默认常量值
            /// </summary>
            public const double DoubleValDefault = 1.23456789;

            /// <summary>
            /// Double字段
            /// </summary>
            [FrogSerializable(Comment = "Double")]
            public double DoubleVal = DoubleValDefault;

            #endregion Double

            #region Enum

            public enum EnumTest
            {
                A,
                B,
                C,
                D,
                E,
            }

            /// <summary>
            /// Enum默认常量值
            /// </summary>
            public const EnumTest EnumValDefault = EnumTest.E;
            /// <summary>

            /// <summary>
            /// Enum字段
            /// </summary>
            [FrogSerializable(Comment = "Enum")]
            public EnumTest EnumVal = EnumValDefault;

            #endregion Enum

            #region Float

            /// <summary>
            /// Enum默认常量值
            /// </summary>
            public const float FloatValDefault = 1.23456789F;

            /// <summary>
            /// Float字段
            /// </summary>
            [FrogSerializable(Comment = "Float")]
            public float FloatVal = FloatValDefault;

            #endregion Float

            #region Int

            /// <summary>
            /// Int默认常量值
            /// </summary>
            public const int IntValDefault = 1024;

            /// <summary>
            /// Int字段
            /// </summary>
            [FrogSerializable(Comment = "Int")]
            public int IntVal = IntValDefault;

            #endregion Int

            #region Long

            /// <summary>
            /// Long默认常量值
            /// </summary>
            public const long LongValDefault = 1024;


            /// <summary>
            /// Long字段
            /// </summary>
            [FrogSerializable(Comment = "Long")]
            public long LongVal = LongValDefault;

            #endregion Long

            #region SByte

            /// <summary>
            /// SByte默认常量值
            /// </summary>
            public const sbyte SByteValDefault = -127;

            /// <summary>
            /// SByte字段
            /// </summary>
            [FrogSerializable(Comment = "SByte")]
            public sbyte SByteVal = SByteValDefault;

            #endregion SByte

            #region Short

            /// <summary>
            /// Short默认常量值
            /// </summary>
            public const short ShortValDefault = 1024;

            /// <summary>
            /// Short字段
            /// </summary>
            [FrogSerializable(Comment = "Short")]
            public short ShortVal = ShortValDefault;

            #endregion Short

            #region String

            /// <summary>
            /// String默认常量值
            /// </summary>
            public const string StringValDefault = "TEST_DEFAULT";
            /// <summary>

            /// <summary>
            /// String字段
            /// </summary>
            [FrogSerializable(Comment = "String")]
            public string StringVal = StringValDefault;

            #endregion String

            #region UInt

            /// <summary>
            /// UInt默认常量值
            /// </summary>
            public const uint UIntValDefault = 1024;

            /// <summary>
            /// UInt字段
            /// </summary>
            [FrogSerializable(Comment = "UInt")]
            public uint UIntVal = UIntValDefault;

            #endregion UInt

            #region ULong

            /// <summary>
            /// ULong默认常量值
            /// </summary>
            public const ulong ULongValDefault = 1024;

            /// <summary>
            /// ULong字段
            /// </summary>
            [FrogSerializable(Comment = "ULong")]
            public ulong ULongVal = ULongValDefault;

            #endregion Long

            #region UShort

            /// <summary>
            /// UShort默认常量值
            /// </summary>
            public const ushort UShortValDefault = 1024;

            /// <summary>
            /// UShort字段
            /// </summary>
            [FrogSerializable(Comment = "UShort")]
            public ushort UShortVal = UShortValDefault;

            #endregion UShort

            #region Asset

            /// <summary>
            /// Asset默认常量值
            /// </summary>
            public readonly  Material MaterialValDefault = AssetDatabase.LoadMainAssetAtPath(Const_PathMaterialA) as Material;

            /// <summary>
            /// Asset字段
            /// </summary>
            [FrogSerializable(Comment = "Asset")]
            public Material MaterialVal = AssetDatabase.LoadMainAssetAtPath(Const_PathMaterialA) as Material;

            #endregion Asset

            #region Serializable

            /// <summary>
            /// 可序列化字段
            /// </summary>
            [FrogSerializable(Comment = "Serializable")]
            public Test_ToXmlOther SerializableVal = new Test_ToXmlOther();

            #endregion Serializable

            #region Array

            /// <summary>
            /// 列表Val
            /// </summary>
            [FrogSerializable(Comment = "Array")]
            public Test_ToXmlOther[] ArrayVal;

            #endregion Array

            #region List

            /// <summary>
            /// 列表Val
            /// </summary>
            [FrogSerializable(Comment = "List")]
            public List<Test_ToXmlOther> ListVal;

            #endregion List

            #region 不序列化属性

            /// <summary>
            /// 不会序列化的属性
            /// </summary>
            public int NonSerializedInt = IntValDefault;

            #endregion 不序列化属性

            #endregion 测试字段

            #endregion 字段

            #region 事件

            #endregion 事件

            #endregion 属性字段

            #region 构造函数

            #endregion 构造函数

            #region 方法

            #region 通用方法

            /// <summary>
            /// 随机字段
            /// </summary>
            public override void RandomField()
            {
                if (HasRandom) return;
                HasRandom = true;
                System.Random random = new System.Random();
                ParentField = random.Next(int.MinValue, int.MaxValue);
                BoolVal = random.Next(0,2) == 0;
                ByteVal = (byte)random.Next(byte.MinValue, byte.MaxValue);
                CharVal = (char)random.Next('\u4E00', '\u9FA5');
                DecimalVal = (decimal)random.Next(int.MinValue, int.MaxValue);
                DoubleVal = (double)random.Next(int.MinValue, int.MaxValue);
                EnumVal = (EnumTest)random.Next((int)EnumTest.A, (int)EnumTest.E);
                FloatVal = (float)random.Next(int.MinValue, int.MaxValue);
                IntVal = random.Next(int.MinValue, int.MaxValue);
                LongVal = (long)random.Next(int.MinValue, int.MaxValue);
                SByteVal = (sbyte)random.Next(sbyte.MinValue, sbyte.MaxValue);
                ShortVal = (short)random.Next(short.MinValue, short.MaxValue);
                StringVal = random.Next(int.MinValue, int.MaxValue).ToString();
                UIntVal = (uint)random.Next(0, int.MaxValue);
                ULongVal = (ulong)random.Next(0, int.MaxValue);
                UShortVal = (ushort)random.Next(ushort.MinValue, ushort.MaxValue);
                MaterialVal = AssetDatabase.LoadMainAssetAtPath(random.Next(0, 2) == 0? Const_PathMaterialB : Const_PathMaterialC) as Material;
                NonSerializedInt = random.Next(int.MinValue, IntValDefault);
                SerializableVal.RandomField();
                ArrayVal = new Test_ToXmlOther[random.Next(2, 6)];
                for (int i = 0; i < ArrayVal.Count(); i++)
                {
                    ArrayVal[i] = new Test_ToXmlOther();
                    ArrayVal[i].RandomField();
                }
                ListVal = new List<Test_ToXmlOther>();
                for (int i = 0; i < random.Next(2, 6); i++)
                {
                    ListVal.Add(new Test_ToXmlOther());
                    ListVal[i].RandomField();
                }
                DictionaryVal = new Dictionary<int, Test_ToXmlOther>();
                for (int i = 0; i < random.Next(2, 6); i++)
                {
                    DictionaryVal.Add(i, new Test_ToXmlOther());
                    DictionaryVal[i].RandomField();
                }
            }

            /// <summary>
            /// 数据相同验证
            /// </summary>
            /// <param name="tester">测试对象</param>
            /// <param name="listHasTest">已测试对象</param>
            /// <returns>测试结果</returns>
            public bool ValueEqual(Test_ToXml tester, List<Test_ToXmlBase> listHasTest)
            {
                if (listHasTest.Contains(this)) return true;
                listHasTest.Add(this);
                if (tester == null) throw new Exception("Null object to test.");
                if (ParentField != tester.ParentField) throw new Exception($"{nameof(ParentField)} is not equal!");
                if (BoolVal != tester.BoolVal) throw new Exception($"{nameof(BoolVal)} is not equal!");
                if (ByteVal != tester.ByteVal) throw new Exception($"{nameof(ByteVal)} is not equal!");
                if (CharVal != tester.CharVal) throw new Exception($"{nameof(CharVal)} is not equal!");
                if (DecimalVal != tester.DecimalVal) throw new Exception($"{nameof(DecimalVal)} is not equal!");
                if (DoubleVal != tester.DoubleVal) throw new Exception($"{nameof(DoubleVal)} is not equal!");
                if (EnumVal != tester.EnumVal) throw new Exception($"{nameof(EnumVal)} is not equal!");
                if (FloatVal != tester.FloatVal) throw new Exception($"{nameof(FloatVal)} is not equal!");
                if (IntVal != tester.IntVal) throw new Exception($"{nameof(IntVal)} is not equal!");
                if (LongVal != tester.LongVal) throw new Exception($"{nameof(LongVal)} is not equal!");
                if (SByteVal != tester.SByteVal) throw new Exception($"{nameof(SByteVal)} is not equal!");
                if (ShortVal != tester.ShortVal) throw new Exception($"{nameof(ShortVal)} is not equal!");
                if (StringVal != tester.StringVal) throw new Exception($"{nameof(StringVal)} is not equal!");
                if (UIntVal != tester.UIntVal) throw new Exception($"{nameof(UIntVal)} is not equal!");
                if (ULongVal != tester.ULongVal) throw new Exception($"{nameof(ULongVal)} is not equal!");
                if (UShortVal != tester.UShortVal) throw new Exception($"{nameof(UShortVal)} is not equal!");
                if (MaterialVal.color != tester.MaterialVal.color) throw new Exception($"{nameof(MaterialVal)} is not equal!");
                if (NonSerializedInt == tester.NonSerializedInt) throw new Exception($"{nameof(NonSerializedInt)} is not equal!");
                if (!(SerializableVal).ValueEqual(tester.SerializableVal, listHasTest)) throw new Exception($"{nameof(SerializableVal)} is not equal!");
                if (ArrayVal.Count() != tester.ArrayVal.Count()) throw new Exception($"{nameof(ArrayVal)} count is not equal!");
                for (int i = 0; i < ArrayVal.Count(); i++)
                {
                    if (!ArrayVal[i].ValueEqual(tester.ArrayVal[i], listHasTest))
                        throw new Exception($"{nameof(ArrayVal)}[{i}] count is not equal!");
                }
                if (ListVal.Count != tester.ListVal.Count) throw new Exception($"{nameof(ListVal)} count is not equal!");
                for (int i = 0; i < ListVal.Count; i++)
                {
                    if (!ListVal[i].ValueEqual(tester.ListVal[i], listHasTest)) 
                        throw new Exception($"{nameof(ListVal)}[{i}] count is not equal!");
                }
                if (DictionaryVal.Count != tester.DictionaryVal.Count) throw new Exception($"{nameof(DictionaryVal)} count is not equal!");
                for (int i = 0; i < DictionaryVal.Count; i++)
                {
                    if (!DictionaryVal[i].ValueEqual(tester.DictionaryVal[i], listHasTest))
                        throw new Exception($"{nameof(DictionaryVal)}[{i}] count is not equal!");
                }
                return true;
            }

            #endregion 通用方法

            #region 重写方法

            #endregion 重写方法

            #region 事件方法

            #endregion 事件方法 

            #endregion 方法
        }

        /// <summary>
        /// 测试类其它
        /// </summary>
        private class Test_ToXmlOther : Test_ToXmlBase
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

            #endregion 属性

            #region 字段

            #region SerializableNull

            /// <summary>
            /// 可序列化字段
            /// </summary>
            [FrogSerializable(Comment = "SerializableNull")]
            public Test_ToXmlBase SerializableNullVal = null;

            #endregion SerializableNull

            #region Other

            /// <summary>
            /// 其它可序列化字段
            /// </summary>
            [FrogSerializable(Comment = "Other")]
            public Test_ToXml OtherVal;

            #endregion Other

            #region This

            /// <summary>
            /// 自身
            /// </summary>
            [FrogSerializable(Comment = "This")]
            public Test_ToXmlOther ThisVal;

            #endregion This

            #region Inherit

            /// <summary>
            /// 继承
            /// </summary>
            [FrogSerializable(Comment = "Inherit")]
            public Test_ToXmlBase InheritVal;

            #endregion Inherit

            #region Int

            /// <summary>
            /// Int默认常量值
            /// </summary>
            public const int IntValDefault = 1024;

            /// <summary>
            /// Int字段
            /// </summary>
            [FrogSerializable(Comment = "Int")]
            public int IntVal = IntValDefault;

            #endregion Int

            #endregion 字段

            #region 事件

            #endregion 事件

            #endregion 属性字段

            #region 构造函数

            /// <summary>
            /// 构造函数
            /// </summary>
            public Test_ToXmlOther()
            {
                ThisVal = this;
            }

            #endregion 构造函数

            #region 方法

            #region 通用方法

            /// <summary>
            /// 随机字段
            /// </summary>
            public override void RandomField()
            {
                if (HasRandom) return;
                HasRandom = true;
                System.Random random = new System.Random();
                IntVal = random.Next(int.MinValue, int.MaxValue);
                ParentField = random.Next(int.MinValue, int.MaxValue);
                if (OtherVal != null) OtherVal.RandomField();
                if (InheritVal == null) InheritVal = new Test_ToXml();
            }

            /// <summary>
            /// 数据相同验证
            /// </summary>
            /// <param name="tester">测试对象</param>
            /// <param name="listHasTest">已测试对象</param>
            /// <returns>测试结果</returns>
            public bool ValueEqual(Test_ToXmlOther tester, List<Test_ToXmlBase> listHasTest)
            {
                if (listHasTest.Contains(this)) return true;
                listHasTest.Add(this);
                if (IntVal != tester.IntVal) throw new Exception($"{nameof(IntVal)} is not equal!");
                if (ParentField != tester.ParentField) throw new Exception($"{nameof(ParentField)} is not equal!");
                if (OtherVal != null && !OtherVal.ValueEqual(tester.OtherVal, listHasTest)) throw new Exception($"{nameof(OtherVal)} is not equal!");
                return true;
            }

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

        #region 静态属性

        #endregion 静态属性

        #region 属性

        #endregion 属性

        #region 字段

        #endregion 字段

        #region 事件

        #endregion 事件

        #endregion 属性字段

        #region 构造函数

        #endregion 构造函数

        #region 方法

        #region 通用方法

        /// <summary>
        /// 系统测试
        /// </summary>
        [MenuItem("蛤序列化系统/自测试", false, 1)]
        public static void TestSystem()
        {
            Test_ToXml tester = new Test_ToXml
            {
                Name = "A"
            };
            tester.SerializableVal.OtherVal = new Test_ToXml
            {
                Name = "B"
            };
            tester.SerializableVal.OtherVal.SerializableVal.OtherVal = new Test_ToXml
            {
                Name = "C"
            };
            tester.SerializableVal.OtherVal.SerializableVal.OtherVal.SerializableVal.OtherVal = new Test_ToXml
            {
                Name = "D"
            };
            tester.SerializableVal.OtherVal.SerializableVal.OtherVal.SerializableVal.OtherVal.SerializableVal.OtherVal = tester.SerializableVal.OtherVal;
            tester.RandomField();
            XElement element = FrogSerialization.Serialize(tester);
            element.Save("Log.log");
            object result = null;
            FrogSerialization.Deserialize(element, ref result);
            if (!tester.ValueEqual(result as Test_ToXml, new List<Test_ToXmlBase>()))
            {
                throw new Exception("反序列化生成结果异常");
            }
            Test_ToXml copy = new Test_ToXml();
            result = copy;
            FrogSerialization.Deserialize(element, ref result);
            if (!tester.ValueEqual(result as Test_ToXml, new List<Test_ToXmlBase>()))
            {
                throw new Exception("反序列化修改结果异常");
            }
            EditorUtility.DisplayDialog("消息", "测试完成", "确定");
        }

        #endregion 通用方法

        #region 重写方法

        #endregion 重写方法

        #region 事件方法

        #endregion 事件方法 

        #endregion 方法
    }

    #endregion 公共类

    #endregion 测试器
}
