namespace SKIT.FlurlHttpClient.Internal.CodeAnalysis
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = true)]
    public sealed class DisallowAnalyzerRuleAttribute : Attribute
    {
        public string RuleId { get; }

        public DisallowAnalyzerRuleAttribute(string ruleId)
        {
            RuleId = ruleId ?? throw new ArgumentNullException(nameof(ruleId));
        }
    }
}
