﻿namespace Cofoundry.Core.Web;

/// <summary>
/// Factory to create the default ruleset for the sanitizer
/// </summary>
public interface IDefaultHtmlSanitizationRuleSetFactory
{
    IHtmlSanitizationRuleSet Create();
}
