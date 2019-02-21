using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Optimization;

namespace SS.Mvc.AxosoftApi
{
    public static class BundleExtensions
    {
        public static BundleString Render(params string[] paths)
        {
            if (paths == null) throw new ArgumentNullException(nameof(paths));
            //var result = Scripts.RenderFormat(Format, paths);
            var uniqueRefs = DeterminePathsToRender(paths);

            return new BundleString(uniqueRefs);
        }

        public static IEnumerable<string> DeterminePathsToRender(IEnumerable<string> assets)
        {
            var resolver = BundleResolver.Current;
            var paths = new List<string>();
            foreach (var path in assets)
            {
                if (resolver.IsBundleVirtualPath(path))
                {
                    if (!BundleTable.EnableOptimizations)
                    {
                        var contents = resolver.GetBundleContents(path)
                            // Fix for BundleTransformer
                            .Select(x => x.StartsWith("/") ? "~" + x : x);
                        paths.AddRange(contents);
                    }
                    else
                    {
                        paths.Add(path);
                        if (BundleTable.Bundles.UseCdn)
                        {
                            var bundle = BundleTable.Bundles.GetBundleFor(path);
                            if (!string.IsNullOrEmpty(bundle?.CdnPath) && !string.IsNullOrEmpty(bundle.CdnFallbackExpression))
                            {
                                throw new NotSupportedException("CDN not supported.");
                            }
                        }
                    }
                }
                else
                {
                    paths.Add(path);
                }
            }

            return EliminateDuplicatesAndResolveUrls(paths);
        }

        private static IEnumerable<string> EliminateDuplicatesAndResolveUrls(IEnumerable<string> paths)
        {
            var firstPass = new List<string>();
            var pathMap = new HashSet<string>();
            var bundledContents = new HashSet<string>();
            var resolver = BundleResolver.Current;

            foreach (var path in paths)
            {
                if (pathMap.Add(path))
                {
                    // Need to crack open bundles to look at its contents for the second pass
                    if (resolver.IsBundleVirtualPath(path))
                    {
                        var contents = resolver.GetBundleContents(path);
                        foreach (var filePath in contents)
                        {
                            bundledContents.Add(ResolveVirtualPath(filePath));
                        }
                        firstPass.Add(resolver.GetBundleUrl(path));
                    }
                    // Non bundles we want to resolve the path and check it's not a duplicate before adding
                    else
                    {
                        var resolvedPath = ResolveVirtualPath(path);
                        if (!pathMap.Contains(resolvedPath))
                        {
                            pathMap.Add(resolvedPath);
                            firstPass.Add(resolvedPath);
                        }
                    }
                }
            }

            return firstPass.Where(x => !bundledContents.Contains(x));
        }

        internal static string ResolveVirtualPath(string virtualPath)
        {
            var basePath = HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath;
            var path = VirtualPathUtility.Combine(basePath, virtualPath);
            // Make sure it's not a ~/ path, which the client couldn't handle
            path = VirtualPathUtility.ToAbsolute(path);
            return HttpUtility.UrlPathEncode(path);
        }
    }

    public sealed class BundleString : IHtmlString
    {
        private const string Format = "\"{0}\"";
        private readonly List<string> _paths;

        public BundleString(IEnumerable<string> paths)
        {
            if (paths == null) throw new ArgumentNullException(nameof(paths));
            _paths = new List<string>(paths);
        }

        public string ToHtmlString()
        {
            var builder = new StringBuilder();
            var first = true;
            foreach (var path in _paths)
            {
                if (!first)
                {
                    builder.Append(",").Append(Environment.NewLine);
                }
                builder.AppendFormat(Format, path);
                first = false;
            }

            return builder.ToString();
        }

        public BundleString Concat(BundleString other)
        {
            if (other == null) throw new ArgumentNullException(nameof(other));
            _paths.AddRange(other._paths);
            return this;
        }

        public BundleString Concat(string path)
        {
            if (path == null) throw new ArgumentNullException(nameof(path));
            _paths.Add(path);
            return this;
        }
    }
}