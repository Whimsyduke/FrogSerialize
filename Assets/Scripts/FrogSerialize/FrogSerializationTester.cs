using FrogSerialization;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

namespace FrogSerialization
{
    public class FrogSerializationTester : MonoBehaviour
    {
        /// <summary>
        /// 属性测试类（基础）
        /// </summary>
        public class Test_PropertyBase
        {
            #region 测试变量

            #endregion 测试变量

            #region 基本属性

            #region Bool依赖项

            /// <summary>
            /// bool默认常量值
            /// </summary>
            public const bool BoolValDefault = true;

            /// <summary>
            /// bool字段
            /// </summary>
            public bool BoolVal = BoolValDefault;

            #endregion Bool依赖项

            #region Byte依赖项

            /// <summary>
            /// Byte默认常量值
            /// </summary>
            public const byte ByteValDefault = 127;

            /// <summary>
            /// Byte字段
            /// </summary>
            public byte ByteVal = ByteValDefault;

            #endregion Byte依赖项

            #region Char依赖项

            /// <summary>
            /// Char默认常量值
            /// </summary>
            public const char CharValDefault = ' ';
            /// <summary>
            /// Char默认修改值
            /// </summary>
            public const char CharValCoerce = 'A';

            /// <summary>
            /// Char依赖项
            /// </summary>
            public static readonly Class_GdsProperty CharValProperty = Class_GdsProperty.Register(nameof(CharVal), null, typeof(Struct_GdsChar), typeof(Test_PropertyBase), new Class_GdsPropertyMetadata((Struct_GdsChar)CharValDefault, (Class_GdsObject sender, Class_GdsPropertyArgs arg) =>
            {
                MetadataBeforeChangedCallback = true;
            }, (Class_GdsObject sender, Class_GdsPropertyArgs arg) =>
            {
                MetadataAfterChangedCallback = true;
            }, (Class_GdsObject sender, IInterface_GdsValue val) =>
            {
                MetadataCoerceCallback = true;
                return (Struct_GdsChar)CharValCoerce;
            }), (IInterface_GdsValue val) =>
            {
                PropertyValidateCallback = true;
                return true;
            });

            /// <summary>
            /// Char字段
            /// </summary>
            public Struct_GdsChar CharVal { set => SetValue(CharValProperty, value); get => (Struct_GdsChar)GetValue(CharValProperty); }

            #endregion Char依赖项

            #region Decimal依赖项

            /// <summary>
            /// Decimal默认常量值
            /// </summary>
            public const decimal DecimalValDefault = 1.23456789m;

            /// <summary>
            /// Decimal依赖项
            /// </summary>
            public static readonly Class_GdsProperty DecimalValProperty = Class_GdsProperty.Register(nameof(DecimalVal), null, typeof(Struct_GdsDecimal), typeof(Test_PropertyBase), new Class_GdsPropertyMetadata((Struct_GdsDecimal)DecimalValDefault, (Class_GdsObject sender, Class_GdsPropertyArgs arg) =>
            {
                MetadataBeforeChangedCallback = true;
            }, (Class_GdsObject sender, Class_GdsPropertyArgs arg) =>
            {
                MetadataAfterChangedCallback = true;
            }, (Class_GdsObject sender, IInterface_GdsValue val) =>
            {
                MetadataCoerceCallback = true;
                return -(Struct_GdsDecimal)val;
            }), (IInterface_GdsValue val) =>
            {
                PropertyValidateCallback = true;
                return true;
            });

            /// <summary>
            /// Decimal字段
            /// </summary>
            public Struct_GdsDecimal DecimalVal { set => SetValue(DecimalValProperty, value); get => (Struct_GdsDecimal)GetValue(DecimalValProperty); }

            #endregion Decimal依赖项

            #region Double依赖项

            /// <summary>
            /// Double默认常量值
            /// </summary>
            public const double DoubleValDefault = 1.23456789;

            /// <summary>
            /// Double依赖项
            /// </summary>
            public static readonly Class_GdsProperty DoubleValProperty = Class_GdsProperty.Register(nameof(DoubleVal), null, typeof(Struct_GdsDouble), typeof(Test_PropertyBase), new Class_GdsPropertyMetadata((Struct_GdsDouble)DoubleValDefault, (Class_GdsObject sender, Class_GdsPropertyArgs arg) =>
            {
                MetadataBeforeChangedCallback = true;
            }, (Class_GdsObject sender, Class_GdsPropertyArgs arg) =>
            {
                MetadataAfterChangedCallback = true;
            }, (Class_GdsObject sender, IInterface_GdsValue val) =>
            {
                MetadataCoerceCallback = true;
                return -(Struct_GdsDouble)val;
            }), (IInterface_GdsValue val) =>
            {
                PropertyValidateCallback = true;
                return true;
            });

            /// <summary>
            /// Double字段
            /// </summary>
            public Struct_GdsDouble DoubleVal { set => SetValue(DoubleValProperty, value); get => (Struct_GdsDouble)GetValue(DoubleValProperty); }

            #endregion Double依赖项

            #region Enum依赖项

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
            /// Enum默认修改值
            /// </summary>
            public const EnumTest EnumValCoerce = EnumTest.D;

            /// <summary>
            /// Enum依赖项
            /// </summary>
            public static readonly Class_GdsProperty EnumValProperty = Class_GdsProperty.Register(nameof(EnumVal), null, typeof(Struct_GdsEnum<EnumTest>), typeof(Test_PropertyBase), new Class_GdsPropertyMetadata((Struct_GdsEnum<EnumTest>)EnumValDefault, (Class_GdsObject sender, Class_GdsPropertyArgs arg) =>
            {
                MetadataBeforeChangedCallback = true;
            }, (Class_GdsObject sender, Class_GdsPropertyArgs arg) =>
            {
                MetadataAfterChangedCallback = true;
            }, (Class_GdsObject sender, IInterface_GdsValue val) =>
            {
                MetadataCoerceCallback = true;
                return (Struct_GdsEnum<EnumTest>)EnumValCoerce;
            }), (IInterface_GdsValue val) =>
            {
                PropertyValidateCallback = true;
                return true;
            });

            /// <summary>
            /// Enum字段
            /// </summary>
            public Struct_GdsEnum<EnumTest> EnumVal { set => SetValue(EnumValProperty, value); get => (Struct_GdsEnum<EnumTest>)GetValue(EnumValProperty); }

            #endregion Enum依赖项

            #region Float依赖项

            /// <summary>
            /// Enum默认常量值
            /// </summary>
            public const float FloatValDefault = 1.23456789F;

            /// <summary>
            /// Float依赖项
            /// </summary>
            public static readonly Class_GdsProperty FloatValProperty = Class_GdsProperty.Register(nameof(FloatVal), null, typeof(Struct_GdsFloat), typeof(Test_PropertyBase), new Class_GdsPropertyMetadata((Struct_GdsFloat)FloatValDefault, (Class_GdsObject sender, Class_GdsPropertyArgs arg) =>
            {
                MetadataBeforeChangedCallback = true;
            }, (Class_GdsObject sender, Class_GdsPropertyArgs arg) =>
            {
                MetadataAfterChangedCallback = true;
            }, (Class_GdsObject sender, IInterface_GdsValue val) =>
            {
                MetadataCoerceCallback = true;
                return -(Struct_GdsFloat)val;
            }), (IInterface_GdsValue val) =>
            {
                PropertyValidateCallback = true;
                return true;
            });

            /// <summary>
            /// Float字段
            /// </summary>
            public Struct_GdsFloat FloatVal { set => SetValue(FloatValProperty, value); get => (Struct_GdsFloat)GetValue(FloatValProperty); }

            #endregion Float依赖项

            #region Int依赖项

            /// <summary>
            /// Int默认常量值
            /// </summary>
            public const int IntValDefault = 1024;

            /// <summary>
            /// Int依赖项
            /// </summary>
            public static readonly Class_GdsProperty IntValProperty = Class_GdsProperty.Register(nameof(IntVal), null, typeof(Struct_GdsInt), typeof(Test_PropertyBase), new Class_GdsPropertyMetadata((Struct_GdsInt)IntValDefault, (Class_GdsObject sender, Class_GdsPropertyArgs arg) =>
            {
                MetadataBeforeChangedCallback = true;
            }, (Class_GdsObject sender, Class_GdsPropertyArgs arg) =>
            {
                MetadataAfterChangedCallback = true;
            }, (Class_GdsObject sender, IInterface_GdsValue val) =>
            {
                MetadataCoerceCallback = true;
                return -(Struct_GdsInt)val;
            }), (IInterface_GdsValue val) =>
            {
                PropertyValidateCallback = true;
                return true;
            });

            /// <summary>
            /// Int字段
            /// </summary>
            public Struct_GdsInt IntVal { set => SetValue(IntValProperty, value); get => (Struct_GdsInt)GetValue(IntValProperty); }

            #endregion Int依赖项

            #region Long依赖项

            /// <summary>
            /// Long默认常量值
            /// </summary>
            public const long LongValDefault = 1024;

            /// <summary>
            /// Long依赖项
            /// </summary>
            public static readonly Class_GdsProperty LongValProperty = Class_GdsProperty.Register(nameof(LongVal), null, typeof(Struct_GdsLong), typeof(Test_PropertyBase), new Class_GdsPropertyMetadata((Struct_GdsLong)LongValDefault, (Class_GdsObject sender, Class_GdsPropertyArgs arg) =>
            {
                MetadataBeforeChangedCallback = true;
            }, (Class_GdsObject sender, Class_GdsPropertyArgs arg) =>
            {
                MetadataAfterChangedCallback = true;
            }, (Class_GdsObject sender, IInterface_GdsValue val) =>
            {
                MetadataCoerceCallback = true;
                return -(Struct_GdsLong)val;
            }), (IInterface_GdsValue val) =>
            {
                PropertyValidateCallback = true;
                return true;
            });

            /// <summary>
            /// Long字段
            /// </summary>
            public Struct_GdsLong LongVal { set => SetValue(LongValProperty, value); get => (Struct_GdsLong)GetValue(LongValProperty); }

            #endregion Long依赖项

            #region SByte依赖项

            /// <summary>
            /// SByte默认常量值
            /// </summary>
            public const sbyte SByteValDefault = -127;

            /// <summary>
            /// SByte依赖项
            /// </summary>
            public static readonly Class_GdsProperty SByteValProperty = Class_GdsProperty.Register(nameof(SByteVal), null, typeof(Struct_GdsSByte), typeof(Test_PropertyBase), new Class_GdsPropertyMetadata((Struct_GdsSByte)SByteValDefault, (Class_GdsObject sender, Class_GdsPropertyArgs arg) =>
            {
                MetadataBeforeChangedCallback = true;
            }, (Class_GdsObject sender, Class_GdsPropertyArgs arg) =>
            {
                MetadataAfterChangedCallback = true;
            }, (Class_GdsObject sender, IInterface_GdsValue val) =>
            {
                MetadataCoerceCallback = true;
                sbyte tempVal = (Struct_GdsSByte)val;
                return new Struct_GdsSByte((sbyte)-tempVal);
            }), (IInterface_GdsValue val) =>
            {
                PropertyValidateCallback = true;
                return true;
            });

            /// <summary>
            /// SByte字段
            /// </summary>
            public Struct_GdsSByte SByteVal { set => SetValue(SByteValProperty, value); get => (Struct_GdsSByte)GetValue(SByteValProperty); }

            #endregion SByte依赖项

            #region Short依赖项

            /// <summary>
            /// Short默认常量值
            /// </summary>
            public const short ShortValDefault = 1024;

            /// <summary>
            /// Short依赖项
            /// </summary>
            public static readonly Class_GdsProperty ShortValProperty = Class_GdsProperty.Register(nameof(ShortVal), null, typeof(Struct_GdsShort), typeof(Test_PropertyBase), new Class_GdsPropertyMetadata((Struct_GdsShort)ShortValDefault, (Class_GdsObject sender, Class_GdsPropertyArgs arg) =>
            {
                MetadataBeforeChangedCallback = true;
            }, (Class_GdsObject sender, Class_GdsPropertyArgs arg) =>
            {
                MetadataAfterChangedCallback = true;
            }, (Class_GdsObject sender, IInterface_GdsValue val) =>
            {
                MetadataCoerceCallback = true;
                short tempVal = (Struct_GdsShort)val;
                return new Struct_GdsShort((short)-tempVal);
            }), (IInterface_GdsValue val) =>
            {
                PropertyValidateCallback = true;
                return true;
            });

            /// <summary>
            /// Short字段
            /// </summary>
            public Struct_GdsShort ShortVal { set => SetValue(ShortValProperty, value); get => (Struct_GdsShort)GetValue(ShortValProperty); }

            #endregion Short依赖项

            #region String依赖项

            /// <summary>
            /// String默认常量值
            /// </summary>
            public const string StringValDefault = "TEST_DEFAULT";
            /// <summary>
            /// String默认修改值
            /// </summary>
            public const string StringValCoerce = "TEST_COERCE";

            /// <summary>
            /// String依赖项
            /// </summary>
            public static readonly Class_GdsProperty StringValProperty = Class_GdsProperty.Register(nameof(StringVal), null, typeof(Struct_GdsString), typeof(Test_PropertyBase), new Class_GdsPropertyMetadata((Struct_GdsString)StringValDefault, (Class_GdsObject sender, Class_GdsPropertyArgs arg) =>
            {
                MetadataBeforeChangedCallback = true;
            }, (Class_GdsObject sender, Class_GdsPropertyArgs arg) =>
            {
                MetadataAfterChangedCallback = true;
            }, (Class_GdsObject sender, IInterface_GdsValue val) =>
            {
                MetadataCoerceCallback = true;
                return (Struct_GdsString)StringValCoerce;
            }), (IInterface_GdsValue val) =>
            {
                PropertyValidateCallback = true;
                return true;
            });

            /// <summary>
            /// String字段
            /// </summary>
            public Struct_GdsString StringVal { set => SetValue(StringValProperty, value); get => (Struct_GdsString)GetValue(StringValProperty); }

            #endregion String依赖项

            #region UInt依赖项

            /// <summary>
            /// UInt默认常量值
            /// </summary>
            public const uint UIntValDefault = 1024;

            /// <summary>
            /// UInt依赖项
            /// </summary>
            public static readonly Class_GdsProperty UIntValProperty = Class_GdsProperty.Register(nameof(UIntVal), null, typeof(Struct_GdsUInt), typeof(Test_PropertyBase), new Class_GdsPropertyMetadata((Struct_GdsUInt)UIntValDefault, (Class_GdsObject sender, Class_GdsPropertyArgs arg) =>
            {
                MetadataBeforeChangedCallback = true;
            }, (Class_GdsObject sender, Class_GdsPropertyArgs arg) =>
            {
                MetadataAfterChangedCallback = true;
            }, (Class_GdsObject sender, IInterface_GdsValue val) =>
            {
                MetadataCoerceCallback = true;
                return (Struct_GdsUInt)val - 1;
            }), (IInterface_GdsValue val) =>
            {
                PropertyValidateCallback = true;
                return true;
            });

            /// <summary>
            /// UInt字段
            /// </summary>
            public Struct_GdsUInt UIntVal { set => SetValue(UIntValProperty, value); get => (Struct_GdsUInt)GetValue(UIntValProperty); }

            #endregion UInt依赖项

            #region ULong依赖项

            /// <summary>
            /// ULong默认常量值
            /// </summary>
            public const ulong ULongValDefault = 1024;

            /// <summary>
            /// ULong依赖项
            /// </summary>
            public static readonly Class_GdsProperty ULongValProperty = Class_GdsProperty.Register(nameof(ULongVal), null, typeof(Struct_GdsULong), typeof(Test_PropertyBase), new Class_GdsPropertyMetadata((Struct_GdsULong)ULongValDefault, (Class_GdsObject sender, Class_GdsPropertyArgs arg) =>
            {
                MetadataBeforeChangedCallback = true;
            }, (Class_GdsObject sender, Class_GdsPropertyArgs arg) =>
            {
                MetadataAfterChangedCallback = true;
            }, (Class_GdsObject sender, IInterface_GdsValue val) =>
            {
                MetadataCoerceCallback = true;
                return (Struct_GdsULong)val - 1;
            }), (IInterface_GdsValue val) =>
            {
                PropertyValidateCallback = true;
                return true;
            });

            /// <summary>
            /// ULong字段
            /// </summary>
            public Struct_GdsULong ULongVal { set => SetValue(ULongValProperty, value); get => (Struct_GdsULong)GetValue(ULongValProperty); }

            #endregion Long依赖项

            #region UShort依赖项

            /// <summary>
            /// UShort默认常量值
            /// </summary>
            public const ushort UShortValDefault = 1024;

            /// <summary>
            /// UShort依赖项
            /// </summary>
            public static readonly Class_GdsProperty UShortValProperty = Class_GdsProperty.Register(nameof(UShortVal), null, typeof(Struct_GdsUShort), typeof(Test_PropertyBase), new Class_GdsPropertyMetadata((Struct_GdsUShort)UShortValDefault, (Class_GdsObject sender, Class_GdsPropertyArgs arg) =>
            {
                MetadataBeforeChangedCallback = true;
            }, (Class_GdsObject sender, Class_GdsPropertyArgs arg) =>
            {
                MetadataAfterChangedCallback = true;
            }, (Class_GdsObject sender, IInterface_GdsValue val) =>
            {
                MetadataCoerceCallback = true;
                return (Struct_GdsUShort)val - 1;
            }), (IInterface_GdsValue val) =>
            {
                PropertyValidateCallback = true;
                return true;
            });

            /// <summary>
            /// UShort字段
            /// </summary>
            public Struct_GdsUShort UShortVal { set => SetValue(UShortValProperty, value); get => (Struct_GdsUShort)GetValue(UShortValProperty); }

            #endregion UShort依赖项

            #region Unset依赖项

            /// <summary>
            /// Unset依赖项
            /// </summary>
            public static readonly Class_GdsProperty UnsetValProperty = Class_GdsProperty.Register(nameof(UnsetVal), null, typeof(Struct_GdsInt), typeof(Test_PropertyBase));

            /// <summary>
            /// Unset字段
            /// </summary>
            public Struct_GdsInt UnsetVal { set => SetValue(UnsetValProperty, value); get => (Struct_GdsInt)GetValue(UnsetValProperty); }

            #endregion Unset依赖项

            #endregion 基本属性

            #region 只读属性

            /// <summary>
            /// 只读默认常量值
            /// </summary>
            public const bool ReadonlyValDefault = true;

            /// <summary>
            /// 只读依赖项
            /// </summary>
            public static readonly Class_GdsProperty ReadonlyValProperty = Class_GdsProperty.RegisterReadOnly(nameof(ReadonlyVal), null, typeof(Struct_GdsBool), typeof(Test_PropertyBase), new Class_GdsPropertyMetadata((Struct_GdsBool)BoolValDefault, (Class_GdsObject sender, Class_GdsPropertyArgs arg) =>
            {
                MetadataBeforeChangedCallback = true;
            }, (Class_GdsObject sender, Class_GdsPropertyArgs arg) =>
            {
                MetadataAfterChangedCallback = true;
            }, (Class_GdsObject sender, IInterface_GdsValue val) =>
            {
                MetadataCoerceCallback = true;
                return !(Struct_GdsBool)val;
            }), (IInterface_GdsValue val) =>
            {
                PropertyValidateCallback = true;
                return true;
            });

            /// <summary>
            /// 只读字段
            /// </summary>
            public Struct_GdsBool ReadonlyVal { set => SetValue(ReadonlyValProperty, value); get => (Struct_GdsBool)GetValue(ReadonlyValProperty); }

            #endregion 只读属性

            #region 附加属性

            /// <summary>
            /// 附加默认常量值
            /// </summary>
            public const bool AttachedValDefault = true;

            /// <summary>
            /// 附加依赖项
            /// </summary>
            public static readonly Class_GdsProperty AttachedValProperty = Class_GdsProperty.RegisterAttached(nameof(AttachedVal), null, typeof(Struct_GdsBool), typeof(Test_PropertyBase), new Class_GdsPropertyMetadata((Struct_GdsBool)BoolValDefault, (Class_GdsObject sender, Class_GdsPropertyArgs arg) =>
            {
                MetadataBeforeChangedCallback = true;
            }, (Class_GdsObject sender, Class_GdsPropertyArgs arg) =>
            {
                MetadataAfterChangedCallback = true;
            }, (Class_GdsObject sender, IInterface_GdsValue val) =>
            {
                MetadataCoerceCallback = true;
                return !(Struct_GdsBool)val;
            }), (IInterface_GdsValue val) =>
            {
                PropertyValidateCallback = true;
                return true;
            });

            /// <summary>
            /// 附加字段
            /// </summary>
            public Struct_GdsBool AttachedVal { set => SetValue(AttachedValProperty, value); get => (Struct_GdsBool)GetValue(AttachedValProperty); }

            #endregion 附加属性
        }

        // Start is called before the first frame update
        void Start()
        {
            XElement element;

            #region 测试不可序列化对象
    
            #endregion 测试不可序列化对象
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}