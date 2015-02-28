using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Classes.Foundation.Extensions
{
    public static class ListExtensions
    {

        private static ListRandomIntegration<T> GetRandomIntegrationIfExist<T>(this List<T> l)
        {
            return ListRandomIntegration<T>.GetRandomIntegrationForListIfExist(l);
        }

        private static ListRandomIntegration<T> CreateRandomIntegrationIfNeeded<T>(this List<T> l)
        {
            return ListRandomIntegration<T>.CreateRandomIntegrationForList(l);
        }

        public static T Random<T>(this List<T> l)
        {

            if (l == null || !l.Any()) return default(T);

            var ri = GetRandomIntegrationIfExist(l) ?? CreateRandomIntegrationIfNeeded(l);
            return ri.GetRandomItem();
        }
    }

    //TODO: fuck with AOT
    internal class ListRandomIntegration<T>
    {


        private static readonly List<object> integrations = new List<object>();


        public static ListRandomIntegration<T> GetRandomIntegrationForListIfExist(List<T> list)
        {
            var i = integrations.FirstOrDefault(o => (o as ListRandomIntegration<T>).List.Equals(list));
            if (i != null) return i as ListRandomIntegration<T>;
            return null;
        }
        public static ListRandomIntegration<T> CreateRandomIntegrationForList(List<T> list)
        {
            var i = new ListRandomIntegration<T>(list);
            integrations.Add(i);
            return i;
        }

        private ListRandomIntegration(List<T> list)
        {
            List = list;
            UsedItems = new List<T>();
            FreeIndexes = new List<int>();
            Seed();
        }



        public List<T> List { get; private set; }

        private List<T> UsedItems { get; set; }
        private List<int> FreeIndexes { get; set; } 

        public bool IsItemIsAlreadyUsed(T item)
        {
            return UsedItems.Contains(item);
        }

        public void NotifyListChanged()
        {
            Seed();
        }
        public void Seed()
        {
            UsedItems.Clear();
            FreeIndexes.Clear();

            for (var i = 0; i < List.Count; i++)
            {
                FreeIndexes.Add(i);
            }
        }

        private T UseItem(int itemIndex)
        {


            FreeIndexes.Remove(itemIndex);
            var i = List[itemIndex];
            UsedItems.Add(i);
            return i;
        }

        public T GetRandomItem()
        {

            if (!List.Any()) return default(T);
            if(!FreeIndexes.Any()) Seed();

            var randomIndex = UnityEngine.Random.Range(0, FreeIndexes.Count);
            var item = UseItem(randomIndex);

            return item;

        }

    }

}
