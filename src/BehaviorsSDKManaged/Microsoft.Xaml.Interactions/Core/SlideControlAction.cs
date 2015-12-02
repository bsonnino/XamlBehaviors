using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Microsoft.Xaml.Interactivity;

namespace Microsoft.Xaml.Interactions.Core
{
    /// <summary>
    ///     An action that will slide the control outside of its parent
    /// </summary>
    public sealed class SlideControlAction : DependencyObject, IAction
    {
        /// <summary>
        ///     Identifies the <seealso cref="SlideAnimationKind" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty SlideAnimationKindProperty =
            DependencyProperty.Register("SlideSlideAnimationKind", typeof (SlideAnimationKind),
                typeof (SlideControlAction),
                new PropertyMetadata(SlideAnimationKind.Right));

        /// <summary>
        ///     Identifies the <seealso cref="Duration" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty DurationProperty =
            DependencyProperty.Register("Duration", typeof (TimeSpan), typeof (SlideControlAction),
                new PropertyMetadata(TimeSpan.FromMilliseconds(500)));

        /// <summary>
        ///     Gets or sets the slide animation type. This is a dependency property.
        /// </summary>
        public SlideAnimationKind SlideAnimationKind
        {
            get { return (SlideAnimationKind) GetValue(SlideAnimationKindProperty); }
            set { SetValue(SlideAnimationKindProperty, value); }
        }

        /// <summary>
        ///     Gets or sets the duration of the sliding animation. This is a dependency property.
        /// </summary>
        public TimeSpan Duration
        {
            get { return (TimeSpan) GetValue(DurationProperty); }
            set { SetValue(DurationProperty, value); }
        }

        /// <summary>
        ///     Executes the action.
        /// </summary>
        /// <param name="sender">
        ///     The <see cref="System.Object" /> that is passed to the action by the behavior. Generally this is
        ///     <seealso cref="Microsoft.Xaml.Interactivity.IBehavior.AssociatedObject" /> or a target object.
        /// </param>
        /// <param name="parameter">The value of this parameter is determined by the caller.</param>
        /// <returns>True if the specified operation is invoked successfully; else false.</returns>
        public object Execute(object sender, object parameter)
        {
            AnimateControl((FrameworkElement) sender, this.Duration, this.SlideAnimationKind);
            return true;
        }

        private void AnimateControl(FrameworkElement control, TimeSpan duration, SlideAnimationKind kind)
        {
            double finalPoint = 0;
            double moveWidth = (control.Parent as FrameworkElement)?.ActualWidth ?? control.ActualWidth;
            double moveHeight = (control.Parent as FrameworkElement)?.ActualHeight ?? control.ActualHeight;
            if (kind == SlideAnimationKind.Left)
            {
                finalPoint = -moveWidth;
            }
            else if (kind == SlideAnimationKind.Right)
            {
                finalPoint = moveWidth;
            }
            else if (kind == SlideAnimationKind.Up)
            {
                finalPoint = -moveHeight;
            }
            else if (kind == SlideAnimationKind.Down)
            {
                finalPoint = moveHeight;
            }
            TranslateTransform translate = new TranslateTransform {X = 0, Y = 0};
            control.RenderTransform = translate;
            CreateAndRunAnimation(control, duration, finalPoint,
                (kind == SlideAnimationKind.Left || kind == SlideAnimationKind.Right)
                    ? "(UIElement.RenderTransform).(TranslateTransform.X)"
                    : "(UIElement.RenderTransform).(TranslateTransform.Y)");
        }

        private static void CreateAndRunAnimation(FrameworkElement control, TimeSpan duration, double finalPoint,
            string targetProperty)
        {
            DoubleAnimation da = new DoubleAnimation {From = 0, To = finalPoint, Duration = duration};
            Storyboard.SetTarget(da, control);
            Storyboard.SetTargetProperty(da, targetProperty);
            Storyboard sb = new Storyboard();
            sb.Children.Add(da);
            sb.Begin();
        }
    }
}