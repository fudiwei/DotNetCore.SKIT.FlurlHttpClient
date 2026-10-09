using System;
using System.Reflection;

namespace SKIT.FlurlHttpClient.Tools.CodeAnalyzer
{
    /// <summary>
    /// 表示类型声明分析器的最小分析单元，通常对应一个具体的类型或方法。
    /// </summary>
    public sealed class TypeDeclarationAnalysisUnit
    {
        /// <summary>
        /// 获取当前分析单元的种类。
        /// </summary>
        public TypeDeclarationKind Kind { get; }

        /// <summary>
        /// 获取当前分析单元的成员反射信息。
        /// </summary>
        public MemberInfo MemberInfo { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="kind"></param>
        /// <param name="memberInfo"></param>
        public TypeDeclarationAnalysisUnit(TypeDeclarationKind kind, MemberInfo memberInfo)
        {
            Kind = kind;
            MemberInfo = memberInfo ?? throw new ArgumentNullException(nameof(memberInfo));

            switch (kind)
            {
                case TypeDeclarationKind.RequestModel:
                case TypeDeclarationKind.ResponseModel:
                case TypeDeclarationKind.WebhookEvent:
                    if (memberInfo is not Type)
                        throw new ArgumentException($"The member info must be a type for type declaration kind \"{kind}\".", nameof(memberInfo));
                    break;

                case TypeDeclarationKind.ExecutingMethod:
                    if (memberInfo is not MethodInfo)
                        throw new ArgumentException($"The member info must be a method for type declaration kind \"{kind}\".", nameof(memberInfo));
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(kind), $"The type declaration kind \"{kind}\" is not supported.");
            }
        }

        /// <summary>
        /// 获取当前分析单元的成员反射信息，并视为 <see cref="Type"/>。
        /// </summary>
        public Type GetMemberAsType()
        {
            return (Type)MemberInfo;
        }

        /// <summary>
        /// 获取当前分析单元的成员反射信息，并视为 <see cref="MethodInfo"/>。
        /// </summary>
        public MethodInfo GetMemberAsMethodInfo()
        {
            return (MethodInfo)MemberInfo;
        }
    }
}
