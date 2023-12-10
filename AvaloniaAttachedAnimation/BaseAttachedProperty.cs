
using Avalonia;

namespace AvaloniaAttachedAnimation;

public abstract class BaseAttachedProperty<TParent, TProperty> : AvaloniaObject
    where TParent : new()
{
    public static TParent Instance { get; private set; } = new();

    public static readonly AvaloniaProperty<TProperty> ValueProperty =
        AvaloniaProperty.RegisterAttached<AvaloniaObject, TProperty>
            ("Value", typeof(BaseAttachedProperty<TParent, TProperty>), coerce: CallBack);

    public static TProperty GetValue(AvaloniaObject d) => d.GetValue<TProperty>(ValueProperty);

    public static void SetValue(AvaloniaObject d, TProperty value) => d.SetValue(ValueProperty, value);

    protected virtual void OnValueChanged(AvaloniaObject sender, TProperty e)
    {
    }

    private static TProperty CallBack(AvaloniaObject sender, TProperty value)
    {
        // Call the parent function
        (Instance as BaseAttachedProperty<TParent, TProperty>)?.OnValueChanged(sender, value);

        // Return the value
        return value;
    }
}







// public class AttachAnimation : AvaloniaObject
// {
//     static AttachAnimation()
//     {
//         ValueProperty.Changed.AddClassHandler<Control>(OnCallBack);
//     }
//
//     public static void Ini()
//     {
//     }
//
//     protected Dictionary<WeakReference, bool> mAlreadyLoaded = new Dictionary<WeakReference, bool>();
//
//     /// <summary>
//     /// The most recent value used if we get a value changed before we do the first load
//     /// </summary>
//     protected Dictionary<WeakReference, bool> mFirstLoadValue = new Dictionary<WeakReference, bool>();
//
//
//     private static void OnCallBack(Control control, AvaloniaPropertyChangedEventArgs property)
//     {
//         control.Loaded += OnControlLoaded;
//     }
//
//     public static readonly AvaloniaProperty<bool> ValueProperty =
//         AvaloniaProperty.RegisterAttached<AvaloniaObject, bool>
//         (
//             "Value",
//             typeof(AttachAnimation),
//             coerce: CallBack1);
//
//     private static bool CallBack1(AvaloniaObject avaloniaObject, bool value)
//     {
//         if (avaloniaObject is not Control control)
//             return value;
//
//         EventHandler<RoutedEventArgs>? onLoaded = null;
//
//         onLoaded = async (sender, arg) =>
//         {
//             control.Loaded -= onLoaded;
//             await Task.Delay(5);
//
//
//             Console.WriteLine(1);
//         };
//
//         control.Loaded += onLoaded;
//
//         return value;
//     }
//
//     public static bool GetValue(Control d) => d.GetValue<bool>(ValueProperty);
//
//     public static void SetValue(Control d, bool value) => d.SetValue(ValueProperty, value);
//
//     private static bool CallBack(AvaloniaObject arg1, bool arg2)
//     {
//         return arg2;
//     }
//
//     private static void OnControlLoaded(object? sender, RoutedEventArgs e)
//     {
//     }
// }