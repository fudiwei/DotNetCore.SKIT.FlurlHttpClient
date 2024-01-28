using System;
using System.Collections.Generic;
using System.Reflection;

namespace SKIT.FlurlHttpClient.Tools.CodeAnalyzer
{
    /// <summary>
    /// 类型声明质量分析器规则。
    /// </summary>
    /// <param name="options">分析器选项。</param>
    /// <param name="aggregation">待执行的全部分析单元。</param>
    /// <param name="current">当前执行的分析单元。</param>
    public delegate void TypeDeclarationAnalyzerRule(
        TypeDeclarationAnalyzerOptions options,
        IEnumerable<TypeDeclarationAnalyzerRuleUnit> aggregation,
        TypeDeclarationAnalyzerRuleUnit current
    );

    /// <summary>
    /// 类型声明质量分析器最小分析单元。
    /// </summary>
    public record TypeDeclarationAnalyzerRuleUnit
    {
        /// <summary>
        /// 获取成员类型。
        /// </summary>
        public TypeDeclarationMemberKinds MemberKind { get; }

        /// <summary>
        /// 获取成员反射信息。
        /// </summary>
        private MemberInfo MemberInfo { get; }

        /// <summary>
        /// 获取成员反射信息，并视为 <see cref="Type"/>。
        /// </summary>
        public Type MemberAsType { get { return (Type)MemberInfo; } }

        /// <summary>
        /// 获取成员反射信息，并视为 <see cref="MethodInfo"/>。
        /// </summary>
        public MethodInfo MemberAsMethod { get { return (MethodInfo)MemberInfo; } }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memberKind"></param>
        /// <param name="memberInfo"></param>
        internal TypeDeclarationAnalyzerRuleUnit(TypeDeclarationMemberKinds memberKind, MemberInfo memberInfo)
        {
            this.MemberKind = memberKind;
            this.MemberInfo = memberInfo ?? throw new ArgumentNullException(nameof(memberInfo));
        }
    }
}
