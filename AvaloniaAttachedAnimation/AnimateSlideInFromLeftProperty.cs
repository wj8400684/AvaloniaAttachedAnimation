using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace AvaloniaAttachedAnimation;

public class AnimateSlideInFromLeftProperty1 : AnimateBaseProperty<AnimateSlideInFromLeftProperty1>
{
    protected override void DoAnimation(Control element, bool value, bool firstLoad)
    {
        throw new NotImplementedException();
    }
}





public abstract class AnimateSlideInFromLeftPropertyBase<TParent>
    where TParent : new()
{
    protected Dictionary<WeakReference, bool> AlreadyLoaded = new();

    protected Dictionary<WeakReference, bool> FirstLoadValue = new();

    public static TParent Instance { get; private set; } = new();

    protected static bool OnCallBack(AvaloniaProperty<bool> property, AvaloniaObject sender, bool value)
    {
        (Instance as AnimateSlideInFromLeftPropertyBase<TParent>)?.OnValueChanged(property, sender, value);

        return value;
    }

    protected virtual void OnValueChanged(AvaloniaProperty<bool> property, AvaloniaObject sender, bool value)
    {
        if (sender is not Control element)
            return;

        // Try and get the already loaded reference
        var alreadyLoadedReference = AlreadyLoaded.FirstOrDefault(f => f.Key.Target == sender);

        // Try and get the first load reference
        var firstLoadReference = FirstLoadValue.FirstOrDefault(f => f.Key.Target == sender);

        // Don't fire if the value doesn't change
        if (sender.GetValue<bool>(property) == value && alreadyLoadedReference.Key != null)
            return;

        // On first load...
        if (alreadyLoadedReference.Key == null)
        {
            // Create weak reference
            var weakReference = new WeakReference(sender);

            // Flag that we are in first load but have not finished it
            AlreadyLoaded[weakReference] = false;

            //首次加载应该隐藏起来
            element.Opacity = 0;

            // Create a single self-unhookable event 
            // for the elements Loaded event
            EventHandler<RoutedEventArgs>? onLoaded = null;
            onLoaded = (ss, ee) =>
            {
                // Unhook ourselves
                element.Loaded -= onLoaded;

                // Refresh the first load value in case it changed
                // since the 5ms delay
                firstLoadReference = FirstLoadValue.FirstOrDefault(f => f.Key.Target == sender);

                // Do desired animation
                DoAnimation(element, firstLoadReference.Key != null ? firstLoadReference.Value : value, true);

                // Flag that we have finished first load
                AlreadyLoaded[weakReference] = true;
            };

            // Hook into the Loaded event of the element
            element.Loaded += onLoaded;
        }
        // If we have started a first load but not fired the animation yet, update the property
        else if (alreadyLoadedReference.Value == false)
            FirstLoadValue[new WeakReference(sender)] = (bool)value;
        else
            // Do desired animation
            DoAnimation(element, value, false);
    }

    protected abstract void DoAnimation(Control element, bool value, bool firstLoad);
}

public class AnimateSlideInFromRightProperty : AnimateSlideInFromLeftPropertyBase<AnimateSlideInFromRightProperty>
{
    public static readonly AvaloniaProperty<bool> ValueProperty =
        AvaloniaProperty.RegisterAttached<AvaloniaObject, bool>
            ("Value", typeof(AnimateSlideInFromRightProperty), coerce: OnCallBack);
    private static bool OnCallBack(AvaloniaObject arg1, bool arg2) => OnCallBack(ValueProperty, arg1, arg2);

    public static bool GetValue(AvaloniaObject d) => d.GetValue<bool>(ValueProperty);

    public static void SetValue(AvaloniaObject d, bool value) => d.SetValue(ValueProperty, value);

    protected override async void DoAnimation(Control element, bool value, bool firstLoad)
    {
        if (value)
            await element.SlideAndFadeInAsync(AnimationSlideInDirection.Right, firstLoad, firstLoad ? 0 : 0.3f,
                keepMargin: true);
        else
            await element.SlideAndFadeOutAsync(AnimationSlideInDirection.Right, firstLoad ? 0 : 0.3f, keepMargin: true,
                firstLoad: firstLoad);
    }
}

public class AnimateSlideInFromLeftProperty : AnimateSlideInFromLeftPropertyBase<AnimateSlideInFromLeftProperty>
{
    public static readonly AvaloniaProperty<bool> ValueProperty =
        AvaloniaProperty.RegisterAttached<AvaloniaObject, bool>
            ("Value", typeof(AnimateSlideInFromLeftProperty), coerce: OnCallBack);
    private static bool OnCallBack(AvaloniaObject arg1, bool arg2) => OnCallBack(ValueProperty, arg1, arg2);

    public static bool GetValue(AvaloniaObject d) => d.GetValue<bool>(ValueProperty);

    public static void SetValue(AvaloniaObject d, bool value) => d.SetValue(ValueProperty, value);

    protected override async void DoAnimation(Control element, bool value, bool firstLoad)
    {
        const float seconds = 3;

        if (value)
            // Animate in
            await element.SlideAndFadeInAsync(AnimationSlideInDirection.Left, firstLoad, firstLoad ? 0 : seconds,
                keepMargin: true);
        else
            // Animate out
            await element.SlideAndFadeOutAsync(AnimationSlideInDirection.Left, firstLoad ? 0 : seconds, keepMargin: true,
                firstLoad: firstLoad);
    }
}

public class AnimateSlideInFromTopProperty : AnimateSlideInFromLeftPropertyBase<AnimateSlideInFromTopProperty>
{
    public static readonly AvaloniaProperty<bool> ValueProperty =
        AvaloniaProperty.RegisterAttached<AvaloniaObject, bool>
            ("Value", typeof(AnimateSlideInFromTopProperty), coerce: OnCallBack);
    private static bool OnCallBack(AvaloniaObject arg1, bool arg2) => OnCallBack(ValueProperty, arg1, arg2);

    public static bool GetValue(AvaloniaObject d) => d.GetValue<bool>(ValueProperty);

    public static void SetValue(AvaloniaObject d, bool value) => d.SetValue(ValueProperty, value);

    protected override async void DoAnimation(Control element, bool value, bool firstLoad)
    {
        if (value)
            // Animate in
            await element.SlideAndFadeInAsync(AnimationSlideInDirection.Top, firstLoad, firstLoad ? 0 : 0.3f,
                keepMargin: true);
        else
            // Animate out
            await element.SlideAndFadeOutAsync(AnimationSlideInDirection.Top, firstLoad ? 0 : 0.3f, keepMargin: true,
                firstLoad: firstLoad);
    }
}


public class AnimateSlideInFromBottomProperty : AnimateSlideInFromLeftPropertyBase<AnimateSlideInFromBottomProperty>
{
    public static readonly AvaloniaProperty<bool> ValueProperty =
        AvaloniaProperty.RegisterAttached<AvaloniaObject, bool>
            ("Value", typeof(AnimateSlideInFromBottomProperty), coerce: OnCallBack);
    private static bool OnCallBack(AvaloniaObject arg1, bool arg2) => OnCallBack(ValueProperty, arg1, arg2);

    public static bool GetValue(AvaloniaObject d) => d.GetValue<bool>(ValueProperty);

    public static void SetValue(AvaloniaObject d, bool value) => d.SetValue(ValueProperty, value);

    protected override async void DoAnimation(Control element, bool value, bool firstLoad)
    {
        if (value)
            // Animate in
            await element.SlideAndFadeInAsync(AnimationSlideInDirection.Bottom, firstLoad, firstLoad ? 0 : 0.3f,
                keepMargin: true);
        else
            // Animate out
            await element.SlideAndFadeOutAsync(AnimationSlideInDirection.Bottom, firstLoad ? 0 : 0.3f, keepMargin: true,
                firstLoad: firstLoad);
    }
}

















