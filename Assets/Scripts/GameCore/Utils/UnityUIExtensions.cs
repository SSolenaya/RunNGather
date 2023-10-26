using System;
using TMPro;
 
namespace UniRx.Extensions
{
    public static class UnityUIExtensions
    {
        public static IDisposable SubscribeToText(this IObservable<string> source, TMP_Text text)
        {
            return source.SubscribeWithState(text, (x, t) => t.text = x);
        }
 
        public static IDisposable SubscribeToText<T>(this IObservable<T> source, TMP_Text text)
        {
            return source.SubscribeWithState(text, (x, t) => t.text = x.ToString());
        }

    }
}


