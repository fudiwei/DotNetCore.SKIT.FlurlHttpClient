using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace SKIT.FlurlHttpClient.Tools.CodeAnalyzer
{
    public class CodeAnalyzer
    {
        private readonly Assembly _assembly;
        private readonly string _dirSourceCode;
        private readonly string _dirTestSample;
        private readonly bool _allowNotFoundModelTypes;
        private readonly bool _allowNotFoundEventTypes;
        private readonly bool _allowNotFoundModelSamples;
        private readonly bool _allowNotFoundEventSamples;

        protected IList<Exception> Errors { get; }

        public CodeAnalyzer(CodeAnalyzerOptions options)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));

            _assembly = Assembly.Load(options.AssemblyName);
            _dirSourceCode = options.WorkDirectoryForSourceCode;
            _dirTestSample = options.WorkDirectoryForTestSample;
            _allowNotFoundModelTypes = options.AllowNotFoundModelTypes;
            _allowNotFoundEventTypes = options.AllowNotFoundEventTypes;
            _allowNotFoundModelSamples = options.AllowNotFoundModelSamples;
            _allowNotFoundEventSamples = options.AllowNotFoundEventSamples;

            Errors = new List<Exception>();
        }

        public void Start()
        {
            lock (Errors)
            {
                AnalyzeAssembly();
                AnalyzeSourceCode();
                AnalyzeTestSample();
            }
        }

        public void Assert()
        {
            lock (Errors)
            {
                if (Errors.Any())
                {
                    throw new AggregateException($"There {(Errors.Count > 1 ? "are" : "is")} {Errors.Count} error{(Errors.Count > 1 ? "s" : "")}.", Errors);
                }
            }
        }

        public void Flush()
        {
            lock (Errors)
            {
                Errors.Clear();
            }
        }

        private void AnalyzeAssembly()
        {
            Type[] modelTypes = _assembly.GetTypes()
                .Where(t =>
                    t.Namespace != null &&
                    t.Namespace.Equals($"{_assembly.GetName().Name}.Models") &&
                    t.IsClass &&
                    !t.IsAbstract &&
                    !t.IsInterface &&
                    !t.IsNested
                )
                .ToArray();
            Type[] eventTypes = _assembly.GetTypes()
                .Where(t =>
                    t.Namespace != null &&
                    t.Namespace.Equals($"{_assembly.GetName().Name}.Events") &&
                    t.IsClass &&
                    !t.IsAbstract &&
                    !t.IsInterface &&
                    !t.IsNested
                )
                .ToArray();
            Trace.Assert(modelTypes.Any() || _allowNotFoundModelTypes, $"【异常】程序集下不存在 API 模型类型，请检查 \"Models\" 命名空间是否为空。");
            Trace.Assert(eventTypes.Any() || _allowNotFoundEventTypes, $"【异常】程序集下不存在 API 事件类型，请检查 \"Events\" 命名空间是否为空。");

            /* 校验 API 模型程序集类型 */
            Parallel.ForEach(modelTypes, AnalyzeAssembly_ModelTypeNameCase);
        }

        private void AnalyzeAssembly_ModelTypeNameCase(Type type)
        {
            const string SUFFIX_REQUEST_MODEL = "Request";
            const string SUFFIX_RESPONSE_MODEL = "Response";

            try
            {
                string typeName = type.Name.Split('`')[0];

                if (typeName.EndsWith(SUFFIX_REQUEST_MODEL))
                {
                    if (!typeof(ICommonRequest).IsAssignableFrom(type))
                        throw new Exception($"The type '{type}' may be a request model, but it's not implement the interface '{nameof(ICommonRequest)}'.");

                    if (!_assembly.GetTypes().Any(t => t.Name == $"{typeName.Substring(0, typeName.Length - SUFFIX_REQUEST_MODEL.Length)}{SUFFIX_RESPONSE_MODEL}"))
                        throw new Exception($"The type `{type}` may be a request model, but there is no associated response model was found.");
                }
                else if (typeName.EndsWith(SUFFIX_RESPONSE_MODEL))
                {
                    if (!typeof(ICommonResponse).IsAssignableFrom(type))
                        throw new Exception($"The type '{type}' may be a response model, but it's not implement the interface '{nameof(ICommonResponse)}'.");

                    if (!_assembly.GetTypes().Any(t => t.Name == $"{typeName.Substring(0, typeName.Length - SUFFIX_RESPONSE_MODEL.Length)}{SUFFIX_REQUEST_MODEL}"))
                        throw new Exception($"The type `{type}` may be a response model, but there is no associated request model was found.");
                }
                else
                {
                    throw new Exception($"The type '{type}' is not named end with '{SUFFIX_REQUEST_MODEL}' or '{SUFFIX_RESPONSE_MODEL}'.");
                }
            }
            catch (Exception ex)
            {
                Errors.Add(ex);
            }
        }

        private void AnalyzeSourceCode()
        {
            DirectoryInfo dir = new DirectoryInfo(_dirSourceCode);
            Trace.Assert(dir.Exists, $"【异常】工作目录 \"{dir.FullName}\" 不存在。");

            FileInfo[] modelDefinationFiles = dir.GetFiles("Models/*.cs", SearchOption.AllDirectories);
            FileInfo[] eventDefinationFiles = dir.GetFiles("Events/*.cs", SearchOption.AllDirectories);
            FileInfo[] apiDefinationFiles = dir.GetFiles("Extensions/*ClientExecute*Extensions.cs", SearchOption.AllDirectories);
            Trace.Assert(modelDefinationFiles.Any() || _allowNotFoundModelTypes, $"【异常】工作目录下不存在 API 模型源代码文件，请检查 \"Models\" 目录是否为空。");
            Trace.Assert(apiDefinationFiles.Any() || _allowNotFoundModelTypes, $"【异常】工作目录下不存在 API 接口源代码文件，请检查 \"Extensions\" 目录是否为空。");

            /* 校验 API 模型源代码文件 */
            Parallel.ForEach(modelDefinationFiles, AnalyzeSourceCode_ModelDefinationFile);

            /* 校验 API 事件源代码文件 */
            Parallel.ForEach(eventDefinationFiles, AnalyzeSourceCode_EventDefinationFile);

            /* 校验 API 接口源代码文件 */
            Parallel.ForEach(apiDefinationFiles, AnalyzeSourceCode_ApiDefinationFile);
        }

        private void AnalyzeSourceCode_ModelDefinationFile(FileInfo file)
        {
            try
            {
                string fileName = Path.GetFileNameWithoutExtension(file.Name);
                Type targetType = _assembly.GetType($"{_assembly.GetName().Name}.Models.{fileName}", throwOnError: true);
                if (!targetType.IsAbstract && !targetType.IsInterface)
                {
                    if (!Helpers.JsonHelper.TrySerialize(Activator.CreateInstance(targetType), targetType, out Exception ex))
                        Errors.Add(ex);
                }
            }
            catch (Exception ex)
            {
                Errors.Add(ex);
            }
        }

        private void AnalyzeSourceCode_EventDefinationFile(FileInfo file)
        {
            try
            {
                string fileName = Path.GetFileNameWithoutExtension(file.Name);
                Type targetType = _assembly.GetType($"{_assembly.GetName().Name}.Events.{fileName}", throwOnError: true);
                if (!targetType.IsAbstract && !targetType.IsInterface)
                {
                    if (!Helpers.JsonHelper.TrySerialize(Activator.CreateInstance(targetType), targetType, out Exception ex))
                        Errors.Add(ex);
                }
            }
            catch (Exception ex)
            {
                Errors.Add(ex);
            }
        }

        private void AnalyzeSourceCode_ApiDefinationFile(FileInfo file)
        {
            try
            {

            }
            catch (Exception ex)
            {
                Errors.Add(ex);
            }
        }

        private void AnalyzeTestSample()
        {
            DirectoryInfo dir = new DirectoryInfo(_dirTestSample);
            Trace.Assert(dir.Exists, $"【异常】工作目录 \"{dir.FullName}\" 不存在。");

            FileInfo[] modelSampleFiles = dir.GetFiles("ModelSamples/*", SearchOption.AllDirectories);
            FileInfo[] eventSampleFiles = dir.GetFiles("EventSamples/*", SearchOption.AllDirectories);
            Trace.Assert(modelSampleFiles.Any() || _allowNotFoundModelSamples, $"【异常】工作目录下不存在 API 模型示例文件，请检查 \"ModelSamples\" 目录是否为空。");
            Trace.Assert(eventSampleFiles.Any() || _allowNotFoundEventSamples, $"【异常】工作目录下不存在 API 事件示例文件，请检查 \"EventSamples\" 目录是否为空。");

            /* 校验 API 模型示例文件 */
            Parallel.ForEach(modelSampleFiles, AnalyzeTestSample_ModelSampleFile);

            /* 校验 API 事件示例文件 */
            Parallel.ForEach(eventSampleFiles, AnalyzeTestSample_EventSampleFile);
        }

        private void AnalyzeTestSample_ModelSampleFile(FileInfo file)
        {
            try
            {
                string fileName = Path.GetFileNameWithoutExtension(file.Name).Split('.').First();
                Type targetType = _assembly.GetType($"{_assembly.GetName().Name}.Models.{fileName}", throwOnError: true);
                if (!targetType.IsAbstract && !targetType.IsInterface)
                {
                    using Stream fileStream = file.OpenRead();
                    using TextReader fileReader = new StreamReader(fileStream);
                    string fileFormat = Path.GetExtension(file.Name).ToLower();
                    string fileContent = fileReader.ReadToEnd();
                    switch (fileFormat)
                    {
                        case ".json":
                            {
                                if (!Helpers.JsonHelper.TryDeserialize(fileContent, targetType, out Exception ex))
                                    Errors.Add(ex);
                            }
                            break;

                        case ".xml":
                            {
                                if (!Helpers.XmlHelper.TryDeserialize(fileContent, targetType, out Exception ex))
                                    Errors.Add(ex);
                            }
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Errors.Add(ex);
            }
        }

        private void AnalyzeTestSample_EventSampleFile(FileInfo file)
        {
            try
            {
                string fileName = Path.GetFileNameWithoutExtension(file.Name).Split('.').First();
                Type targetType = _assembly.GetType($"{_assembly.GetName().Name}.Events.{fileName}", throwOnError: true);
                if (!targetType.IsAbstract && !targetType.IsInterface)
                {
                    using Stream fileStream = file.OpenRead();
                    using TextReader fileReader = new StreamReader(fileStream);
                    string fileFormat = Path.GetExtension(file.Name).ToLower();
                    string fileContent = fileReader.ReadToEnd();
                    switch (fileFormat)
                    {
                        case ".json":
                            {
                                if (!Helpers.JsonHelper.TryDeserialize(fileContent, targetType, out Exception ex))
                                    Errors.Add(ex);
                            }
                            break;

                        case ".xml":
                            {
                                if (!Helpers.XmlHelper.TryDeserialize(fileContent, targetType, out Exception ex))
                                    Errors.Add(ex);
                            }
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Errors.Add(ex);
            }
        }
    }
}
