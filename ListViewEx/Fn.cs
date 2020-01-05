using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListViewEx
{
    internal static class Fn
    {
        /// <summary>
        /// 转Int,失败返回pReturn
        /// </summary>
        /// <param name="e"></param>
        /// <param name="pReturn">失败返回的值</param>
        /// <returns></returns>
        public static int ToInt(this string t, int pReturn)
        {
            int n;
            if (!int.TryParse(t, out n))
                return pReturn;
            return n;
        }

        /// <summary>
        /// 转Double,失败返回pReturn
        /// </summary>
        /// <param name="e"></param>
        /// <param name="pReturn">失败返回的值</param>
        /// <returns></returns>
        public static double ToDouble(this string t, double pReturn)
        {
            double n;
            if (!double.TryParse(t, out n))
                return pReturn;
            return n;
        }
        /// <summary>
        /// 转DateTime,失败返回pReturn
        /// </summary>
        /// <param name="e"></param>
        /// <param name="pReturn">失败返回的值</param>
        /// <returns></returns>
        public static DateTime ToDateTime(this string t, DateTime pReturn)
        {
            DateTime n;
            if (!DateTime.TryParse(t, out n))
                return pReturn;
            return n;
        }

    }
}
