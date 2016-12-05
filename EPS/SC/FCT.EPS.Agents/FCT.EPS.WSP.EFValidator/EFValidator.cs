using System;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;

namespace FCT.EPS.WSP.EFValidator
{
    public class EFValidator
    {
        static string GetVarName<T>(T item) where T : class
        {
            return typeof(T).GetProperties()[0].Name;
        }
        static string GetFuncName<T>(Func<T> fn)
        {
            return fn.Target.GetType().Module.ResolveField(BitConverter.ToInt32(fn.Method.GetMethodBody().GetILAsByteArray(), 2)).Name;
        }
        static void ValidateAllData<D, S>(D dest, S source) where D : class where S : class // throws exception!!
        {
            PropertyInfo[] srcProps = typeof(S).GetProperties();
            foreach (PropertyInfo piDest in typeof(D).GetProperties())
            {
                PropertyInfo piSrc = srcProps.FirstOrDefault(pi => pi.Name == piDest.Name);
                if(piSrc != null)
                    Validator.ValidateProperty(piSrc.GetValue(source), new ValidationContext(dest, null, null) { MemberName = piDest.Name });
            }
        }
        public static void Validate<T>(T item, string propName, object value) where T : class // throws ArgumentNullException and System.ComponentModel.DataAnnotations.ValidationException!!
        {
            Validator.ValidateProperty(value, new ValidationContext(item, null, null) { MemberName = propName });
        }
        public static A GetAttribute<A, T>(string prop) where A : Attribute
        {
            return typeof(T).GetProperty(prop).GetCustomAttributes(typeof(A), false).Cast<A>().Single();
        }
        /// <summary>
        /// Validates data from string and assigns valid ones to the given object properties
        /// <para>&#160;</para>
        /// <para>Exceptions:</para>
        /// <para>&#160;&#160;&#160;&#160;ArgumentNullException</para>
        /// <para>&#160;&#160;&#160;&#160;ArgumentOutOfRangeException</para>
        /// <para>&#160;&#160;&#160;&#160;System.ComponentModel.DataAnnotations.ValidationException</para>
        /// <para>&#160;&#160;&#160;&#160;DataMisalignedException</para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="s"></param>
        /// <param name="o"></param>
        /// <param name="isFixedLen"></param>
        public static void ParseNValidate<T>(string s, T o, bool isFixedLen = true) where T : class
        {
            if (string.IsNullOrEmpty(s))
                throw new ArgumentNullException("ParseNValidate(string s, " + typeof(T).Name + " o): String parameter cannot be null or empty!");
            if (string.IsNullOrEmpty(s))
                throw new ArgumentNullException("ParseNValidate(string s, " + typeof(T).Name + " o): Parameter of '" + typeof(T).Name + "' type cannot be null!");
            int curPos = 0;
            // rough validation
            PropertyInfo[] piArr = typeof(T).GetProperties();
            int len = piArr.Select(pi => GetAttribute<StringLengthAttribute, T>(pi.Name).MaximumLength).Sum();
            if(isFixedLen ? s.Length != len : s.Length < len)
            {
                throw new DataMisalignedException("ParseNValidate(string s, " + typeof(T).Name + " o): String parameter has invalid length [" + s.Length + "], must be [" + len + "]" + (isFixedLen ? "" : " or longer") + "!");
            }
            // detailed validation, field by field
            Type type = o.GetType();
            StringBuilder errors = new StringBuilder();
            Array.ForEach(piArr, pr =>
            {
                int curLen = GetAttribute<StringLengthAttribute, T>(pr.Name).MaximumLength;
                string value = s.Substring(curPos, curLen);
                curPos += curLen;
                try
                {
                    Validate(o, pr.Name, value);
                    type.GetProperty(pr.Name).SetValue(o, value);
                }
                catch(Exception ex)
                {
                    errors.Append(ex.Message + "'\r\n");
                }
            });
            if(errors.Length > 0)
            {
                throw new System.ComponentModel.DataAnnotations.ValidationException(errors.ToString());
            }
        }
    }
}
