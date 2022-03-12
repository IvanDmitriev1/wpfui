using System;

namespace WPFUI.DIControls;

public sealed class StoreNavigationItem : DefaultNavigationItem
{
    public StoreNavigationItem(Type pageType, string text, Common.Icon icon, bool iconFilled = false) : base(pageType, text, icon, iconFilled)
    {
    }

    public StoreNavigationItem(Type pageType, Common.Icon icon, bool iconFilled = false) : base(pageType, icon, iconFilled)
    {
    }

    public StoreNavigationItem(Type pageType, string text) : base(pageType, text)
    {
    }

    public StoreNavigationItem(Type pageType) : base(pageType)
    {
    }

    public StoreNavigationItem(Type pageType, string text, Uri imageUri) : base(pageType, text, imageUri)
    {
    }

    public StoreNavigationItem(Type pageType, Uri imageUri) : base(pageType, imageUri)
    {
    }
}