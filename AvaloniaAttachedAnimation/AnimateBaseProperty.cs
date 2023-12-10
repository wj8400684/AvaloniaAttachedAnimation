using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace AvaloniaAttachedAnimation;

public abstract class AnimateBaseProperty<TParent> : BaseAttachedProperty<TParent, bool>
    where TParent : BaseAttachedProperty<TParent, bool>, new()
{
    #region Protected Properties

    /// <summary>
    /// True if this is the very first time the value has been updated
    /// Used to make sure we run the logic at least once during first load
    /// </summary>
    protected Dictionary<WeakReference, bool> mAlreadyLoaded = new Dictionary<WeakReference, bool>();

    /// <summary>
    /// The most recent value used if we get a value changed before we do the first load
    /// </summary>
    protected Dictionary<WeakReference, bool> mFirstLoadValue = new Dictionary<WeakReference, bool>();

    #endregion

    protected override void OnValueChanged(AvaloniaObject sender, bool value)
    {
        if (sender is not Control element)
            return;

        // Try and get the already loaded reference
        var alreadyLoadedReference = mAlreadyLoaded.FirstOrDefault(f => f.Key.Target == sender);

        // Try and get the first load reference
        var firstLoadReference = mFirstLoadValue.FirstOrDefault(f => f.Key.Target == sender);

        // Don't fire if the value doesn't change
        if ((bool)sender.GetValue(ValueProperty) == value && alreadyLoadedReference.Key != null)
            return;

        // On first load...
        if (alreadyLoadedReference.Key == null)
        {
            // Create weak reference
            var weakReference = new WeakReference(sender);

            // Flag that we are in first load but have not finished it
            mAlreadyLoaded[weakReference] = false;

            // Start off hidden before we decide how to animate
            element.IsVisible = true;

            // Create a single self-unhookable event 
            // for the elements Loaded event
            EventHandler<RoutedEventArgs> onLoaded = null;
            onLoaded = async (ss, ee) =>
            {
                // Unhook ourselves
                element.Loaded -= onLoaded;

                // Slight delay after load is needed for some elements to get laid out
                // and their width/heights correctly calculated
                await Task.Delay(5);

                // Refresh the first load value in case it changed
                // since the 5ms delay
                firstLoadReference = mFirstLoadValue.FirstOrDefault(f => f.Key.Target == sender);

                // Do desired animation
                DoAnimation(element, firstLoadReference.Key != null ? firstLoadReference.Value : (bool)value, true);

                // Flag that we have finished first load
                mAlreadyLoaded[weakReference] = true;
            };

            // Hook into the Loaded event of the element
            element.Loaded += onLoaded;
        }
        // If we have started a first load but not fired the animation yet, update the property
        else if (alreadyLoadedReference.Value == false)
            mFirstLoadValue[new WeakReference(sender)] = (bool)value;
        else
            // Do desired animation
            DoAnimation(element, (bool)value, false);
    }

    protected abstract void DoAnimation(Control element, bool value, bool firstLoad);
}