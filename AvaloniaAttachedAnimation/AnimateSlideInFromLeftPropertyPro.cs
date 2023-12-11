using Avalonia;

namespace AvaloniaAttachedAnimation;

public class AnimateSlideInFromLeftPropertyBasePro<TParent> : AvaloniaObject
{
    public static readonly AvaloniaProperty<bool> ValueProperty =
        AvaloniaProperty.RegisterAttached<AvaloniaObject, bool>
            ("Value", typeof(AnimateSlideInFromLeftPropertyBasePro<TParent>));

    public static bool GetValue(AvaloniaObject d) => d.GetValue<bool>(ValueProperty);

    public static void SetValue(AvaloniaObject d, bool value) => d.SetValue(ValueProperty, value);
}

public class AnimateSlideInFromLeftPropertyPro : AnimateSlideInFromLeftPropertyBasePro<AnimateSlideInFromLeftPropertyPro>
{
    
}