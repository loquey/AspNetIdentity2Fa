using Castle.Components.DictionaryAdapter;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityMvc.CoreServices.Validators
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public sealed class NigeriaPhoneAttribute : ValidationAttribute
    {
        private readonly IEnumerable<string> _NetworkCodes;
        private const string _IDDNigeria = "+234";

        public NigeriaPhoneAttribute(params string[] NetworkCodes)
        {
            _NetworkCodes = NetworkCodes;
        }

        public override bool IsValid(object value)
        {
            string phoneNumber = value as string;

            if (string.IsNullOrEmpty(phoneNumber)) return false;

            foreach (string networkCode in _NetworkCodes)
            {
                if (phoneNumber.StartsWith($"{_IDDNigeria}{networkCode}")) return true;
            }

            return false;
        }
    }
}
