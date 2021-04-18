using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Boost.Infrastructure;

namespace Boost.Nuget
{
    public class PackageVersionService : IPackageVersionService
    {
        private readonly IBoostApplicationContext _applicationContext;

        public PackageVersionService(IBoostApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }

        public Dictionary<string, IEnumerable<PackageVersion>> GetVersionsByDirectory(
            string? root)
        {
            root = root ?? _applicationContext.WorkingDirectory.FullName;

            Dictionary<string, IEnumerable<PackageVersion>> result = new();

            foreach (var file in Directory.GetFiles(
                root,
                "Versions.props",
                SearchOption.AllDirectories))
            {
                try
                {
                    IEnumerable<PackageVersion>? versions = GetVersions(file);
                    result.Add(file, versions);
                }
                catch (Exception ex)
                {

                }
            }

            return result;
        }

        public IEnumerable<PackageVersion> GetVersions(string fileName)
        {
            var props = File.ReadAllText(fileName);
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(@"<?xml version=""1.0"" encoding=""UTF-8""?>" + props);
            XmlElement propertyGroup = xml["Project"]["PropertyGroup"];

            foreach (XmlNode child in propertyGroup.ChildNodes)
            {
                yield return new PackageVersion(child.Name, child.InnerText)
                {
                    Reference = child.Attributes["ref"]?.Value
                };
            }
        }
    }
}
