using System;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Animation;
using Avalonia.Controls;
using Avalonia.Layout;

namespace AvaloniaAttachedAnimation;

public static class TransitionExtension
{
    public static async Task SlideAndFadeOutAsync(this Control element, AnimationSlideInDirection direction,
        float seconds = 0.3f, bool keepMargin = true, int size = 0, bool firstLoad = false)
    {
        // Create the storyboard
        var offset = size == 0 ? element.Bounds.Width : size;

        element.Margin = new Thickness(0);

        element.Transitions = new Transitions
        {
            new ThicknessTransition()
            {
                Duration = TimeSpan.FromSeconds(seconds),
                Property = Layoutable.MarginProperty,
            }
        };

        //首次加载应该显示出来
        if (firstLoad)
            element.Opacity = 1;
        
        element.Margin = direction switch
        {
            AnimationSlideInDirection.Left => new Thickness(-offset, 0, keepMargin ? offset : 0, 0),
            AnimationSlideInDirection.Right => new Thickness(keepMargin ? offset : 0, 0, -offset, 0),
            AnimationSlideInDirection.Top => new Thickness(0, -offset, 0, keepMargin ? offset : 0),
            AnimationSlideInDirection.Bottom => new Thickness(0, keepMargin ? offset : 0, 0, -offset),
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };

        // Make element invisible
        // if (element.Opacity == 0)
        //     element.IsVisible = true;
        
        // Wait for it to finish
        await Task.Delay((int)(seconds * 1000));
    }

    public static async Task SlideAndFadeInAsync(this Control element, AnimationSlideInDirection direction,
        bool firstLoad, float seconds = 0.3f, bool keepMargin = true, int size = 0)
    {
        var offset = size == 0 ? element.Bounds.Width : size;

        element.Margin = direction switch
        {
            AnimationSlideInDirection.Left => new Thickness(-offset, 0, keepMargin ? offset : 0, 0),
            AnimationSlideInDirection.Right => new Thickness(keepMargin ? offset : 0, 0, -offset, 0),
            AnimationSlideInDirection.Top => new Thickness(0, -offset, 0, keepMargin ? offset : 0),
            AnimationSlideInDirection.Bottom => new Thickness(0, keepMargin ? offset : 0, 0, -offset),
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };

        element.Transitions = new Transitions
        {
            new ThicknessTransition()
            {
                Duration = TimeSpan.FromSeconds(seconds),
                Property = Layoutable.MarginProperty,
            }
        };

        // Make page visible only if we are animating or its the first load
        if (firstLoad)//首次加载应该显示出来
            element.Opacity = 1;

        element.Margin = new Thickness(0);

        // Wait for it to finish
        await Task.Delay((int)(seconds * 1000));
    }
}