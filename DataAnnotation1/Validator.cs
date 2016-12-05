using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataAnnotation1
{
    public static class Validator
    {
        public static void ParseNValidate(IValidateable o, List<ValidationResult> errors)
        {
            //List<ValidationAttribute> atts = new List<ValidationAttribute>();
            //atts.Add(new RegularExpressionAttribute())

            o.GetType().GetProperties().Where(el => Attribute.IsDefined(el, typeof(StringLengthAttribute)))
                .ToList().ForEach(pr =>
            {
                TryValidateValue(o, pr.Name, o.GetType().GetProperty(pr.Name).GetValue(o, null), errors);
            });
        }
        public static A GetAttribute<A>(IValidateable o, string prop) where A : Attribute
        {
         return o.GetType().GetProperty(prop).GetCustomAttributes(typeof(A), false).Cast<A>().Single();        
        }

        public static bool TryValidateValue(IValidateable o, string propName, object value, ICollection<ValidationResult> errors) 
        // throws ArgumentNullException and System.ComponentModel.DataAnnotations.ValidationException!!
        {
            bool isDefined = Attribute.IsDefined(o.GetType().GetProperty(propName), typeof(RegularExpressionAttribute));
            ValidationContext vc = new ValidationContext(o, null, null);
            var attrList = new List<ValidationAttribute>();
            ValidationAttribute att = GetAttribute<RegularExpressionAttribute>(o, propName);
            if (att != null) { attrList.Add(att); };
            //if (( att = GetAttribute<RequiredAttribute>(o, propName)) != null) attrList.Add(att);
            if (( att = GetAttribute<StringLengthAttribute>(o, propName)) != null) attrList.Add(att);

            return isDefined
             ? System.ComponentModel.DataAnnotations.Validator.TryValidateValue(value, vc, errors, attrList)
             : true;
        }
        public static void Init(IValidateable o, TabPage p)
        {
            foreach (PropertyInfo pr in o.GetType().GetProperties())
            {
                PropertyInfo pi = o.GetType().GetProperty(pr.Name, typeof(string));
                if(pi != null)
                    pi.SetValue(o, p.Controls[pr.Name].Text);
            }
        }
    }

}
