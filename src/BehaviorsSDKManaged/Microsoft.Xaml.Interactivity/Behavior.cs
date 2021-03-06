﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Windows.UI.Xaml;

namespace Microsoft.Xaml.Interactivity
{
  /// <summary>
  /// A base class for behaviors, implementing the basic plumbing of IBehavior
  /// </summary>
  public abstract class Behavior : DependencyObject, IBehavior
  {
    public DependencyObject AssociatedObject { get; set; }

    public virtual void Attach(DependencyObject associatedObject)
    {
      AssociatedObject = associatedObject;
    }

    public virtual void Detach()
    {
    }
  }
}
