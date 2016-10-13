using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets {
    public static class ListExtensionMethods {
        public static bool ContainsSameItems<T>(this List<T> list, List<T> other)
        {
            var deleted_items = list.Except(other).Any();
            var new_items = other.Except(list).Any();

            return !new_items && !deleted_items;
        }
    }
}
