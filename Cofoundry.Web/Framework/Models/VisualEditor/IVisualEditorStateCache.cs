﻿namespace Cofoundry.Web;

public interface IVisualEditorStateCache
{
    /// <summary>
    /// Sets the cache value.
    /// </summary>
    VisualEditorState Get();

    /// <summary>
    /// Gets the cache value or null if it has not been set.
    /// </summary>
    void Set(VisualEditorState data);
}
