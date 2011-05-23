using System;
using System.IO;
using NUnit.Framework;
using Should;
using VisualStudio.IO;

namespace Tests.Unit
{
    [TestFixture]
    class FileSystemTests
    {
        private string _path;

        private readonly string[] _paths = new []
                {
                    @"docs\personal\budget",
                    @"docs\addresses",
                    @"docs\resume",
                    @"music\jazz",
                    @"music\latin"
                };

        private readonly string[] _files = new[]
                {
                    @"docs\addresses\addresses.txt",
                    @"music\jazz\Herbie Hancock - Cantaloupe.mp3"
                };

        [SetUp]
        public void Setup()
        {
            _path = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            foreach (var path in _paths) Directory.CreateDirectory(Path.Combine(_path, path));
            foreach (var file in _files) File.WriteAllText(Path.Combine(_path, file), "yada yada yada");
        }

        [TearDown]
        public void Teardown()
        {
            Directory.Delete(_path, true);
        }

        [Test]
        public void Find_File_On_Disk()
        {
            var fileSystem = new FileSystem();

            var path = fileSystem.FindFirst(_path, "addresses.txt");
            path.HasValue.ShouldBeTrue();
            path.Value.ShouldEqual(Path.Combine(_path, _files[0]));

            path = fileSystem.FindFirst(_path, "Herbie Hancock - Cantaloupe.mp3");
            path.HasValue.ShouldBeTrue();
            path.Value.ShouldEqual(Path.Combine(_path, _files[1]));
        }
    }
}
