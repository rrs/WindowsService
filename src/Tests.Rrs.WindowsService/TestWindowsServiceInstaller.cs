using Rrs.WindowsService;

namespace Tests.Rrs.WindowsService;

[TestClass]
public sealed class TestWindowsServiceInstaller
{
    [TestMethod]
    public void TestFirstFailureNull()
    {
        var recoveryOptions = new RecoveryOptions(null, 1, 1);
        var output = WindowsServiceInstaller.GenerateResetString(recoveryOptions);
        Assert.AreEqual(@"""", output);
    }

    [TestMethod]
    public void TestSecondFailureNull()
    {
        var recoveryOptions = new RecoveryOptions(10, null, null);
        var output = WindowsServiceInstaller.GenerateResetString(recoveryOptions);
        Assert.AreEqual(@"restart/10///", output);
    }


    [TestMethod]
    public void TestSubsequentFailuresNull()
    {
        var recoveryOptions = new RecoveryOptions(10, 20, null);
        var output = WindowsServiceInstaller.GenerateResetString(recoveryOptions);
        Assert.AreEqual(@"restart/10/restart/20//", output);
    }

    [TestMethod]
    public void TestAllFalures()
    {
        var recoveryOptions = new RecoveryOptions(10, 20, 30);
        var output = WindowsServiceInstaller.GenerateResetString(recoveryOptions);
        Assert.AreEqual(@"restart/10/restart/20/restart/30/", output);
    }
}
