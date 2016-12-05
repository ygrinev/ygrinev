using System;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace FCT.EPS.FileSerializer.Common
{
    public class EFValidator
    {
        public static bool TryValidateValue<T>(T item, string propName, object value, ICollection<ValidationResult> errors) where T : class, new() // throws ArgumentNullException and System.ComponentModel.DataAnnotations.ValidationException!!
        {
            return Attribute.IsDefined(typeof(T).GetProperty(propName), typeof(RegularExpressionAttribute)) 
                ?  Validator.TryValidateValue(value, new ValidationContext(item, null, null) { MemberName = propName }, errors, new List<RegularExpressionAttribute> {  GetAttribute<RegularExpressionAttribute, T>(propName)})
                : true;
        }
        public static A GetAttribute<A, T>(string prop) where A : Attribute where T : class, new()
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
        public static void ParseNValidate<T>(string s, T o, bool isFixedLen = true) where T : class, new()
        {
            if (string.IsNullOrEmpty(s))
                throw new ArgumentNullException("ParseNValidate(string s, " + typeof(T).Name + " o): String parameter cannot be null or empty!");
            if (string.IsNullOrEmpty(s))
                throw new ArgumentNullException("ParseNValidate(string s, " + typeof(T).Name + " o): Parameter of '" + typeof(T).Name + "' type cannot be null!");
            int curPos = 0;
            // rough validation
            List<PropertyInfo> piList = typeof(T).GetProperties().AsEnumerable().Where(el => Attribute.IsDefined(el, typeof(StringLengthAttribute)))
                .OrderBy(el => GetAttribute<ColumnAttribute, T>(el.Name).Order).ToList();
            int len = piList.Select(pi => GetAttribute<StringLengthAttribute, T>(pi.Name).MaximumLength).Sum();
            if(isFixedLen ? s.Length != len : s.Length < len)
            {
                throw new DataMisalignedException("ParseNValidate(string s, " + typeof(T).Name + " o): String parameter has invalid length [" + s.Length + "], must be [" + len + "]" + (isFixedLen ? "" : " or longer") + "!");
            }
            // detailed validation, field by field
            Type type = o.GetType();
            List<ValidationResult> errors = new List<ValidationResult>();
            piList.ForEach(pr =>
            {
                int curLen = GetAttribute<StringLengthAttribute, T>(pr.Name).MaximumLength;
                string value = s.Substring(curPos, curLen);
                curPos += curLen;
                if (TryValidateValue(o, pr.Name, value, errors))
                   typeof(T).GetProperty(pr.Name).SetValue(o, value);
            });
            if(errors.Count() > 0)
            {
                throw new ValidationException(string.Join("", errors.Select(er => er.ErrorMessage).ToArray()));
            }
        }
    }
}
