using System;
using WPFUI.Common;

namespace WPFUI.DIControls;

/// <summary>
/// 
/// </summary>
public sealed class StoreNavigationItem : DefaultNavigationItem
{
    /// <inheritdoc />
    public StoreNavigationItem(Type pageType, string pageTag, string text, SymbolRegular icon, bool iconFilled = false) : base(pageType, pageTag, text, icon, iconFilled)
    {
    }

    /// <inheritdoc />
    public StoreNavigationItem(Type pageType, string pageTag, string text) : base(pageType, pageTag, text)
    {
    }

    /// <inheritdoc />
    public StoreNavigationItem(Type pageType, string pageTag, Uri imageUri) : base(pageType, pageTag, imageUri)
    {
    }

    /// <inheritdoc />
    public StoreNavigationItem(Type pageType, string pageTag) : base(pageType, pageTag)
    {
    }

    /// <inheritdoc />
    public StoreNavigationItem(Type pageType, string pageTag, SymbolRegular icon) : base(pageType, pageTag, icon)
    {
    }
}