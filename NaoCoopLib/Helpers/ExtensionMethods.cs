using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using NaoCoopLib.Enums;

namespace NaoCoopLib.Helpers
{
    public static class ExtensionMethods
    {
        public static bool IsNullOrEmpty(this List<string> arr)
        {
            return arr == null || arr.Count == 0;
        }

        public static bool IsNullOrEmpty<T>(this List<T> arr)
        {
            return arr == null || arr.Count == 0;
        }

        public static bool IsNullOrEmpty(this ArrayList arr)
        {
            return arr == null || arr.Count == 0;
        }

        public static string GetText(this NaoCommand value)
        {
            return NaoCommandAttribute.GetText(value);
        }
    }
}
