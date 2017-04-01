using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;

namespace PresetPedalForms
{
    public static class Extensions
    {
        public static ObservableCollection<T> ToObservableCollection<T>(this List<T> list)
        {
            var oc = new ObservableCollection<T>();

            if (list != null)
            {
                foreach (var item in list)
                    oc.Add(item);
            }

            return oc;
        }

        public static void Resize<T>(this List<T> list, int size, T defaultValue)
        {
            int cur = list.Count;
            if(size < cur)
                list.RemoveRange(size, cur - size);
            else if(size > cur)
            {
                if(size > list.Capacity)//this bit is purely an optimisation, to avoid multiple automatic capacity changes.
                    list.Capacity = size;
                list.AddRange(Enumerable.Repeat(defaultValue, size - cur));
            }
        }
        public static void Resize<T>(this List<T> list, int sz) where T : new()
        {
            Resize(list, sz, new T());
        }

        public static IList<TypeInfo> GetInstances<T>()
        {
            var assembly = typeof(App).GetTypeInfo().Assembly;
            //var retItems = (from t in assembly.DefinedTypes
            //                where (t.BaseType == typeof(T))
            //                select t.GetType()).ToList();
            var retItems = assembly.DefinedTypes.Where(d => d.IsSubclassOf(typeof(T))).ToList();

            return retItems;
        }

        //public static TypeInfo GetInstance<T>()
        //{
        //    var assembly = typeof(App).GetTypeInfo().Assembly;
        //    //var retItems = (from t in assembly.DefinedTypes
        //    //                where (t.BaseType == typeof(T))
        //    //                select t.GetType()).ToList();
        //    var retItem = assembly.DefinedTypes.Single(d => d.IsSubclassOf(typeof(T)));

        //    return retItem;
        //}
    }
}
