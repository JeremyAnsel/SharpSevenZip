using SharpSevenZip.Exceptions;

namespace SharpSevenZip.Tests;

[TestFixture]
public class LibraryManagerTests : TestBase
{
    [Test]
    public void SetNonExistant7zDllLocationTest()
    {
        Assert.Throws<SharpSevenZipLibraryException>(() => SharpSevenZipLibraryManager.SetLibraryPath("null"));
    }

    [Test]
    public void CurrentLibraryFeaturesTest()
    {
        var features = SharpSevenZipBase.CurrentLibraryFeatures;

        // Exercising more code paths...
        features = SharpSevenZipLibraryManager.CurrentLibraryFeatures;

        Assert.Multiple(() =>
        {
            Assert.That(features.HasFlag(LibraryFeature.ExtractAll), Is.True);
            Assert.That(features.HasFlag(LibraryFeature.CompressAll), Is.True);
            Assert.That(features.HasFlag(LibraryFeature.Modify), Is.True);
        });
    }
}
