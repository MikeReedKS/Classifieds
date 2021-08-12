using System;
using System.IO;
using Classifieds.DataAccess;
using Classifieds.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

//I didn't fully implement testing of all services like I typically would. I then add running tests to the build pipeline in Azure DevOps
//to assure that any changes made do not affect backwards compatibility.
//I did add one just to show the process. 

namespace ClassifiedsTests.Services
{
    [TestClass()]
    public class UserServiceTests
    {
        [TestMethod()]
        public void PostUserTest()
        {
            //Start with no user data by deleting the user data file
            var dataFolder = StorageHelper.GetDataFolder();
            File.Delete(Path.Combine(dataFolder, "userDB.json"));

            //Create first user to show that it works
            Guid result = UserDataAccess.CreateUser("MikeReedKS", "MikeReedKS@Gmail.com", "1234567");
            Assert.AreNotEqual("00000000-0000-0000-0000-000000000000", result.ToString(), "Error occurred");
            Assert.AreNotEqual("00000000-0000-0000-0000-000000000001", result.ToString(), "Duplicate should not be found");

            //Create the first user a second time to show that it will fail
            result = UserDataAccess.CreateUser("MikeReedKS", "MikeReedKS@Gmail.com", "1234567");
            Assert.AreNotEqual("00000000-0000-0000-0000-000000000000", result.ToString(), "Error occurred");
            Assert.AreEqual("00000000-0000-0000-0000-000000000001", result.ToString(), "Duplicate should have been found but was not");

            //Create second user to show that it works
            result = UserDataAccess.CreateUser("SarahReedKS", "SarahReedKS@Gmail.com", "NinjaWife");
            Assert.AreNotEqual("00000000-0000-0000-0000-000000000000", result, "Error occurred");
            Assert.AreNotEqual("00000000-0000-0000-0000-000000000001", result, "Duplicate should not be found");

            //Create the second user a second time to show it fail (Now there are multiple users in the DB)
            result = UserDataAccess.CreateUser("MikeReedKS", "MikeReedKS@Gmail.com", "1234567");
            Assert.AreNotEqual("00000000-0000-0000-0000-000000000000", result.ToString(), "Error occurred");
            Assert.AreEqual("00000000-0000-0000-0000-000000000001", result.ToString(), "Duplicate should have been found but was not");

            //Create a new user but using the email address from the first user to show that this will fail
            result = UserDataAccess.CreateUser("MichaelReedKS", "MikeReedKS@Gmail.com", "ABCDEFG");
            Assert.AreNotEqual("00000000-0000-0000-0000-000000000000", result.ToString(), "Error occurred");
            Assert.AreEqual("00000000-0000-0000-0000-000000000001", result.ToString(), "Duplicate should have been found but was not");
        }
    }
}