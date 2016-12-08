using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BlindApp.Views
{
    public static class AccessibilityEffect
    {
        public static readonly BindableProperty AccessibilityLabelProperty =
          BindableProperty.CreateAttached("AccessibilityLabel",
              typeof(string), typeof(AccessibilityEffect), "",
              propertyChanged: OnAccessibilityLabelChanged);
        public static readonly BindableProperty InAccessibleTreeProperty =
          BindableProperty.CreateAttached("InAccessibleTree",
              typeof(bool), typeof(AccessibilityEffect), true,
              propertyChanged: OnInAccessibleTreeChanged);

        // AccessibilityLabel
        public static string GetAccessibilityLabel(BindableObject view)
        {
            return (string)view.GetValue(AccessibilityLabelProperty);
        }
        public static void SetAccessibilityLabel(BindableObject view, string value)
        {
            view.SetValue(AccessibilityLabelProperty, value);
        }
        static void OnAccessibilityLabelChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var hasLabel = !string.IsNullOrEmpty(newValue as string);
            AddRemoveEffect(bindable, hasLabel);
        }
        // InAccessibleTree
        public static bool GetInAccessibleTree(BindableObject view)
        {
            return (bool)view.GetValue(InAccessibleTreeProperty);
        }
        public static void SetInAccessibleTree(BindableObject view, bool value)
        {
            view.SetValue(InAccessibleTreeProperty, value);
        }
        static void OnInAccessibleTreeChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var add = newValue != null;
            AddRemoveEffect(bindable, add);
        }
        // .. other properties
        // helper method
        static void AddRemoveEffect(BindableObject bindable, bool add)
        {
            var view = bindable as View;
            if (view == null)
            {
                return;
            }
            if (add)
            {
                if (view.Effects.Count == 0)
                {
                    // shortcut to add if there are none already
                    view.Effects.Add(new AddAccessibilityEffect());
                }
                else
                {
                    // more expensive check to see if it exists before adding
                    var exists = view.Effects.First(e => e is AddAccessibilityEffect);
                    if (exists == null)
                    {
                        view.Effects.Add(new AddAccessibilityEffect());
                    }
                }
            }
            else
            {
                var toRemove = view.Effects.FirstOrDefault(e => e is AddAccessibilityEffect);
                if (toRemove != null)
                {
                    view.Effects.Remove(toRemove);
                }
            }
        }
        public class AddAccessibilityEffect : RoutingEffect
        {
            // string identifier matches [assembly] attributes in the platform-specific projects
            public AddAccessibilityEffect() : base("MyCompany.AddAccessibilityEffect")
            {
            }
        }
    }
}
