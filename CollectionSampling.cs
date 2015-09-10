using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Collections.Generic.CollectionSampling
{
    public static class CollectionSampling
    {
        /// <summary>
        /// 以序列元素之值作為權重進行隨機抽樣。
        /// (Do the random sampling, in elements of sequence as weight.)
        /// </summary>
        /// <typeparam name="TSource">source之項目的型別。(Source type.)</typeparam>
        /// <param name="rnd">已建立之隨機物件。(Constructed random object.)</param>
        /// <returns>抽樣結果之索引值。(Index of sampling result.)</returns>
        public static int SamplingIndex<TSource>(this IEnumerable<TSource> source, Random rnd)
        {
            int i = 0;
            switch (NumberType(typeof(TSource)))
            {
                case NumberTypes.Integer:
                    ulong itotal = 0;
                    foreach (TSource o in source)
                    {
                        itotal += (ulong)Convert.ChangeType(o, TypeCode.UInt64);
                    }
                    ulong ir = Random(0, itotal, rnd);
                    while (ir >= (ulong)Convert.ChangeType(source.ElementAt<TSource>(i), TypeCode.UInt64))
                    {
                        ir -= (ulong)Convert.ChangeType(source.ElementAt<TSource>(i), TypeCode.UInt64);
                        i++;
                    }
                    return i;
                case NumberTypes.Float:
                    decimal dtotal = 0;
                    foreach (TSource o in source)
                    {
                        dtotal += (decimal)Convert.ChangeType(o, TypeCode.Decimal);
                    }
                    decimal dr = ((decimal)rnd.NextDouble()) * dtotal;
                    while (dr >= (decimal)Convert.ChangeType(source.ElementAt<TSource>(i), TypeCode.Decimal))
                    {
                        dr -= (decimal)Convert.ChangeType(source.ElementAt<TSource>(i), TypeCode.Decimal);
                        i++;
                    }
                    return i;
                case NumberTypes.String:
                    if (IsNumber<TSource>(source))
                    {
                        decimal stotal = 0;
                        foreach (TSource o in source)
                        {
                            stotal += (decimal)Convert.ChangeType(o, TypeCode.Decimal);
                        }
                        decimal sr = ((decimal)rnd.NextDouble()) * stotal;
                        while (sr >= (decimal)Convert.ChangeType(source.ElementAt<TSource>(i), TypeCode.Decimal))
                        {
                            sr -= (decimal)Convert.ChangeType(source.ElementAt<TSource>(i), TypeCode.Decimal);
                            i++;
                        }
                        return i;
                    }
                    else
                    {
                        goto default;
                    }
                default:    //NaN or NaN string
                    throw new NotFiniteNumberException("Collection Element is not a Number.");
            }
        }

        /// <summary>
        /// 以序列元素之值作為權重進行隨機抽樣。
        /// (Do the random sampling, in elements of sequence as weight.)
        /// </summary>
        /// <typeparam name="TSource">source之項目的型別。(Source type.)</typeparam>
        /// <returns>抽樣結果之索引值。(Index of sampling result.)</returns>
        public static int SamplingIndex<TSource>(this IEnumerable<TSource> source)
        {
            Random rnd = new Random(Guid.NewGuid().GetHashCode());
            return source.SamplingIndex<TSource>(rnd);
        }

        /// <summary>
        /// 以序列元素之值作為權重進行隨機抽樣。
        /// (Do the random sampling, in elements of sequence as weight.)
        /// </summary>
        /// <typeparam name="TSource">source之項目的型別。(Source type.)</typeparam>
        /// <param name="rnd">已建立之隨機物件。(Constructed random object.)</param>
        /// <returns>抽樣結果之物件。(Object of sampling result.)</returns>
        public static TSource Sampling<TSource>(this IEnumerable<TSource> source, Random rnd)
        {
            return source.ElementAt<TSource>(source.SamplingIndex<TSource>(rnd));
        }

        /// <summary>
        /// 以序列元素之值作為權重進行隨機抽樣。
        /// (Do the random sampling, in elements of sequence as weight.)
        /// </summary>
        /// <typeparam name="TSource">source之項目的型別。(Source type.)</typeparam>
        /// <returns>抽樣結果之物件。(Object of sampling result.)</returns>
        public static TSource Sampling<TSource>(this IEnumerable<TSource> source)
        {
            return source.ElementAt<TSource>(source.SamplingIndex<TSource>());
        }

        /// <summary>
        /// 以指定的序列元素之屬性作為權重進行隨機抽樣。
        /// (Do the random sampling, in assign  property of elements of sequence as weight.)
        /// </summary>
        /// <typeparam name="TSource">source之項目的型別。(Source type.)</typeparam>
        /// <param name="weightPropertyName">作為權重之屬性的名稱。(Name of the property that will be used as weight.)</param>
        /// <param name="rnd">已建立之隨機物件。(Constructed random object.)</param>
        /// <returns>抽樣結果之索引值。(Index of sampling result.)</returns>
        public static int SamplingIndex<TSource>(this IEnumerable<TSource> source, string weightPropertyName, Random rnd)
        {
            int i = 0;
            System.Reflection.PropertyInfo p = typeof(TSource).GetProperty(weightPropertyName);
            switch (NumberType(p.PropertyType))
            {
                case NumberTypes.Integer:
                    ulong itotal = 0;
                    foreach (TSource o in source)
                    {
                        itotal += (ulong)Convert.ChangeType(p.GetValue(o), TypeCode.UInt64);
                    }
                    ulong ir = Random(0, itotal, rnd);
                    while (ir >= (ulong)Convert.ChangeType(p.GetValue(source.ElementAt<TSource>(i)), TypeCode.UInt64))
                    {
                        ir -= (ulong)Convert.ChangeType(p.GetValue(source.ElementAt<TSource>(i)), TypeCode.UInt64);
                        i++;
                    }
                    return i;
                case NumberTypes.Float:
                    decimal dtotal = 0;
                    foreach (TSource o in source)
                    {
                        dtotal += (decimal)Convert.ChangeType(p.GetValue(o), TypeCode.Decimal);
                    }
                    decimal dr = ((decimal)rnd.NextDouble()) * dtotal;
                    while (dr >= (decimal)Convert.ChangeType(p.GetValue(source.ElementAt<TSource>(i)), TypeCode.Decimal))
                    {
                        dr -= (decimal)Convert.ChangeType(p.GetValue(source.ElementAt<TSource>(i)), TypeCode.Decimal);
                        i++;
                    }
                    return i;
                case NumberTypes.String:
                    if (IsNumber<TSource>(source, weightPropertyName))
                    {
                        decimal stotal = 0;
                        foreach (TSource o in source)
                        {
                            stotal += (decimal)Convert.ChangeType(p.GetValue(o), TypeCode.Decimal);
                        }
                        decimal sr = ((decimal)rnd.NextDouble()) * stotal;
                        while (sr >= (decimal)Convert.ChangeType(p.GetValue(source.ElementAt<TSource>(i)), TypeCode.Decimal))
                        {
                            sr -= (decimal)Convert.ChangeType(p.GetValue(source.ElementAt<TSource>(i)), TypeCode.Decimal);
                            i++;
                        }
                        return i;
                    }
                    else
                    {
                        goto default;
                    }
                default:    //NaN or NaN string
                    throw new NotFiniteNumberException("Collection Element is not a Number.");
            }
        }

        /// <summary>
        /// 以指定的序列元素之屬性作為權重進行隨機抽樣。
        /// (Do the random sampling, in assign  property of elements of sequence as weight.)
        /// </summary>
        /// <typeparam name="TSource">source之項目的型別。(Source type.)</typeparam>
        /// <param name="weightPropertyName">作為權重之屬性的名稱。(Name of the property that will be used as weight.)</param>
        /// <returns>抽樣結果之索引值。(Index of sampling result.)</returns>
        public static int SamplingIndex<TSource>(this IEnumerable<TSource> source, string weightPropertyName)
        {
            Random rnd = new Random(Guid.NewGuid().GetHashCode());
            return source.SamplingIndex<TSource>(weightPropertyName, rnd);
        }

        /// <summary>
        /// 以指定的序列元素之屬性作為權重進行隨機抽樣。
        /// (Do the random sampling, in assign  property of elements of sequence as weight.)
        /// </summary>
        /// <typeparam name="TSource">source之項目的型別。(Source type.)</typeparam>
        /// <param name="weightPropertyName">作為權重之屬性的名稱。(Name of the property that will be used as weight.)</param>
        /// <param name="rnd">已建立之隨機物件。(Constructed random object.)</param>
        /// <returns>抽樣結果之物件。(Object of sampling result.)</returns>
        public static TSource Sampling<TSource>(this IEnumerable<TSource> source, string weightPropertyName, Random rnd)
        {
            return source.ElementAt<TSource>(source.SamplingIndex<TSource>(weightPropertyName, rnd));
        }

        /// <summary>
        /// 以指定的序列元素之屬性作為權重進行隨機抽樣。
        /// (Do the random sampling, in assign  property of elements of sequence as weight.)
        /// </summary>
        /// <typeparam name="TSource">source之項目的型別。(Source type.)</typeparam>
        /// <param name="weightPropertyName">作為權重之屬性的名稱。(Name of the property that will be used as weight.)</param>
        /// <returns>抽樣結果之物件。(Object of sampling result.)</returns>
        public static TSource Sampling<TSource>(this IEnumerable<TSource> source, string weightPropertyName)
        {
            return source.ElementAt<TSource>(source.SamplingIndex<TSource>(weightPropertyName));
        }

        /// <summary>
        /// 對字典之值進行隨機抽樣。
        /// (Do the random sampling to dictionary's values.)
        /// </summary>
        /// <typeparam name="TKey">索引的型別。(Key type.)</typeparam>
        /// <typeparam name="TValue">值的型別。(Value type.)</typeparam>
        /// <param name="rnd">已建立之隨機物件。(Constructed random object.)</param>
        /// <param name="weightOnKey">是否使用索引作為權重，預設為否。(Whehter use keys as weight or not, false by default.)</param>
        /// <returns>抽樣結果之值。(Value of sampling result.)</returns>
        public static TValue Sampling<TKey, TValue>(this Dictionary<TKey, TValue> source, Random rnd, bool weightOnKey = false)
        {
            if (weightOnKey)
            {
                return source.Values.ToArray<TValue>()[source.Keys.SamplingIndex<TKey>(rnd)];
            }
            else
            {
                return source.Values.Sampling<TValue>(rnd);
            }
        }

        /// <summary>
        /// 對字典之值進行隨機抽樣。
        /// (Do the random sampling to dictionary's values.)
        /// </summary>
        /// <typeparam name="TKey">索引的型別。(Key type.)</typeparam>
        /// <typeparam name="TValue">值的型別。(Value type.)</typeparam>
        /// <param name="weightOnKey">是否使用索引作為權重，預設為否。(Whehter use keys as weight or not, false by default.)</param>
        /// <returns>抽樣結果之值。(Value of sampling result.)</returns>
        public static TValue Sampling<TKey, TValue>(this Dictionary<TKey, TValue> source, bool weightOnKey = false)
        {
            Random rnd = new Random(Guid.NewGuid().GetHashCode());
            return source.Sampling<TKey, TValue>(rnd, weightOnKey);
        }

        /// <summary>
        /// 對字典之索引進行隨機抽樣。
        /// (Do the random sampling to dictionary's keys.)
        /// </summary>
        /// <typeparam name="TKey">索引的型別。(Key type.)</typeparam>
        /// <typeparam name="TValue">值的型別。(Value type.)</typeparam>
        /// <param name="rnd">已建立之隨機物件。(Constructed random object.)</param>
        /// <param name="weightOnKey">是否使用索引作為權重，預設為否。(Whehter use keys as weight or not, false by default.)</param>
        /// <returns>抽樣結果之索引。(Key of sampling result.)</returns>
        public static TKey SamplingKey<TKey, TValue>(this Dictionary<TKey, TValue> source, Random rnd, bool weightOnKey = false)
        {
            if (weightOnKey)
            {
                return source.Keys.Sampling<TKey>(rnd);
            }
            else
            {
                return source.Keys.ToArray<TKey>()[source.Values.SamplingIndex<TValue>(rnd)];
            }
        }

        /// <summary>
        /// 對字典之索引進行隨機抽樣。
        /// (Do the random sampling to dictionary's keys.)
        /// </summary>
        /// <typeparam name="TKey">索引的型別。(Key type.)</typeparam>
        /// <typeparam name="TValue">值的型別。(Value type.)</typeparam>
        /// <param name="weightOnKey">是否使用索引作為權重，預設為否。(Whehter use keys as weight or not, false by default.)</param>
        /// <returns>抽樣結果之索引。(Key of sampling result.)</returns>
        public static TKey SamplingKey<TKey, TValue>(this Dictionary<TKey, TValue> source, bool weightOnKey = false)
        {
            Random rnd = new Random(Guid.NewGuid().GetHashCode());
            return source.SamplingKey<TKey, TValue>(rnd, weightOnKey);
        }

        /// <summary>
        /// 對字典之值進行隨機抽樣。
        /// (Do the random sampling to dictionary's values.)
        /// </summary>
        /// <typeparam name="TKey">索引的型別。(Key type.)</typeparam>
        /// <typeparam name="TValue">值的型別。(Value type.)</typeparam>
        /// <param name="weightPropertyName">作為權重之屬性的名稱。(Name of the property that will be used as weight.)</param>
        /// <param name="rnd">已建立之隨機物件。(Constructed random object.)</param>
        /// <param name="weightOnKey">是否使用索引作為權重，預設為否。(Whehter use keys as weight or not, false by default.)</param>
        /// <returns>抽樣結果之值。(Value of sampling result.)</returns>
        public static TValue Sampling<TKey, TValue>(this Dictionary<TKey, TValue> source, string weightPropertyName, Random rnd, bool weightOnKey = false)
        {
            if (weightOnKey)
            {
                return source.Values.ToArray<TValue>()[source.Keys.SamplingIndex<TKey>(weightPropertyName, rnd)];
            }
            else
            {
                return source.Values.Sampling<TValue>(weightPropertyName, rnd);
            }
        }

        /// <summary>
        /// 對字典之值進行隨機抽樣。
        /// (Do the random sampling to dictionary's values.)
        /// </summary>
        /// <typeparam name="TKey">索引的型別。(Key type.)</typeparam>
        /// <typeparam name="TValue">值的型別。(Value type.)</typeparam>
        /// <param name="weightPropertyName">作為權重之屬性的名稱。(Name of the property that will be used as weight.)</param>
        /// <param name="weightOnKey">是否使用索引作為權重，預設為否。(Whehter use keys as weight or not, false by default.)</param>
        /// <returns>抽樣結果之值。(Value of sampling result.)</returns>
        public static TValue Sampling<TKey, TValue>(this Dictionary<TKey, TValue> source, string weightPropertyName, bool weightOnKey = false)
        {
            Random rnd = new Random(Guid.NewGuid().GetHashCode());
            return source.Sampling<TKey, TValue>(weightPropertyName, rnd, weightOnKey);
        }

        /// <summary>
        /// 對字典之索引進行隨機抽樣。
        /// (Do the random sampling to dictionary's keys.)
        /// </summary>
        /// <typeparam name="TKey">索引的型別。(Key type.)</typeparam>
        /// <typeparam name="TValue">值的型別。(Value type.)</typeparam>
        /// <param name="weightPropertyName">作為權重之屬性的名稱。(Name of the property that will be used as weight.)</param>
        /// <param name="rnd">已建立之隨機物件。(Constructed random object.)</param>
        /// <param name="weightOnKey">是否使用索引作為權重，預設為否。(Whehter use keys as weight or not, false by default.)</param>
        /// <returns>抽樣結果之索引。(Key of sampling result.)</returns>
        public static TKey SamplingKey<TKey, TValue>(this Dictionary<TKey, TValue> source, string weightPropertyName, Random rnd, bool weightOnKey = false)
        {
            if (weightOnKey)
            {
                return source.Keys.Sampling<TKey>(weightPropertyName, rnd);
            }
            else
            {
                return source.Keys.ToArray<TKey>()[source.Values.SamplingIndex<TValue>(weightPropertyName, rnd)];
            }
        }

        /// <summary>
        /// 對字典之索引進行隨機抽樣。
        /// (Do the random sampling to dictionary's keys.)
        /// </summary>
        /// <typeparam name="TKey">索引的型別。(Key type.)</typeparam>
        /// <typeparam name="TValue">值的型別。(Value type.)</typeparam>
        /// <param name="weightPropertyName">作為權重之屬性的名稱。(Name of the property that will be used as weight.)</param>
        /// <param name="weightOnKey">是否使用索引作為權重，預設為否。(Whehter use keys as weight or not, false by default.)</param>
        /// <returns>抽樣結果之索引。(Key of sampling result.)</returns>
        public static TKey SamplingKey<TKey, TValue>(this Dictionary<TKey, TValue> source, string weightPropertyName, bool weightOnKey = false)
        {
            Random rnd = new Random(Guid.NewGuid().GetHashCode());
            return source.SamplingKey<TKey, TValue>(weightPropertyName, weightOnKey);
        }

        //type determine
        #region Number Types
        private enum NumberTypes { Integer, Float, String, NaN };

        private static NumberTypes NumberType(Type t)
        {
            switch (Type.GetTypeCode(t))
            {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Char:
                    return NumberTypes.Integer;
                case TypeCode.Single:
                case TypeCode.Double:
                case TypeCode.Decimal:
                    return NumberTypes.Float;
                case TypeCode.String:
                    return NumberTypes.String;
                default:
                    return NumberTypes.NaN;
            }
        }

        private static bool IsNumber(string str)
        {
            double d;
            if (double.TryParse(str, out d))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static bool IsNumber<T>(IEnumerable<T> source)
        {
            foreach (T s in source)
            {
                if (!IsNumber((string)Convert.ChangeType(s, TypeCode.String)))
                {
                    return false;
                }
            }
            return true;
        }

        private static bool IsNumber<T>(IEnumerable<T> source, string weightPropertyName)
        {
            System.Reflection.PropertyInfo p = typeof(T).GetProperty(weightPropertyName);
            foreach (T o in source)
            {
                if (!IsNumber((string)p.GetValue(o)))
                {
                    return false;
                }
            }
            return true;
        }
        #endregion

        //ulong random
        #region Extra Random
        private static ulong Random(Random rnd)
        {
            byte[] buffer = new byte[8];
            rnd.NextBytes(buffer);
            return BitConverter.ToUInt64(buffer, 0);
        }

        private static ulong Random(ulong min, ulong max, Random rnd)
        {
            ulong temp = Random(rnd);
            ulong range = max - min;
            temp = temp % range;
            return temp;
        }
        #endregion
    }
}
